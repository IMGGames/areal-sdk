using System;
using System.Threading.Tasks;
using Unity.Usercentrics;

namespace Areal.SDK {
    public static class Usercentrics {
        public enum InitializationState {
            Uninitialized,
            Initializing,
            Initialized
        }

        public static InitializationState State { get; private set; } = InitializationState.Uninitialized;

        public static Task<UsercentricsReadyStatus> Initialize(string settingsId = null, string rulesetId = null) {
            var tcs = new TaskCompletionSource<UsercentricsReadyStatus>();

            try {
                if (State != InitializationState.Uninitialized) {
                    throw new Exception($"{nameof(Usercentrics)} is {(State == InitializationState.Initialized ? "already initialized" : "already initializing")}");
                }

                State = InitializationState.Initializing;

                bool settingsIdSet = settingsId != null;
                bool rulesetIdSet = rulesetId != null;

                if (!settingsIdSet && !rulesetIdSet) {
                    throw new ArgumentException("At least one id must be specified.");
                }

                if (settingsIdSet) {
                    Unity.Usercentrics.Usercentrics.Instance.SettingsID = settingsId;
                }

                if (rulesetIdSet) {
                    Unity.Usercentrics.Usercentrics.Instance.RulesetID = rulesetId;
                }

// #if UNITY_EDITOR
                // tcs.SetResult(new UsercentricsReadyStatus {consents = new List<UsercentricsServiceConsent>()});
// #else
                Unity.Usercentrics.Usercentrics.Instance.Initialize(
                    status => {
                        tcs.SetResult(status);
                        State = InitializationState.Initialized;
                    },
                    errorMessage => {
                        tcs.SetException(new Exception($"Failed to initialize Usercentrics: {errorMessage}"));
                        State = InitializationState.Uninitialized;
                    });
// #endif
            }
            catch (Exception e) {
                tcs.SetException(e);
                State = InitializationState.Uninitialized;
            }

            return tcs.Task;
        }
    }
}