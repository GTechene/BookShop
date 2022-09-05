using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace BookShop.web.Pages; 

public static class Urls {

    public record Checkout(string[] Books) {
        private const string Uri = "/checkout";
        
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? UserName { get; init; }
        public string? Email { get; init; }
        
        public bool? SameAddressForShippingAsBilling { get; init; }
        
        public string? BillingAddress { get; init; }
        public string? BillingAddress2 { get; init; }
        public string? BillingCountry { get; init; }
        public string? BillingState { get; init; }
        public string? BillingZipCode { get; init; }
        
        
        public string? ShippingAddress { get; init; }
        public string? ShippingAddress2 { get; init; }
        public string? ShippingCountry { get; init; }
        public string? ShippingState { get; init; }
        public string? ShippingZipCode { get; init; }
        
        public string? Payment { get; init; }
        
        public bool? Redirected { get; init; }
        
        public string? BaseUrl { get; init; }

        public override string ToString()
        {
            var queryParams = new Dictionary<string, StringValues>();
            
            var setup = 
                AddQueryParam(x => x.Books) +
                AddQueryParam(x => x.FirstName) +
                AddQueryParam(x => x.LastName) +
                AddQueryParam(x => x.UserName) +
                AddQueryParam(x => x.Email) +
                AddQueryParam(x => x.Payment) +
                AddQueryParam(x => x.SameAddressForShippingAsBilling) +
                AddQueryParam(x => x.BillingAddress) + 
                AddQueryParam(x => x.BillingAddress2) + 
                AddQueryParam(x => x.BillingCountry) + 
                AddQueryParam(x => x.BillingState) + 
                AddQueryParam(x => x.BillingZipCode) +
                AddQueryParam(x => x.ShippingAddress) + 
                AddQueryParam(x => x.ShippingAddress2) + 
                AddQueryParam(x => x.ShippingCountry) + 
                AddQueryParam(x => x.ShippingState) + 
                AddQueryParam(x => x.ShippingZipCode) +
                AddQueryParam(x => x.Redirected);

            setup(queryParams);
            
            return QueryHelpers.AddQueryString($"{BaseUrl?.TrimEnd('/')}{Uri}", queryParams);
        }
        private Action<Dictionary<string, StringValues>> AddQueryParam(Expression<Func<Checkout, object?>> property) => queryString => {
            var propertyValue = property.Compile().Invoke(this);

            if (propertyValue is null)
            {
                return;
            }
            
            LambdaExpression lambda = property;
            var memberExpression = lambda.Body is UnaryExpression expression
                ? (MemberExpression)expression.Operand
                : (MemberExpression)lambda.Body;

            var info = (PropertyInfo)memberExpression.Member;

            if(propertyValue is string[] strings) {
                queryString[info.Name.ToLowerInvariant()] = strings;
            }
            else
            {
                queryString[info.Name.ToLowerInvariant()] = propertyValue.ToString();
            }
            
            
        };
        
        private static string[] ReadQueryParamList(
            Dictionary<string, StringValues> queryString,
            Expression<Func<Checkout, string[]>> property) 
        {
            LambdaExpression lambda = property;
            var memberExpression = lambda.Body is UnaryExpression expression
                ? (MemberExpression)expression.Operand
                : (MemberExpression)lambda.Body;

            var info = (PropertyInfo)memberExpression.Member;
            var name = info.Name.ToLowerInvariant(); 

            return queryString[name];
        }
        
        private static string? ReadQueryParam(
            Dictionary<string, StringValues> queryString,
            Expression<Func<Checkout, string?>> property)
        {
            return ReadQueryParam(queryString, property, x => x);
        }
        
        
        private static T? ReadQueryParam<T>(
            Dictionary<string, StringValues> queryString,
            Expression<Func<Checkout, T>> property,
            Func<string, T> converter) 
        {
            LambdaExpression lambda = property;
            var memberExpression = lambda.Body is UnaryExpression expression
                ? (MemberExpression)expression.Operand
                : (MemberExpression)lambda.Body;

            var info = (PropertyInfo)memberExpression.Member;
            var name = info.Name.ToLowerInvariant();

            return !queryString.ContainsKey(name) 
                ? default 
                : converter(queryString[name]);
        }

        public static Checkout? Parse(Uri? uri)
        {
            if (uri is null || uri.LocalPath != Uri)
            {
                return null;
            }
            
            var query = QueryHelpers.ParseQuery(uri.Query);

            return new Checkout(ReadQueryParamList(query, x => x.Books))
            {
                FirstName = ReadQueryParam(query, x=> x.FirstName),
                LastName = ReadQueryParam(query, x=> x.LastName),
                UserName = ReadQueryParam(query, x=> x.UserName),
                Email = ReadQueryParam(query, x=> x.Email),
                
                SameAddressForShippingAsBilling = ReadQueryParam(query, x=> x.SameAddressForShippingAsBilling, v => bool.Parse(v)), 
                
                Payment = ReadQueryParam(query, x=> x.Payment),
                
                BillingAddress = ReadQueryParam(query, x=> x.BillingAddress),
                BillingAddress2 = ReadQueryParam(query, x=> x.BillingAddress2),
                BillingCountry = ReadQueryParam(query, x=> x.BillingCountry),
                BillingState = ReadQueryParam(query, x=> x.BillingState),
                BillingZipCode = ReadQueryParam(query, x=> x.BillingZipCode),
                
                ShippingAddress = ReadQueryParam(query, x=> x.ShippingAddress),
                ShippingAddress2 = ReadQueryParam(query, x=> x.ShippingAddress2),
                ShippingCountry = ReadQueryParam(query, x=> x.ShippingCountry),
                ShippingState = ReadQueryParam(query, x=> x.ShippingState),
                ShippingZipCode = ReadQueryParam(query, x=> x.ShippingZipCode),
                
                Redirected = ReadQueryParam(query, x=> x.Redirected, v => bool.Parse(v)),
            };
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