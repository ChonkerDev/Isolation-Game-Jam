using System.Collections;
using Chonker.Core;
using Chonker.Core.Tween;
using Chonker.Scripts.Game_Management;
using TMPro;
using UnityEngine;

public class LevelThreeSequencer : MonoBehaviour {
    [SerializeField] private TextMeshPro thanksForPlayerText;

    [SerializeField] private GameObject DeadWife;

    [SerializeField] private GameObject LotsOfFlowers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        PersistantDataManager.instance.DebugSetCollectedAllFlowers(false);
        if (!PersistantDataManager.instance.GetCollectedAllFlowers()) {
            DeadWife.SetActive(false);
        }
        
        thanksForPlayerText.gameObject.SetActive(false);
        ScreenFader.instance.FadeOut(Color.white, 0, null, EaseType.EaseOutQuad);
        yield return new WaitForSeconds(1f);
        ScreenFader.instance.FadeIn(Color.white, 3, null, EaseType.EaseOutQuad);
    }

    public void ActivateText() {
        thanksForPlayerText.gameObject.SetActive(true);

        StartCoroutine(TweenCoroutines.RunTaperRealTime(1, f => {
            thanksForPlayerText.color = Color.Lerp(Color.clear, Color.white, f);
        }, () => {
            thanksForPlayerText.color = Color.white;
        }, EaseType.EaseInOutQuad));
    }

    public void ActivateLotsOfFlowers() {
        LotsOfFlowers.gameObject.SetActive(PersistantDataManager.instance.GetCollectedAllFlowers());
    }

    public void ExitScene() {
        ScreenFader.instance.FadeOut(Color.white, 3,() => SceneManagerWrapper.LoadScene(SceneManagerWrapper.SceneId.MainMenu), EaseType.EaseOutQuad);
    }
}
