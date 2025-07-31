using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Areal.SDK.Healer.Checks {
    public class AndroidDefaultTextureCompressionCheck : ICheck {
        private class CheckResult : ICheckResult {
            public readonly TextureImporter Importer;

            public string GetMessage() => Importer.assetPath + " has invalid compression";

            public CheckResult(TextureImporter importer) {
                Importer = importer;
            }
        }

        public IEnumerable<ICheckResult> Check() {
            var results = AssetDatabase.FindAssets("t:Texture").Select(guid =>
                AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter
            ).Where(importer => {
                if (importer == null || importer.textureType != TextureImporterType.Default || !importer.assetPath.ToLower().StartsWith("assets")) {
                    return false;
                }

                var androidSettings = importer.GetPlatformTextureSettings("Android");

                return !(androidSettings.overridden && androidSettings.format == TextureImporterFormat.ASTC_6x6);
            }).Select(importer => new CheckResult(importer));

            return results;
        }

        public void Fix(ICheckResult iCheckResult) {
            TextureImporter importer = ((CheckResult)iCheckResult).Importer;

            var androidSettings = importer.GetPlatformTextureSettings("Android");
            androidSettings.overridden = true;
            androidSettings.format = TextureImporterFormat.ASTC_6x6;

            importer.SetPlatformTextureSettings(androidSettings);
            importer.SaveAndReimport();
        }
    }
}
