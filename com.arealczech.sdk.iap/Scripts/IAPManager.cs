using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Areal.SDK.IAP {
    public static class IAPManager {
        public enum InitializationState {
            Uninitialized,
            Initializing,
            Initialized
        }

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        private const string Environment = "development";
#else
        private const string Environment = "production";
#endif

        private const string PlayerPrefsKey = "Areal.SDK.IAP.IAPManager";

        private static readonly StoreListener Listener = new StoreListener(ProcessPurchase, OnInitialized, OnInitializeFailed, OnPurchaseFailed);

        public static InitializationState State { get; private set; } = InitializationState.Uninitialized;
        private static readonly Dictionary<string, Action<string>> Handlers = new Dictionary<string, Action<string>>();

        public static async Task Initialize(params IPurchaseHandler[] handlers) {
            if (State != InitializationState.Uninitialized) {
                Debug.LogError(
                    $"{nameof(IAPManager)} is {(State == InitializationState.Initialized ? "already initialized" : "already initializing")}");
            }

            State = InitializationState.Initializing;

            _currentPayload = PlayerPrefs.GetString(PlayerPrefsKey, "");

            try {
                foreach (var handler in handlers) {
                    string id = handler.GetId();

                    if (Handlers.ContainsKey(id)) {
                        throw new ArgumentException($"Duplicate handler with id '{id}'");
                    }

                    Handlers[id] = handler.HandlePurchase;
                }

                switch (UnityServices.State) {
                    case ServicesInitializationState.Uninitialized:
                        InitializationOptions options = new InitializationOptions().SetEnvironmentName(Environment);
                        await UnityServices.InitializeAsync(options);
                        break;
                    case ServicesInitializationState.Initializing:
                        throw new Exception("Cannot initialize while UnityServices are initializing. " +
                                            "Either initialize IAPManager after UnityServices, or let it initialize services itself.");
                }

                ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                builder.AddProducts(handlers.Select(e => new ProductDefinition(e.GetId(), (ProductType)e.GetEntryType())));

                UnityPurchasing.Initialize(Listener, builder);
            }
            catch (Exception e) {
                OnInitializeFailed(e.ToString());
            }
        }

        public static bool PurchasingLocked { get; private set; }
        private static Action<PurchaseResult> _currentCallback;
        private static string _currentPayload;

        public static void Purchase(string id, string payload = "", Action<PurchaseResult> callback = null) {
            if (State != InitializationState.Initialized) {
                throw new InvalidOperationException($"{nameof(IAPManager)} is not initialized yet");
            }

            if (PurchasingLocked) {
                return;
            }

            PurchasingLocked = true;

            _currentPayload = payload;
            PlayerPrefs.SetString(PlayerPrefsKey, payload);
            _currentCallback = callback;

            _controller.InitiatePurchase(_controller.products.WithID(id));
        }

        private static PurchaseProcessingResult ProcessPurchase(string id) {
            Handlers[id](_currentPayload);

            if (_currentCallback != null) {
                _currentCallback(PurchaseResult.Succeeded);

                _currentCallback = null;
                _currentPayload = "";
                PlayerPrefs.SetString(PlayerPrefsKey, "");
            }

            PurchasingLocked = false;

            return PurchaseProcessingResult.Complete;
        }

        public static Product GetProduct(string id) {
            if (State != InitializationState.Initialized) {
                throw new InvalidOperationException($"{nameof(IAPManager)} is not initialized yet");
            }

            return _controller.products.WithID(id);
        }

        public static void RestorePurchases(Action<bool> callback) {
            if (State != InitializationState.Initialized) {
                throw new InvalidOperationException($"{nameof(IAPManager)} is not initialized yet");
            }
            
            _extensions.GetExtension<IAppleExtensions> ().RestoreTransactions ((result, message) => {
                if (!result) {
                    Debug.LogError($"Failed to restore purchases: {message}");
                }

                callback(result);
            });
        }

        private static void OnPurchaseFailed(string message) {
            Debug.LogError(message);
            PurchasingLocked = false;
        }

        private static IStoreController _controller;
        private static IExtensionProvider _extensions;

        private static void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
            _controller = controller;
            _extensions = extensions;
            State = InitializationState.Initialized;
        }

        private static void OnInitializeFailed(string message) {
            Handlers.Clear();
            State = InitializationState.Uninitialized;
            Debug.LogError("Initialization failed: " + message);
        }
    }
}
