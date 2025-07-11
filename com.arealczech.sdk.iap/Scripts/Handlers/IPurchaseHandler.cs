namespace Areal.SDK.IAP {
    public interface IPurchaseHandler {
        public string GetProductId();
        public EntryType GetEntryType();
        public PurchaseResult HandlePurchase(string payload);
    }
}
