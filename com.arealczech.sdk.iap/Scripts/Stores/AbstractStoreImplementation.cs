using System;

namespace Areal.SDK.IAP.Stores {
    internal abstract class AbstractStoreImplementation {
        internal abstract void Initialize(Action<ITransaction> transactionProcessor, Action<string> onTransactionFail, Action callback);
        internal abstract void Purchase(string productId);
        internal abstract void ConfirmTransaction(ITransaction transaction);
        internal abstract void RestorePurchases(Action<RestoreResult> callback = null);
        internal abstract void PullUnconfirmedTransactions(Action<ITransaction[]> callback);
    }
}