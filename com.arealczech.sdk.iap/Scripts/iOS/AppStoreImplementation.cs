using System;

namespace Areal.SDK.IAP {
    internal class AppStoreImplementation : AbstractStoreImplementation {
        internal override void Initialize(Action callback, Action<ITransaction> transactionProcessor, Action<string> onTransactionFail) {
            throw new NotImplementedException();
        }

        internal override void Purchase(string productId) {
            throw new NotImplementedException();
        }

        internal override void ConfirmTransaction(ITransaction transaction) {
            throw new NotImplementedException();
        }

        internal override void RestorePurchases(Action<RestoreResult> callback = null) {
            throw new NotImplementedException();
        }

        internal override void PullUnconfirmedTransactions(Action<ITransaction[]> callback) {
            throw new NotImplementedException();
        }
    }
}