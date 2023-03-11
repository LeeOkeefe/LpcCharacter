using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    internal sealed class LpcSpritesheetProcessor : AssetPostprocessor
    {
        private readonly Vector2 _offset = new();
        private readonly Vector2 _size = new(64, 64);
        private readonly Vector2 _padding = new();
    
        private void OnPreprocessTexture()
        {
            if (!IsLpcSpritesheet())
            {
                return;
            }
        
            var textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            textureImporter.mipmapEnabled = false;
            textureImporter.filterMode = FilterMode.Point;
            textureImporter.spritePixelsPerUnit = 64;
        }

        public void OnPostprocessTexture(Texture2D texture)
        {
            if (!IsLpcSpritesheet())
            {
                return;
            }
        
            var rects = InternalSpriteUtility.GenerateGridSpriteRectangles(texture, _offset, _size, _padding, false);
        
            var textureImporter = (TextureImporter)assetImporter;
            textureImporter.spritesheet = rects.Select((r, i) => new SpriteMetaData { rect = r, name = $"{texture.name}_{i}"}).ToArray();
        }

        private bool IsLpcSpritesheet()
        {
            return assetPath.StartsWith("Assets/Sprites/LPC") && assetPath.EndsWith(".png");
        }
    }
}