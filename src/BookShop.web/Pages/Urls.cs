using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace BookShop.web.Pages; 

public static class Urls {

    public record Checkout(string[] Books) {
        private const string Uri = "/checkout";
        private const string BookQueryParam = "books";
        
        public override string ToString()
        {
            return QueryHelpers.AddQueryString(Uri, new[]
            {
                new KeyValuePair<string, StringValues>(BookQueryParam, this.Books)
            });
        }

        public static Checkout? Parse(Uri? uri)
        {
            if (uri is null || uri.LocalPath != Uri)
            {
                return null;
            }
            
            var query = QueryHelpers.ParseQuery(uri.Query);

            return query.TryGetValue(BookQueryParam, out var bookIdsQueryString) ? 
                new Checkout(bookIdsQueryString) : 
                new Checkout(Array.Empty<string>());
       } 
    }
    
    public record Catalog(string[] Books) {
        private const string Uri = "/catalog";
        private const string BookQueryParam = "books";
        
        public override string ToString()
        {
            return QueryHelpers.AddQueryString(Uri, new[]
            {
                new KeyValuePair<string, StringValues>(BookQueryParam, this.Books)
            });
        }

        public static Catalog? Parse(Uri? uri)
        {
            if (uri is null || uri.LocalPath != Uri)
            {
                return null;
            }
            
            var query = QueryHelpers.ParseQuery(uri.Query);

            return query.TryGetValue(BookQueryParam, out var bookIdsQueryString) ? 
                new Catalog(bookIdsQueryString) : 
                new Catalog(Array.Empty<string>());
        } 
    }
    
    public record Receipt(string ReceiptId) {
        private const string Uri = "/receipt";
        private const string ReceiptIdQueryParam = "receiptId";
        
        public override string ToString()
        {
            return QueryHelpers.AddQueryString(Uri, new[]
            {
                new KeyValuePair<string, StringValues>(ReceiptIdQueryParam, this.ReceiptId)
            });
        }

        public static Receipt? Parse(Uri? uri)
        {
            if (uri is null || uri.LocalPath != Uri)
            {
                return null;
            }
            
            var query = QueryHelpers.ParseQuery(uri.Query);

            if (!query.TryGetValue(ReceiptIdQueryParam, out var receiptIdQueryString))
            {
                throw new InvalidOperationException("Missing receiptId parameter");
            }
                
            return new Receipt(receiptIdQueryString);
        } 
    }
}