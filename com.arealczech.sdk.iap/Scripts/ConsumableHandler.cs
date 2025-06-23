using System;

namespace Areal.SDK.IAP {
    public class ConsumableHandler : AbstractPurchaseHandler {
        private readonly Action<string> _handler;

        override internal EntryType GetEntryType() => EntryType.Consumable;
        override internal void Handle(string payload) => _handler(payload);

        public ConsumableHandler(string id, Action<string> handler) : base(id) {
            _handler = handler;
        }
    }
}
