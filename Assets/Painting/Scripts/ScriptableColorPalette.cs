using UnityEngine;

namespace Tannern
{

    [CreateAssetMenu(fileName = "ColorPalette", menuName = "Scriptable Objects/Color Palette")]
    public class ScriptableColorPalette : ScriptableObject
    {
        public uint gridSize = 3;
        public Color emptyColor = Color.black;
        public Color[] colors = new Color[0];
        public uint textureSize = 128;

        public void ApplyToTexture2D(Texture2D tex)
        {
            tex.Resize((int)textureSize, (int)textureSize);
            int cellSize = (int)textureSize / (int)gridSize;
            int i = 0;
            for (int y = 0; y < gridSize; y++)
                for (int x = 0; x < gridSize; x++)
                    FillRect(tex, colors[i++], new Rect
                    {
                        x = x * cellSize,
                        y = textureSize - y * cellSize - cellSize,
                        width = cellSize,
                        height = cellSize
                    });
        }

        public static void FillRect(Texture2D tex, Color color, Rect rect)
        {
            for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
                for (int x = (int)rect.xMin; x < (int)rect.xMax; x++)
                    tex.SetPixel(x, y, color);
        }
    }

}
