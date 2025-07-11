using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Areal.SDK.IAP {
    internal class FakeStoreUI : MonoBehaviour {
        [Header("Confirmation")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private Text title;
        [SerializeField] private Slider delaySlider;
        [SerializeField] private Text delayText;

        [Header("Confirmation")]
        [SerializeField] private Transform notificationsContainer;
        [SerializeField] private GameObject notificationPrefab;

        public void OnDelayChanged() {
            delayText.text = $"Response delay: {(Mathf.Round(delaySlider.value * 1000) / 1000).ToString(CultureInfo.InvariantCulture)}s (realtime)";
        }

        private Action<bool, float> _callback;

        public void Show(string titleText, Action<bool, float> callback) {
            title.text = titleText;
            _callback = callback;
            canvas.SetActive(true);
        }

        public void Confirm(bool ok) {
            canvas.SetActive(false);
            _callback(ok, delaySlider.value);
        }

        public void ShowNotification(Func<Text, IEnumerator> customRoutine) {
            GameObject notification = Instantiate(notificationPrefab, notificationsContainer);
            Text text = notification.GetComponentInChildren<Text>();

            IEnumerator Routine() {
                yield return customRoutine(text);
                yield return new WaitForSecondsRealtime(2);
                Destroy(notification);
            }

            StartCoroutine(Routine());
        }
    }
}