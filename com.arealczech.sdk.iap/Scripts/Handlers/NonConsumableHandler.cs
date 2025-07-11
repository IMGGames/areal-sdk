using System;

namespace Areal.SDK.IAP {
    public class NonConsumableHandler : IPurchaseHandler {
        private readonly string _id;
        private readonly Func<PurchaseResult> _handler;

        public string GetProductId() => _id;
        public EntryType GetEntryType() => EntryType.NonConsumable;
        public PurchaseResult HandlePurchase(string _) => _handler();

        public NonConsumableHandler(string id, Func<PurchaseResult> handler) {
            _id = id;
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
    }
}
