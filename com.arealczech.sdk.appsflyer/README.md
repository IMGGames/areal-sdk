# Areal SDK AppsFlyer

This package provides a wrapper around [AppsFlyer Analytics](https://github.com/AppsFlyerSDK/appsflyer-unity-plugin).

```
https://github.com/IMGGames/areal-sdk.git?path=/com.arealczech.sdk.appsflyer
```

## Dependencies

> [!IMPORTANT]
> Unfortunately, Unity doesn't support git packages for packages, so you have to install them manually via Package Manager.
>
> ¯\\\_(ツ)\_/¯

-   Areal SDK Common
    ```
    https://github.com/IMGGames/areal-sdk.git?path=/com.arealczech.sdk.common
    ```
-   AppsFlyer Analytics
    ```
    https://github.com/AppsFlyerSDK/appsflyer-unity-plugin.git#upm
    ```
-   _External Dependency Manager - installed automatically_

<br/>

Alternatively, you can add them directly to your `manifest.json`:

```json
"com.appsflyer.unity": "https://github.com/AppsFlyerSDK/appsflyer-unity-plugin.git#upm",
"com.arealczech.sdk.common": "https://github.com/IMGGames/areal-sdk.git?path=/com.arealczech.sdk.common"
```
