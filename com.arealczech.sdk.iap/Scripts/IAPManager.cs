using System;
using System.Collections.Generic;
using System.Linq;
using Areal.SDK.IAP.Stores;
using UnityEngine;

namespace Areal.SDK.IAP {
    public static class IAPManager {
        [Flags]
        public enum InitializationState {
            Uninitialized = 0,
            StoreInitializeCalled = 1 << 0,
            StoreInitialized = 1 << 1,
            ManagerInitializeCalled = 1 << 2,
            ManagerInitialized = 1 << 3,
            Initialized = StoreInitializeCalled | StoreInitialized | ManagerInitialized,
        }

        public static InitializationState State { get; private set; } = InitializationState.Uninitialized;

        private static readonly AbstractStoreImplementation Store =
#if UNITY_EDITOR
            new FakeStore();
#elif UNITY_IOS
            new AppStoreImplementation();
#endif

        private static readonly Dictionary<string, Action<PurchaseResult>> LocalCallbacks = new();
        internal static readonly Dictionary<string, IPurchaseHandler> PurchaseHandlers = new();

        public static void Initialize(params IPurchaseHandler[] handlers) {
            PayloadProvider.Load();

            try {
                foreach (IPurchaseHandler handler in handlers) {
                    string id = handler.GetProductId();

                    if (!PurchaseHandlers.TryAdd(id, handler)) {
                        throw new ArgumentException($"Duplicate handler '{id}'.");
                    }
                }
            }
            catch {
                PurchaseHandlers.Clear();
                throw;
            }

            Store.Initialize(() => {
                Store.PullUnconfirmedTransactions(transactions => {
                    foreach (var transaction in transactions) {
                        ProcessTransaction(transaction);
                    }

                    PayloadProvider.CleanNotPresent(transactions.Select(e => e.GetProductId()).ToArray());
                    // todo: set status = done
                });
            }, ProcessTransaction, OnTransactionFail);

            // todo: set & check initialized status
        }

        private static void ProcessTransaction(ITransaction transaction) {
            string productId = transaction.GetProductId();

            PurchaseResult result = PurchaseResult.Failed;

            try {
                result = PurchaseHandlers[productId].HandlePurchase(PayloadProvider.Get(productId));
                if (result == PurchaseResult.Failed) {
                    Debug.LogWarning($"Processing purchase '{productId}' failed");
                }
            }
            catch (Exception e) {
                Debug.LogError("Exception while processing purchase: " + e);
            }

            try {
                if (LocalCallbacks.TryGetValue(productId, out var callback)) {
                    callback?.Invoke(result);
                }
            }
            catch (Exception e) {
                Debug.LogError("Exception while invoking local callback: " + e);
            }

            LocalCallbacks.Remove(productId);
            if (result == PurchaseResult.Succeeded) {
                PayloadProvider.Remove(productId);
                Store.ConfirmTransaction(transaction);
            }
        }

        private static void OnTransactionFail(string productId) {
            LocalCallbacks.Remove(productId);
            PayloadProvider.Remove(productId);
        }

        public static void Purchase(string productId, string payload = null, Action<PurchaseResult> callback = null) {
            // Disallow concurrent purchases with the same productId.
            if (PayloadProvider.Contains(productId)) {
                try {
                    callback?.Invoke(PurchaseResult.Failed);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }

                return;
            }

            PayloadProvider.Set(productId, payload);
            LocalCallbacks[productId] = callback;
            Store.Purchase(productId);
        }

        public static void RestorePurchases(Action<RestoreResult> callback = null) => Store.RestorePurchases();
    }
}