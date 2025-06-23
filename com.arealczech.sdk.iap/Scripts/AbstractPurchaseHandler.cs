namespace Areal.SDK.IAP {
    public abstract class AbstractPurchaseHandler {
        internal readonly string Id;

        internal abstract EntryType GetEntryType();
        internal abstract void Handle(string payload);

        protected AbstractPurchaseHandler(string id) {
            Id = id;
        }
    }
}
