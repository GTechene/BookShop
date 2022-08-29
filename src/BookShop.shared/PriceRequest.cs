namespace BookShop.shared; 

// TODO : instead of providing the list of ISBN, repeating multiple times the same isbn, it feels more intuitive to provide a json like : { "ISBN" : quantity } which can be deserialized as a dictionary
public record PriceRequest(string[] Books, string Currency);