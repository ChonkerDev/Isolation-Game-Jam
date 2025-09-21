using System.Collections;
using Chonker.Core;
using Chonker.Core.Tween;
using Chonker.Scripts.Game_Management;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class LevelThreeSequencer : MonoBehaviour {
    [SerializeField] private TextMeshPro thanksForPlayerText;

    [SerializeField] private GameObject DeadWife;

    [SerializeField] private GameObject LotsOfFlowers;

    [SerializeField] private Color textAllFlowersCollected;
    [SerializeField] private Color textNotAllFlowersCollected;
    [SerializeField] private PlayableDirector allFlowersCollectedDirector;
    [SerializeField] private PlayableDirector NotallFlowersCollectedDirector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start() {
        //textAllFlowersCollected = new Color(233 / 255f, 127 / 255f, 212 / 255f);
        //textNotAllFlowersCollected = new Color(0 / 255f, 169 / 255f, 255 / 255f);

        //PersistantDataManager.instance.DebugSetCollectedAllFlowers(false);
        if (!PersistantDataManager.instance.GetCollectedAllFlowers()) {
            DeadWife.SetActive(false);
            NotallFlowersCollectedDirector.Play();
        }
        else {
            allFlowersCollectedDirector.Play();
        }

        thanksForPlayerText.gameObject.SetActive(false);
        ScreenFader.instance.FadeOut(Color.white, 0, null, EaseType.EaseOutQuad);
        yield return new WaitForSeconds(1f);
        ScreenFader.instance.FadeIn(Color.white, 3, null, EaseType.EaseOutQuad);
    }

    public void ActivateText() {
        thanksForPlayerText.gameObject.SetActive(true);
        Color targetColor = PersistantDataManager.instance.GetCollectedAllFlowers()
            ? textAllFlowersCollected
            : textNotAllFlowersCollected;
        StartCoroutine(TweenCoroutines.RunTaperRealTime(1,
            f => { thanksForPlayerText.color = Color.Lerp(Color.clear, targetColor, f); },
            () => { thanksForPlayerText.color = targetColor; }, EaseType.EaseInOutQuad));
    }

    public void ActivateLotsOfFlowers() {
        LotsOfFlowers.gameObject.SetActive(PersistantDataManager.instance.GetCollectedAllFlowers());
    }

    public void ExitScene() {
        ScreenFader.instance.FadeOut(Color.white, 3,
            () => SceneManagerWrapper.LoadScene(SceneManagerWrapper.SceneId.MainMenu), EaseType.EaseOutQuad);
    }
}