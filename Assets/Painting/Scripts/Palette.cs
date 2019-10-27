using UnityEngine;

namespace Tannern.Painting
{
    public class Palette : MonoBehaviour
    {
        public Transform[] paintBlobTransforms;
        public GameObject paintBlobPrefab;

        private Transform paintBlobParent;
        private Color[] _loadedColors;

        public void Awake()
        {
            if (paintBlobParent == null)
            {
                paintBlobParent = new GameObject("Paint Blob Parent").transform;
                paintBlobParent.parent = transform;
            }
        }

        public void Start()
        {
            LoadWithColors(new Color[] { Color.red, Color.blue, Color.yellow, Color.cyan });
        }

        public void Unload()
        {
            _loadedColors = new Color[0];
            foreach (Transform child in paintBlobParent)
            {
                Destroy(child.gameObject);
            }
        }

        public void LoadWithColors(Color[] colors)
        {
            Unload();
            _loadedColors = (Color[])colors.Clone();
            int limit = Mathf.Min(_loadedColors.Length, paintBlobTransforms.Length);
            for (int i = 0; i < limit; i++)
            {
                Instantiate(
                    paintBlobPrefab,
                    paintBlobTransforms[i].position,
                    paintBlobTransforms[i].rotation,
                    paintBlobParent
                ).BroadcastMessage("SetColor", _loadedColors[i]);
            }
        }
    }
}
