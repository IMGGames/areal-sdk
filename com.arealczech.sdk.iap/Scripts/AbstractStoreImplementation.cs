using System;

namespace Areal.SDK.IAP {
    public abstract class AbstractStoreImplementation {
        internal abstract void Initialize(Action callback, Action<ITransaction> transactionProcessor, Action<string> onTransactionFail);
        internal abstract void Purchase(string productId);
        internal abstract void ConfirmTransaction(ITransaction transaction);
        internal abstract void RestorePurchases();
        internal abstract void PullUnconfirmedTransactions(Action<ITransaction[]> callback);
    }
}