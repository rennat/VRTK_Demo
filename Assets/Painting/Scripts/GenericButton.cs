using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tannern
{
    [RequireComponent(typeof(Collider))]
    public class GenericButton : MonoBehaviour
    {
        public UnityEvent OnPress;
        public bool requireActuatorTag = true;
        public bool repeat = true;
        public float repeatMinPressesPerSecond = 2f;
        public float repeatMaxPressesPerSecond = 40f;
        public float repeatRampSeconds = 20f;

        private int _holdCount = 0;

        public bool CanActuate(Collider other)
        {
            if (requireActuatorTag)
                return other.gameObject.GetComponent<ButtonActuatorTag>() != null;
            else
                return true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (CanActuate(other))
            {
                if (repeat)
                    if (_holdCount == 0)
                    {
                        _holdCount += 1;
                        StartCoroutine(ButtonHoldCoroutine());
                    }
                else
                    OnPress.Invoke();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (CanActuate(other))
            {
                if (repeat)
                    _holdCount = Mathf.Max(0, _holdCount - 1);
            }
        }

        public IEnumerator ButtonHoldCoroutine()
        {
            float startTime = Time.time;
            float duration, rampAmount, pressesPerSecond, delay;
            while (_holdCount > 0)
            {
                OnPress.Invoke();
                duration = Time.time - startTime;
                rampAmount = Mathf.Clamp01(duration / repeatRampSeconds);
                pressesPerSecond = Mathf.Lerp(repeatMinPressesPerSecond, repeatMaxPressesPerSecond, rampAmount);
                delay = 1f / pressesPerSecond;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
