using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerWrapper : MonoBehaviour
{
    public static SceneManagerWrapper instance; 

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    public static void LoadScene(SceneId sceneId) {
        SceneManager.LoadScene((int)sceneId);
    }

    public static SceneId GetSceneId(Scene scene) {
        return (SceneId) scene.buildIndex;
    }

    public static SceneId CurrentSceneId => (SceneId)SceneManager.GetActiveScene().buildIndex;

    public enum SceneId {
        Bootstrap,
        MainMenu,
        Level1,
        Level2,
        Level3,
    }
}
