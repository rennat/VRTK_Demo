using UnityEngine;

namespace Tannern.Painting
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class PaintBlob : MonoBehaviour
    {
        public Mesh[] meshes;

        public void Awake()
        {
            Mesh mesh = meshes[Random.Range(0, meshes.Length - 1)];
            gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        public void SetColor(Color color)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }
}