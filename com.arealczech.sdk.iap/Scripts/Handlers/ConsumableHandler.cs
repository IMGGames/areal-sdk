namespace Areal.SDK.IAP {
    public class ConsumableHandler : IPurchaseHandler {
        public delegate PurchaseResult PurchaseHandlerDelegate(string payload);

        private readonly string _id;
        private readonly PurchaseHandlerDelegate _handler;

        public string GetProductId() => _id;
        public EntryType GetEntryType() => EntryType.Consumable;
        public PurchaseResult HandlePurchase(string payload) => _handler?.Invoke(payload) ?? PurchaseResult.Succeeded;

        public ConsumableHandler(string id, PurchaseHandlerDelegate handler) {
            _id = id;
            _handler = handler;
        }
    }
}