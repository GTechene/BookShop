namespace BookShop.domain.Catalog;

public class UnknownBookInInventoryException : Exception
{
    public UnknownBookInInventoryException(ISBN id) : base($"Book with the following ISBN is unknown : {id}") {}
}