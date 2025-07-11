namespace Areal.SDK.IAP.Stores {
    public class AppStoreTransaction : ITransaction {
        public string Id;
        public string GetProductId() => Id;
    }
}