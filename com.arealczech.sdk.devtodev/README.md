# Areal SDK DevToDev

This package provides a wrapper around [DevToDev Analytics](https://github.com/devtodev-analytics/package_Analytics.git).

## Dependencies

> [!IMPORTANT]
> Unfortunately, Unity doesn't support git packages for packages, so you have to install them manually via Package Manager.
>
> ¯\\\_(ツ)\_/¯

-   Areal SDK Common
    ```
    https://github.com/IMGGames/areal-sdk.git?path=/com.arealczech.sdk.common
    ```
-   DevToDev Analytics
    ```
    https://github.com/devtodev-analytics/package_Analytics.git
    ```

<br/>

Alternatively, you can add them directly to your `manifest.json`:

```json
"com.devtodev.sdk.analytics": "https://github.com/devtodev-analytics/package_Analytics.git",
"com.arealczech.sdk.common": "https://github.com/IMGGames/areal-sdk.git?path=/com.arealczech.sdk.common"
```
