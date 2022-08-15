using BookShop.domain;
using BookShop.domain.Checkout;
using BookShop.domain.Prices;

namespace BookShop.infra;

public class TransactionLog : ILogTransaction {

    private readonly List<Transaction> _transactions = new();

    public void Add(ReceiptId id, IEnumerable<ISBN> books, Price checkoutPrice)
    {
        _transactions.Add(new Transaction(
            id.ToString(),
            books.Select(isbn => isbn.ToString()).ToArray(),
            checkoutPrice.Amount,
            checkoutPrice.Currency
        ));
    }
    private record Transaction(string Id, string[] Books, decimal Price, string Currency);
}