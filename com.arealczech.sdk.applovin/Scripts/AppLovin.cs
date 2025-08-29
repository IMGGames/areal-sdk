using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Areal.SDK.Common.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Areal.SDK {
    // ReSharper disable AccessToStaticMemberViaDerivedType
    public static class AppLovin {
        public enum InitializationState {
            Uninitialized,
            Initializing,
            Initialized
        }

        private const string Prefix = "[Areal SDK " + nameof(AppLovin) + "]";

        public static InitializationState State { get; private set; } = InitializationState.Uninitialized;

        private static DummyMonoBehaviour _dummyMono;

        private static AdUnit[] _rewardedUnits;
        [CanBeNull] public static AdUnit Rewarded => _rewardedUnits[0];

        private static AdUnit[] _interstitialUnits;
        [CanBeNull] public static AdUnit Interstitial => _interstitialUnits[0];

        private static bool _maxSdkCallbackInitializedSet;
        private static TaskCompletionSource<bool> _initializeTaskCompletionSource;

        public static Task<bool> Initialize(params AdUnit[] adUnits) {
            var tcs = new TaskCompletionSource<bool>();

            try {
                if (adUnits == null) {
                    throw new ArgumentNullException(nameof(adUnits));
                }

                if (adUnits.Length <= 0) {
                    throw new ArgumentException("At least one ad unit must be provided.", nameof(adUnits));
                }

                if (State != InitializationState.Uninitialized) {
                    throw new Exception($"{nameof(AppLovin)} is {(State == InitializationState.Initialized ? "already initialized" : "already initializing")}");
                }

                State = InitializationState.Initializing;
                _initializeTaskCompletionSource = tcs;

                _rewardedUnits = adUnits.Where(e => e.Type == UnitType.Rewarded).ToArray();
                _interstitialUnits = adUnits.Where(e => e.Type == UnitType.Interstitial).ToArray();

                if (_dummyMono == null) {
                    _dummyMono = DummyMonoBehaviour.CreateUndestroyableInstance("Areal SDK AppLovin");
                }

                if (!_maxSdkCallbackInitializedSet) {
                    _maxSdkCallbackInitializedSet = true;
                    MaxSdkCallbacks.OnSdkInitializedEvent += OnInitialized;
                }

                MaxSdk.InitializeSdk();
            } catch (Exception e) {
                tcs.SetException(e);
                State = InitializationState.Uninitialized;
            }

            return tcs.Task;
        }

        private static void OnInitialized(MaxSdkBase.SdkConfiguration _) {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (id, _) => RouteOnLoaded(UnitType.Rewarded, id);
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (id, info) => RouteOnFailed(UnitType.Rewarded, id, info);
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (id, _, _) => RouteOnRewardReceived(UnitType.Rewarded, id);
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (id, _) => RouteOnClosed(UnitType.Rewarded, id);

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (id, _) => RouteOnLoaded(UnitType.Interstitial, id);
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (id, info) => RouteOnFailed(UnitType.Interstitial, id, info);
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (id, _) => RouteOnRewardReceived(UnitType.Interstitial, id);

            foreach (var unit in _rewardedUnits) {
                unit.Load();
            }
            
            foreach (var unit in _interstitialUnits) {
                unit.Load();
            }

            _initializeTaskCompletionSource?.SetResult(true);
        }

        #region Router

        private static void RouteOnLoaded(UnitType unitType, string unitId) => GetAdUnit(unitType, unitId)?.OnLoaded();

        private static void RouteOnFailed(UnitType unitType, string unitId, MaxSdkBase.ErrorInfo info) {
            Debug.LogError($"{Prefix} Failed to load {unitType} '{unitId}': {info}");
            GetAdUnit(unitType, unitId)?.OnLoadFailed();
        }

        private static void RouteOnRewardReceived(UnitType unitType, string unitId) => GetAdUnit(unitType, unitId)?.OnRewardReceived();
        private static void RouteOnClosed(UnitType unitType, string unitId) => GetAdUnit(unitType, unitId)?.OnClosed();

        private static AdUnit GetAdUnit(UnitType type, string id) {
            AdUnit[] units = type switch {
                UnitType.Rewarded => _rewardedUnits,
                UnitType.Interstitial => _interstitialUnits,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return units.FirstOrDefault(unit => unit.Id == id);
        }

        #endregion

        internal static void CallAfter(float seconds, Action action) {
            _dummyMono.StartCoroutine(WaitAndCall(seconds, action));
            return;

            static IEnumerator WaitAndCall(float seconds, Action action) {
                yield return new WaitForSecondsRealtime(seconds);
                action();
            }
        }
    }
}
