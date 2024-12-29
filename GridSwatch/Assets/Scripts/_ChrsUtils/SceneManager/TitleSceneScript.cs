using Ads;
using UnityEngine;
using UnityEngine.UI;
using GameScreen;

public class TitleSceneScript : Scene<TransitionData>
{
    public KeyCode startGame = KeyCode.Space;

    [SerializeField]private float SECONDS_TO_WAIT = 0.1f;

    private TaskManager _tm = new TaskManager();




    internal override void OnEnter(TransitionData data)
    {

    }

    internal override void OnExit()
    {

    }

    public void PressedStartGame()
    {
        Services.Scenes.PushScene<MapSelectSceneScript>();
    }

    public void PressedOptions()
    {

    }

    public void PressedWatchAd()
    {
        Debug.Log("Showing Test Ad...");
        AdManager.Instance.ShowAd(AdType.INTERSTITIAL, () => { Debug.Log("Test Ad closed");});
    }

    private void TitleTransition()
    {

    }

    private void Update()
    {
        _tm.Update();
    }
}
