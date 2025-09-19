using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;

namespace Chonker.Scripts.Level_Parts.End_of_Level {
    public class EndOfLevelCheck : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            LevelManager.instance.TransitionToNextLevel();
            gameObject.SetActive(false);
        }
    }
}