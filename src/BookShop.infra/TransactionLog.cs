using BookShop.domain;
using BookShop.domain.Checkout;
using BookShop.domain.Prices;
using BookShop.domain.Receipt;

namespace BookShop.infra;

public class TransactionLog : IManageTransactions
{
    private readonly List<Transaction> _transactions = new();

    public void Add(ReceiptId id, IEnumerable<ISBN> books, Price checkoutPrice)
    {
        _transactions.Add(new Transaction(
            id.ToString(),
            books.Select(isbn => isbn).ToArray(),
            checkoutPrice.Amount,
            checkoutPrice.Currency
        ));
    }

    public Transaction? Find(ReceiptId id)
    {
        return _transactions.SingleOrDefault(transaction => transaction.Id == id.ToString());
    }
}