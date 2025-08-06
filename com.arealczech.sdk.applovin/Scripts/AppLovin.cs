using System;
using System.Collections;
using System.Threading.Tasks;
using Areal.SDK.Common.Utils;
using UnityEngine;

namespace Areal.SDK {
    // ReSharper disable AccessToStaticMemberViaDerivedType
    public static class AppLovin {
        private const string Prefix = "[Areal SDK " + nameof(AppLovin) + "]";

        private static DummyMonoBehaviour _dummyMono;

        public static Task<bool> Initialize(string rewardedAdUnitId) {
            if (_dummyMono == null) {
                _dummyMono = DummyMonoBehaviour.CreateUndestroyableInstance("Areal SDK AppLovin");
            }

            var tcs = new TaskCompletionSource<bool>();

            try {
                MaxSdkCallbacks.OnSdkInitializedEvent += _ => {
                    tcs.TrySetResult(true);

                    InitializeRewarded(rewardedAdUnitId);
                };

                MaxSdk.SetHasUserConsent(true);
                MaxSdk.SetDoNotSell(true);
                MaxSdk.InitializeSdk();
            }
            catch (Exception e) {
                tcs.TrySetException(e);
            }

            return tcs.Task;
        }

        #region Rewarded

        private static bool IsRewardedAdReady => MaxSdk.IsInitialized() && MaxSdk.IsRewardedAdReady(_rewardedUnitId) && !_adLocked;
        private static bool _adLocked;

        private static string _rewardedUnitId;

        private static Action<bool> _rewardedCallback;

        private static void InitializeRewarded(string unitId) {
            _rewardedUnitId = unitId;

            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedLoaded;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedReceived;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedClosed;
            LoadRewarded();
        }

        private static void LoadRewarded() {
            MaxSdk.LoadRewardedAd(_rewardedUnitId);
        }

        private static int _rewardedLoadAttempt;

        private static void OnRewardedLoadFailed(string unitId, MaxSdkBase.ErrorInfo info) {
            _rewardedLoadAttempt++;
            float delay = Mathf.Pow(2, Mathf.Min(6, _rewardedLoadAttempt));

            Debug.LogWarning($"{Prefix} Failed to load rewarded ad unit '{unitId}': {info}, retrying in {delay} seconds.");

            _dummyMono.StartCoroutine(WaitAndCall(delay, LoadRewarded));
        }

        private static void OnRewardedLoaded(string unitId, MaxSdkBase.AdInfo info) {
            _rewardedLoadAttempt = 0;
        }

        private static void CallbackAndReset(bool ok) {
            try {
                _rewardedCallback?.Invoke(ok);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }

            _rewardedCallback = null;
            _adLocked = false;
            
            LoadRewarded();
        }

        private static void OnRewardedReceived(string unitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo info) => CallbackAndReset(true);
        private static void OnRewardedClosed(string unitId, MaxSdkBase.AdInfo info) => CallbackAndReset(false);

        public static void ShowRewarded(Action<bool> callback) {
            if (!IsRewardedAdReady) {
                callback(false);
                return;
            }
            
            _adLocked = true;

            _rewardedCallback = callback;

            MaxSdk.ShowRewardedAd(_rewardedUnitId);
        }

        #endregion

        private static IEnumerator WaitAndCall(float seconds, Action action) {
            yield return new WaitForSecondsRealtime(seconds);
            action();
        }
    }
}
