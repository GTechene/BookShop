using System.Collections;
using System.Collections.Immutable;

namespace BookShop.domain.Pricing;


public class Cart : IEnumerable<ISBN>
{
    private ImmutableList<ISBN> Books { get; init; }=  ImmutableList<ISBN>.Empty;
    public int Size => Books.Count;

    public static readonly Cart Empty = new();
    
    private Cart() {}

    public bool IsEmpty => Size == 0; 
    
    public Cart Add(params ISBN[] titles)
    {
        return new Cart
        {
            Books = Books.AddRange(titles)
        };
    }

    public Cart Remove(params ISBN[] titles)
    {
        var books = titles.Aggregate(Books, (books, book) => books.Remove(book)); 
        
        return new Cart
        {
            Books = books
        };
    }
       
    public IEnumerator<ISBN> GetEnumerator()
    {
        return Books.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}