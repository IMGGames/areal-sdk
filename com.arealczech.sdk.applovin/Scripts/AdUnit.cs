using System;
using UnityEngine;

namespace Areal.SDK {
    // ReSharper disable AccessToStaticMemberViaDerivedType
    public class AdUnit {
        internal readonly UnitType Type;
        internal readonly string Id;

        private bool _locked;

        public bool IsReady => !_locked && MaxSdk.IsInitialized() && Type switch {
            UnitType.Rewarded => MaxSdk.IsRewardedAdReady(Id),
            UnitType.Interstitial => MaxSdk.IsInterstitialReady(Id),
            _ => throw new ArgumentOutOfRangeException()
        };

        public event Action<bool> OnReadyStateChanged;

        internal void Load() {
            switch (Type) {
                case UnitType.Rewarded:
                    MaxSdk.LoadRewardedAd(Id);
                    break;
                case UnitType.Interstitial:
                    MaxSdk.LoadInterstitial(Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int _loadAttempt;

        internal void OnLoaded() {
            _loadAttempt = 0;
            OnReadyStateChanged?.Invoke(true);
        }

        internal void OnLoadFailed() => AppLovin.CallAfter(Mathf.Pow(2, Mathf.Min(6, _loadAttempt++)), Load);

        private Action<bool> _callback;

        private void CallbackAndReset(bool value) {
            try {
                _callback?.Invoke(value);
            } catch (Exception e) {
                Debug.LogException(e);
            }

            _callback = null;
            _locked = false;
            Load();
        }

        public void Show(Action<bool> callback = null) {
            if (!IsReady) {
                try {
                    _callback?.Invoke(false);
                } catch (Exception e) {
                    Debug.LogException(e);
                }

                return;
            }

            _locked = true;

            _callback = callback;

            switch (Type) {
                case UnitType.Rewarded:
                    MaxSdk.ShowRewardedAd(Id);
                    break;
                case UnitType.Interstitial:
                    MaxSdk.ShowInterstitial(Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            OnReadyStateChanged?.Invoke(false);
        }

        internal void OnRewardReceived() => CallbackAndReset(true);
        internal void OnClosed() => CallbackAndReset(false);

        public AdUnit(UnitType type, string id) {
            Type = type;
            Id = id;
        }
    }
}
