using System;

namespace Areal.SDK.IAP {
    public class NonConsumableHandler : IPurchaseHandler {
        private readonly string _id;
        private readonly Action _handler;

        public string GetId() => _id;
        public EntryType GetEntryType() => EntryType.Consumable;
        public void HandlePurchase(string _) => _handler();

        public NonConsumableHandler(string id, Action handler) {
            _id = id;
            _handler = handler;
        }
    }
}
