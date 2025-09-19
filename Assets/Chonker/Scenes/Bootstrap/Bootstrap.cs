using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start() {
        yield return null;
        SceneManagerWrapper.LoadScene(SceneManagerWrapper.SceneId.MainMenu);
    }
    
}
