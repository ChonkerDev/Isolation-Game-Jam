using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;


public class UIMainMenu : NavigationUIMenu {
    public AnimationCurve MainMenuPopInScaleCurve;
    void Start() {
        Deactivate();
        OnCurrentSelectionChanged += OnCurrentSelectionChangedEvent;
    }

    private void OnCurrentSelectionChangedEvent(GameObject prev, GameObject current) {
        Debug.Log($"{prev}, {current}");
    }
}
