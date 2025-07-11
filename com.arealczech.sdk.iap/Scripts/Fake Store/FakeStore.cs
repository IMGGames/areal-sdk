using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Areal.SDK.IAP {
    public class FakeStore : AbstractStoreImplementation {
        private const string PlayerPrefsKey = "Areal.SDK.IAP.FakeStore";

        private static SaveData _data;

        private static FakeStoreUI _ui;

        private static Action<ITransaction> _transactionProcessor;
        private static Action<string> _onTransactionFail;

        internal override void Initialize(Action callback, Action<ITransaction> transactionProcessor, Action<string> onTransactionFail) {
            _transactionProcessor = transactionProcessor;
            _onTransactionFail = onTransactionFail;

            if (PlayerPrefs.HasKey(PlayerPrefsKey)) {
                _data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(PlayerPrefsKey));
            }
            else {
                _data = new SaveData();
            }

            _ui = Object.Instantiate(Resources.Load<FakeStoreUI>("com.arealczech.sdk.iap/Fake Store UI"));
            Object.DontDestroyOnLoad(_ui);

            callback();
        }

        private readonly Queue<string> _purchaseQueue = new Queue<string>();

        internal override void Purchase(string productId) {
            _purchaseQueue.Enqueue(productId);
            if (_purchaseQueue.Count > 1) {
                return;
            }

            _ui.Show("Purchase\n" + productId, (ok, delay) => {
                _purchaseQueue.Dequeue();
                if (_purchaseQueue.Count > 0) {
                    Purchase(_purchaseQueue.Dequeue());
                }
                    
                _ui.ShowNotification(text => {
                    return Enumerator();

                    IEnumerator Enumerator() {
                        FakeTransaction ft = null;

                        if (ok) {
                            ft = new FakeTransaction { productId = productId, uuid = Guid.NewGuid().ToString()[..8] };
                            _data.unfinishedTransactions.Add(ft);
                            Save();
                        }

                        float t = delay;
                        while (t >= 0) {
                            text.text = $"Purchasing\n{productId}\nin {t:F3}s{(ok ? ", transaction id: " + ft.uuid : "")}";
                            t -= Time.unscaledDeltaTime;
                            yield return null;
                        }

                        if (ok) {
                            _transactionProcessor(ft);

                            text.text = $"Purchasing\n{productId}\n<color=\"lime\">succeeded</color>";
                        }
                        else {
                            _onTransactionFail(productId);

                            text.text = $"Purchasing\n{productId}\n<color=\"red\">failed</color>";
                        }
                    }
                });
            });
        }

        internal override void ConfirmTransaction(ITransaction transaction) {
            FakeTransaction fakeTransaction = (FakeTransaction)transaction;

            for (int i = 0; i < _data.unfinishedTransactions.Count; i++) {
                if (_data.unfinishedTransactions[i].uuid == fakeTransaction.uuid) {
                    _data.unfinishedTransactions.RemoveAt(i);
                    break;
                }
            }

            if (IAPManager.PurchaseHandlers[fakeTransaction.productId].GetEntryType() == EntryType.NonConsumable &&
                _data.boughtNonConsumables.All(tx => tx.productId != fakeTransaction.productId)) {
                _data.boughtNonConsumables.Add(fakeTransaction);
            }

            Save();

            Debug.Log($"Transaction {fakeTransaction.uuid} confirmed");
        }

        private static void Save() => PlayerPrefs.SetString(PlayerPrefsKey, JsonUtility.ToJson(_data));

        internal override void RestorePurchases() {
            _ui.Show("Restore purchases", (ok, delay) => {
                _ui.ShowNotification(text => {
                    return Enumerator();

                    IEnumerator Enumerator() {
                        float t = delay;
                        while (t >= 0) {
                            text.text = $"Restoring purchases in {t:F3}s";
                            t -= Time.unscaledDeltaTime;
                            yield return null;
                        }

                        if (ok) {
                            foreach (FakeTransaction transaction in _data.boughtNonConsumables) {
                                _transactionProcessor(transaction);
                            }

                            text.text = $"Restoring purchases <color=\"lime\">succeeded</color>, {_data.boughtNonConsumables.Count} transactions";
                        }
                        else {
                            text.text = "Restoring purchases <color=\"red\">failed</color>";
                        }
                    }
                });
            });
        }

        internal override void PullUnconfirmedTransactions(Action<ITransaction[]> callback) {
            _ui.ShowNotification(text => {
                return Enumerator();

                IEnumerator Enumerator() {
                    float t = 2;
                    while (t >= 0) {
                        text.text = $"Pulling unfinished transactions in {t:F3}s";
                        t -= Time.unscaledDeltaTime;
                        yield return null;
                    }

                    var transactions = _data.unfinishedTransactions.Select(e => (ITransaction)e).ToArray();

                    callback(transactions);

                    text.text = $"Pulling {transactions.Length} unfinished transactions done.";
                }
            });
        }

        [Serializable]
        private class SaveData {
            public List<FakeTransaction> boughtNonConsumables = new List<FakeTransaction>();
            public List<FakeTransaction> unfinishedTransactions = new List<FakeTransaction>();
        }
    }
}