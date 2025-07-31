using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Areal.SDK.Healer.Checks {
    public class SpriteFullRectCheck : ICheck {
        private class CheckResult : ICheckResult {
            public readonly TextureImporter Importer;

            public string GetMessage() => Importer.assetPath + " has invalid mesh type";

            public CheckResult(TextureImporter importer) {
                Importer = importer;
            }
        }

        public IEnumerable<ICheckResult> Check() {
            var results = AssetDatabase.FindAssets("t:Texture").Select(guid =>
                AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter
            ).Where(importer => {
                if (importer == null || importer.textureType != TextureImporterType.Sprite || !importer.assetPath.ToLower().StartsWith("assets")) {
                    return false;
                }

                TextureImporterSettings settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);

                return settings.spriteMeshType != SpriteMeshType.FullRect;
            }).Select(importer => new CheckResult(importer));

            return results;
        }

        public void Fix(ICheckResult iCheckResult) {
            TextureImporter importer = ((CheckResult)iCheckResult).Importer;

            TextureImporterSettings settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);

            settings.spriteMeshType = SpriteMeshType.FullRect;
            importer.SetTextureSettings(settings);
            
            importer.SaveAndReimport();
        }
    }
}
