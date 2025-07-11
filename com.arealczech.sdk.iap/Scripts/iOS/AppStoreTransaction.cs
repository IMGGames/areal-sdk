namespace Areal.SDK.IAP {
    public class AppStoreTransaction : ITransaction {
        public string Id;
        public string GetProductId() => Id;
    }
}