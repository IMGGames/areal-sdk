using System;

namespace Areal.SDK.IAP {
    public class ConsumableHandler : IPurchaseHandler {
        private readonly string _id;
        private readonly Action<string> _handler;

        public string GetId() => _id;
        public EntryType GetEntryType() => EntryType.Consumable;
        public void HandlePurchase(string payload) => _handler?.Invoke(payload);

        public ConsumableHandler(string id, Action<string> handler) {
            _id = id;
            _handler = handler;
        }
    }
}
