using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;

public class CollectableFlower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        collectFlower();
    }

    private void collectFlower() {
        LevelManager.instance.NumCollectedFlowers++;
        gameObject.SetActive(false);
    }
}
