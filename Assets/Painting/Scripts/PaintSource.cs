using UnityEngine;
using UnityEngine.Events;

namespace Tannern.Painting
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Renderer))]
    public class PaintSource : MonoBehaviour
    {
        [System.Serializable]
        public class ColorChangeEvent : UnityEvent<Color> { }

        public Color color;
        public bool canPickup = false;
        public ColorChangeEvent OnColorChange;
        public bool SetColorOnAwake = true;

        private void Awake()
        {
            SetColor(color);
        }

        public void SetColor(Color color)
        {
            this.color = color;
            GetComponent<Renderer>().material.color = color;
            OnColorChange.Invoke(color);
        }

        public void PickupColor(Color color)
        {
            if (canPickup)
                SetColor(color);
        }

        public void OnTriggerEnter(Collider other)
        {
            other.gameObject.BroadcastMessage("PickupColor", color, SendMessageOptions.DontRequireReceiver);
        }
    }
}