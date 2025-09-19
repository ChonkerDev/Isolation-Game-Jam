using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Chonker.Scripts.Common {
    public class TriggerAction : MonoBehaviour {
        [SerializeField] private UnityEvent Event;

        private void OnTriggerEnter2D(Collider2D other) {
            Event.Invoke();
        }
    }
}