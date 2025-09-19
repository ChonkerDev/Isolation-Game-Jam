using System;
using UnityEngine;

namespace Chonker.Scripts.Game_Management {
    public class GameManager : MonoBehaviour {
        private static GameManager instance;

        private void Awake() {
            if (!instance) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else if (instance != this) {
                Destroy(gameObject);
            }
        }
    }
}