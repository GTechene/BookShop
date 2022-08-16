using BookShop.domain.Catalog;
using BookShop.domain.Checkout.Payment;
using BookShop.domain.Prices;
using BookShop.domain.Pricing;

namespace BookShop.domain.Checkout;

// TODO : do we add a port interface here ?
public class CheckoutService
{
    // TODO: Something feels wrong here. Maybe we should not directly reference the Catalog domain ?

    private readonly ILockCatalog _catalogLock;
    private readonly IUpdateInventory _inventoryManager;
    private readonly IProvideBookMetadata _bookMetadataProvider;
    private readonly IProcessPayment _paymentService;

    private readonly CartPricer _pricer;
    private readonly ILogTransaction _transactionLog;
    private readonly IProvideInventory _inventoryProvider;

    public CheckoutService(
        CartPricer pricer,
        IProvideBookMetadata bookMetadataProvider,
        IUpdateInventory inventoryManager,
        ILockCatalog catalogLock,
        ILogTransaction transactionLog,
        IProcessPayment paymentService,
        IProvideInventory inventoryProvider)
    {
        _pricer = pricer;
        _bookMetadataProvider = bookMetadataProvider;
        _inventoryManager = inventoryManager;
        _catalogLock = catalogLock;
        _transactionLog = transactionLog;
        _paymentService = paymentService;
        _inventoryProvider = inventoryProvider;
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
        _inventoryManager.Remove(books);
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
        var books = new List<(BookReference Book, Quantity Quantity)>();

        var unavailableBooks = new List<(ISBN isbn, Book? Book)>();

        foreach (var (isbn, count) in checkout.Cart
                     .GroupBy(isbn => isbn)
                     .ToDictionary(keySelector: group => group.Key, elementSelector: group => group.Count()))
        {
            var bookReference = _bookMetadataProvider.Get(isbn);

            if (bookReference is null)
            {
                unavailableBooks.Add((isbn, new UnknownBook(isbn)));
                continue;
            }

            var inventory = _inventoryProvider.Get(new List<BookReference> {bookReference}).Single();

            if (inventory.Quantity < count)
            {
                unavailableBooks.Add((isbn, new Book(bookReference, inventory.Quantity)));
                continue;
            }

            books.Add((bookReference, count));
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
        // I don't like the fact that the Checkout domain calls the Pricing domain
        // Maybe we can improve this using a hash or something
        // For instance, if the pricing service creates a salted hash based on the book list and the price
        // Then when we checkout, we juste have to recalculate this hash with the same book list and price and make sure it has not changed
        // The goal here is to ensure that nobody request a price different from the one created by the pricer.

        var (actualPrice, _) = _pricer.ComputePrice(checkout.Cart, checkout.Price.Currency);

        if (checkout.Price != actualPrice)
        {
            throw new InvalidCheckoutPrice(actualPrice, checkout.Price);
        }
    }
}