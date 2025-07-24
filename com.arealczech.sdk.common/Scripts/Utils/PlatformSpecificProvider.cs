namespace Areal.SDK.Common.Utils {
    public class PlatformSpecificProvider<T> {
        public readonly T Value;

        public static implicit operator T(PlatformSpecificProvider<T> p) {
            return p.Value;
        }

        public PlatformSpecificProvider(
            // ReSharper disable UnusedParameter.Local
            T editor = default,
            T android = default,
            T ios = default,
            T webGL = default,
            T standaloneWindows = default,
            T standaloneOSX = default,
            T standaloneLinux = default,
            T windowsStoreApp = default,
            T fallback = default
            // ReSharper restore UnusedParameter.Local
        ) {
            Value =
#if UNITY_EDITOR
                editor
#elif UNITY_ANDROID
                android
#elif UNITY_IOS
                ios
#elif UNITY_WEBGL
                webGL
#elif UNITY_STANDALONE_WIN
                standaloneWindows
#elif UNITY_STANDALONE_OSX
                standaloneOSX
#elif UNITY_STANDALONE_LINUX
                standaloneOSX
#elif UNITY_WSA
                windowsStoreApp
#else
                fallback
#endif
                ;
        }
    }
}
