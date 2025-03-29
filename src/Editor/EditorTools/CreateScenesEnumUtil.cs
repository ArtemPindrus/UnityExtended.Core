using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UnityEditor;
using UnityEngine;

namespace UnityExtended.Core.Editor.EditorTools {
    public static class CreateScenesEnumUtil {
        public const string DefaultEnumFolderPath = "Resources";
        public const string DefaultEnumFileName = "Scenes.cs";

        [MenuItem("UnityExtended/" + nameof(CreateScenesEnum))]
        public static void CreateScenesEnum() {
            var classBuilder = new IndentedStringBuilder();
            classBuilder
                .AppendLine("namespace UnityExtended.Core.Utilities {")
                .IncrementIndent()
                .AppendLine("public enum Scenes {")
                .IncrementIndent();
            
            var assetsPath = Application.dataPath;
            var projectFolder = Path.GetDirectoryName(assetsPath);

            var buildSettingsPath = $"{projectFolder}/ProjectSettings/EditorBuildSettings.asset";
            var text = File.ReadAllLines(buildSettingsPath);

            for (var i = 0; i < text.Length; i++) {
                var line = text[i];

                if (line.Contains("- enabled:")) {
                    string sceneLine = text[i + 1];

                    int leftInd = sceneLine.LastIndexOf("/", StringComparison.Ordinal) + 1;
                    int rightInd = sceneLine.LastIndexOf(".", StringComparison.Ordinal);
                    
                    var sceneName = sceneLine[leftInd..rightInd].Replace(" ", "");

                    if (line.Contains("0")) sceneName += "DEACTIVATED";

                    classBuilder.AppendLine($"{sceneName},");
                }
            }

            classBuilder.DecrementIndent().AppendLine("}")
                .DecrementIndent().AppendLine("}");

            var directoryPath = Path.Combine(assetsPath, DefaultEnumFolderPath);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(assetsPath, DefaultEnumFolderPath, DefaultEnumFileName);
            File.WriteAllText(filePath, classBuilder.ToString());
            
            AssetDatabase.Refresh();
        }
    }
}
