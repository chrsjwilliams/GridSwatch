using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameScreen;
using UnityEngine.Events;
using DG.Tweening;

public class PressAndHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool canPerfromAction = true;
    [SerializeField] Transform targetTransform;
    [SerializeField] float holdScale;
    [SerializeField] float pauseTime;
    [SerializeField] float scaleTime;


    private float timeHeld;
    public Image filledImage;
    private bool pressed;
    private readonly Vector3 offset = 50 * Vector3.left;
    private Vector3 basePos;

    public UnityEvent OnComplete;

    private delegate void ButtonAction();

    private void Start()
    {
        filledImage.fillAmount = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetTransform.DOScale(holdScale, scaleTime).SetEase(Ease.InExpo);
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
        targetTransform.DOScale(1f, scaleTime).SetEase(Ease.OutExpo);
        targetTransform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (!canPerfromAction) return;

        filledImage.fillAmount = timeHeld / pauseTime;
        if (pressed)
        {
            timeHeld += Time.unscaledDeltaTime;
            if (timeHeld >= pauseTime)
            {
                OnComplete?.Invoke();
                timeHeld = 0;
                ReturnToNeutral();
            }
        }
        else
        {
            timeHeld -= Time.unscaledDeltaTime;
            if (timeHeld <= 0) timeHeld = 0;

        }
    }

}
