using System;

namespace Areal.SDK.IAP {
    public class NonConsumableHandler : AbstractPurchaseHandler {
        private readonly Action _handler;

        override internal EntryType GetEntryType() => EntryType.Consumable;
        override internal void Handle(string _) => _handler();

        public NonConsumableHandler(string id, Action handler) : base(id) {
            _handler = handler;
        }
    }
}
