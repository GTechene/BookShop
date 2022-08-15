namespace BookShop.domain.Catalog;

public interface IProvideCatalog
{
    Catalog Get();
}

public interface ILockCatalog
{
    void Lock();
    void UnLock();
}