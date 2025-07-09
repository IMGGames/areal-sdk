using System;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Areal.SDK.IAP {
    internal class StoreListener : IDetailedStoreListener {
        internal delegate PurchaseProcessingResult PurchaseProcessor(string id);

        private readonly PurchaseProcessor _processor;
        private readonly Action<IStoreController, IExtensionProvider> _onInitialized;
        private readonly Action<string> _onInitializeFailed;
        private readonly Action<Product, string> _onPurchaseFailed;

        public void OnInitializeFailed(InitializationFailureReason error) => _onInitializeFailed(error.ToString());
        public void OnInitializeFailed(InitializationFailureReason error, string message) => _onInitializeFailed($"{error}: {message}");

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions) => _onInitialized(controller, extensions);

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent) => _processor(purchaseEvent.purchasedProduct.definition.id);

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
            _onPurchaseFailed(product, failureReason.ToString());

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription) =>
            _onPurchaseFailed(product, failureDescription.ToString());

        public StoreListener(
            PurchaseProcessor processor,
            Action<IStoreController, IExtensionProvider> onInitialized,
            Action<string> onInitializeFailed,
            Action<Product, string> onPurchaseFailed) {
            _processor = processor;
            _onInitialized = onInitialized;
            _onInitializeFailed = onInitializeFailed;
            _onPurchaseFailed = onPurchaseFailed;
        }
    }
}
