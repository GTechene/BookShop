using BookShop.domain.Catalog;
using BookShop.domain.Checkout;
using BookShop.domain.Prices;

namespace BookShop.domain.Receipt;

public class ReceiptDetailsService : IProvideReceiptDetails
{
    private readonly IManageTransactions _transactionsManager;
    private readonly IProvideBookMetadata _metadataProvider;
    private readonly IProvideBookPrice _bookPricer;

    public ReceiptDetailsService(IManageTransactions transactionsManager, IProvideBookMetadata metadataProvider, IProvideBookPrice bookPricer)
    {
        _transactionsManager = transactionsManager;
        _metadataProvider = metadataProvider;
        _bookPricer = bookPricer;
    }

    public ReceiptDetails Get(ReceiptId id)
    {
        var transaction = _transactionsManager.Find(id);

        if (transaction == null)
        {
            return new UnknownReceipt();
        }

        var bookDetails = transaction.Books
            .GroupBy(isbn => isbn)
            .Select(group => 
            {
                var orderedQuantity = group.Count();

                var isbn = group.Key;
                var metadata = _metadataProvider.GetMetadataForReceipt(isbn);
                var unitPrice = _bookPricer.GetPrice(isbn, transaction.Currency);

                return new BookDetails(metadata.Title, metadata.Author, metadata.PictureUrl, orderedQuantity, unitPrice);
            })
            .ToArray();

        return new ReceiptDetails(bookDetails, new Price(transaction.Price, transaction.Currency));
    }
}