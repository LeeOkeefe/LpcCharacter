using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Directory = UnityEngine.Windows.Directory;

namespace Editor
{
    internal sealed class LpcCosmeticLibraryGenerator : EditorWindow
    {
        private const string BasePath = "Assets/Sprites/LPC/Cosmetics";
    
        [MenuItem("Tools/Generate LPC Cosmetic Libraries")]
        public static void Create()
        {
            var paths = AssetDatabase.GetAllAssetPaths().Where(p => p.StartsWith(BasePath) && p.Contains(".png"));

            foreach (var path in paths)
            {
                var library = CreateSpriteLibrary(path);
                
                AssetDatabase.CreateAsset(library, CreateSpriteLibraryPath(path));
                AssetDatabase.SaveAssets();
            }
        }

        private static string CreateSpriteLibraryPath(string path)
        {
            var pathSegments = path.Split('/');
            pathSegments[1] = "ScriptableObjects";

            for (var i = 0; i < pathSegments.Length; i++)
            {
                var pathConstruct = string.Join('/', pathSegments[..i]);
                if (!Directory.Exists(pathConstruct))
                {
                    Directory.CreateDirectory(pathConstruct);
                }
            }

            return string.Join('/', pathSegments).Replace(".png", ".asset");
        }

        private static SpriteLibraryAsset CreateSpriteLibrary(string path)
        {
            var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(path).Cast<Sprite>().ToArray();

            var library = CreateInstance<SpriteLibraryAsset>();

            for (var i = 0; i < sprites.Length; i++)
            {
                library.AddCategoryLabel(sprites[i], GetCategoryName(i), $"{GetCategoryName(i)}_{i}");
            }

            return library;
        }

        private static string GetCategoryName(int index)
        {
            return index switch
            {
                < 28 => "Spellcast",
                < 60 => "Thrust",
                < 96 => "Walk",
                < 120 => "Slash",
                < 172 => "Shoot",
                < 178 => "Hurt",
                < 202 => "SlashOversize",
                _ => "Unknown"
            };
        }
    }
}