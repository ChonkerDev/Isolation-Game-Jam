using System;
using UnityEngine;

namespace Chonker.Scripts.Game_Management {
    public abstract class LevelResettable : MonoBehaviour {

        [SerializeField] private bool DebugReset;
        public abstract void Reset();

        protected void OnValidate() {
            if (DebugReset) {
                DebugReset = false;
                Reset();
            }
        }
    }
}