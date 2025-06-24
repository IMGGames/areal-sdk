namespace Areal.SDK.IAP {
    public interface IPurchaseHandler {
        public string GetId();
        public EntryType GetEntryType();
        public void HandlePurchase(string payload);
    }
}
