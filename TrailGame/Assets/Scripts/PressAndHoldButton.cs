using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameScreen;


public class PressAndHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    private const float pauseTime = 0.5f;
    private float timeHeld;
    private const float holdScale = 1.2f;
    public Image filledImage;
    private bool pressed;
    private readonly Vector3 offset = 50 * Vector3.left;
    private Vector3 basePos;

    private delegate void ButtonAction();
    ButtonAction handler;

    private void Start()
    {

        if (transform.name.Contains("Home"))
        {
            handler = ToMainMenu;
        }
        else if (transform.name.Contains("Options"))
        {
            handler = ToggleOptionMenu;
        }
        else if (transform.name.Contains("Play"))
        {
            handler = PlayGame;
        }
        
        filledImage.fillAmount = 0;
        basePos = transform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Services.AudioManager.PlaySoundEffect(Services.Clips.UIButtonPressed, 1.0f);
        transform.parent.transform.localScale = new Vector3(holdScale, holdScale, 1);
        pressed = true;
        timeHeld = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ReturnToNeutral();
    }


    private void ReturnToNeutral()
    {
        pressed = false;
        transform.parent.transform.localScale = Vector3.one;
        filledImage.fillAmount = 0;
        transform.localPosition = basePos;
    }

    private void Update()
    {
        if (pressed)
        {
            timeHeld += Time.unscaledDeltaTime;
            filledImage.fillAmount = timeHeld / pauseTime;
            if (timeHeld >= pauseTime)
            {
                handler();
            }
        }
    }

    private void ToMainMenu()
    {
        Services.Scenes.Swap<TitleSceneScript>();
    }

    private void ToggleOptionMenu()
    {
        GameObject.Find("Options_Menu").SetActive(true);
    }

    private void PlayGame()
    {
        Services.Scenes.Swap<GameSceneScript>();
    }

    
}
