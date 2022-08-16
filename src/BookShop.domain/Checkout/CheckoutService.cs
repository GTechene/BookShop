﻿using BookShop.domain.Catalog;
using BookShop.domain.Checkout.Payment;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;

namespace BookShop.domain.Checkout;

public class CheckoutService {
    // TODO: Something feels wrong here. Maybe we should not directly reference the Catalog domain ?
    private readonly ILockCatalog _catalogLock;
    private readonly IUpdateCatalog _catalogManager;
    private readonly IProvideCatalog _catalogProvider;
    private readonly IProcessPayment _paymentService;

    private readonly CartPricer _pricer;
    private readonly ILogTransaction _transactionLog;

    public CheckoutService(
        CartPricer pricer,
        IProvideCatalog catalogProvider,
        IUpdateCatalog catalogManager,
        ILockCatalog catalogLock,
        ILogTransaction transactionLog,
        IProcessPayment paymentService)
    {
        _pricer = pricer;
        _catalogProvider = catalogProvider;
        _catalogManager = catalogManager;
        _catalogLock = catalogLock;
        _transactionLog = transactionLog;
        _paymentService = paymentService;
    }

    public Receipt ProcessCheckout(Checkout checkout)
    {
        CheckPrice(checkout);

        _catalogLock.Lock();

        var books = CheckBooksAreStillAvailable(checkout);

        ProcessPayment(checkout.Payment);

        var id = ReceiptId.Generate();

        SaveTransaction(id, checkout.Cart, checkout.Price);

        RemoveBooks(books);

        _catalogLock.UnLock();

        return new Receipt(
            id, books,
            checkout.Price);
    }

    private void RemoveBooks(IReadOnlyCollection<(BookReference Book, Quantity Quantity)> books)
    {
        _catalogManager.Remove(books);
    }

    private void ProcessPayment(Payment.Payment checkoutPayment)
    {
        var paymentReceipt = _paymentService.Process(checkoutPayment);

        if (paymentReceipt is PaymentReceipt.FailureReceipt failure)
        {
            throw new PaymentProcessFailed(failure.Reason);
        }
    }

    private void SaveTransaction(ReceiptId id, IEnumerable<ISBN> books, Price checkoutPrice)
    {
        _transactionLog.Add(id, books, checkoutPrice);
    }

    
    private IReadOnlyCollection<(BookReference Book, Quantity Quantity)> CheckBooksAreStillAvailable(Checkout checkout)
    {
        var catalog = _catalogProvider.Get();
        
        var books = new List<(BookReference Book, Quantity Quantity)>();

        var unavailableBooks = new List<(ISBN isbn, Book? Book)>();

        foreach (var (isbn, count) in checkout.Cart
                     .GroupBy(isbn => isbn)
                     .ToDictionary(keySelector: group => group.Key, elementSelector: group => group.Count()))
        {
            var book = catalog.Books.SingleOrDefault(book => book.Reference.Id == isbn);

            if (book is null || book.Quantity < count)
            {
                unavailableBooks.Add((isbn, book));
                continue;
            }

            books.Add((book.Reference, count));
        }

        if (unavailableBooks.Any())
        {
            throw new UnavailableBooks(unavailableBooks);
        }

        return books;
    }

    private void CheckPrice(Checkout checkout)
    {
        // TODO: not the best way to do this.
        // I don't like the fact that the Checkout Domin calls the Pricing domain
        // Maybe we can improve this using a hash or something
        // For instance, if the pricing service creates a salted hash based on the book list and the price
        // Then when we checkout, we juste have to recalculate this hash with the same book list and price and make sure it has not change
        // The goal here is to ensure that nobody request a price too low. 
        
        var (actualPrice, _) = _pricer.ComputePrice(checkout.Cart, checkout.Price.Currency);

        if (checkout.Price != actualPrice)
        {
            throw new InvalidCheckoutPrice(actualPrice, checkout.Price);
        }
    }
}