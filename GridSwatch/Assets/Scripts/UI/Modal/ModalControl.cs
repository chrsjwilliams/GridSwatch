
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public struct ModalContent
{
    public string title;
    [TextArea]
    public string message;
    public ModalControl.InfoType type;
    public Sprite sprite;
}

public class ModalControl : MonoBehaviour
{

    public static ModalControl Instance { get; private set; }

    public RectTransform rootRT;
    public CanvasGroup modalCanvasGroup;
    public TextMeshProUGUI title, textContent;
    public MeshRenderer modelContent;
    public GameObject banner;
    public TextMeshProUGUI bannerTitle, bannerContent;
    public Image imageContent;
    public RectTransform buttonContainer;
    public GameObject buttonAffirm, buttonDecline, buttonWatchAd;
    public Image panel;
    public Vector2 showPos = new Vector2(0f, -115f);
    public Vector2 hidePos = new Vector2(0f, 200f);
    public float inDur = .3f, outDur = .2f, buttonTime = .2f, punchTime = .1f, punchScale = 0.2f;
    public float delayAfterPress = 0f;
    public float notificationDelay = 2f;
    [SerializeField] SimpleEvent BlockInteractionEvent;
    [SerializeField] SimpleEvent AllowInteractionEvent;
    [SerializeField] BoolVariable ScreenIsCovered;
    [SerializeField] HorizontalLayoutGroup standardModalButtonLayoutGroup;

    public static event Action OnModalQueueEmptied, OnModalAddedToQueue;

    private Queue<ModalRequest> requestQueue = new Queue<ModalRequest>();

    ModalRequest currentRequest { get { return requestQueue.Peek(); } }

    enum ModalType
    {
        Notification = 0
        , Ok = 1
        , Confirmation = 2
        , Banner = 3
        , Ad = 4
    }

    public enum InfoType
    {
        Text = 0,
        Image = 1,
        Mesh = 2,
        Banner = 3,
    }

    struct ModalRequest
    {
        public string title;
        public string content;
        public Sprite imageContent;
        public ModalType modalType;
        public InfoType infoType;
        public Action<bool> callback;
        public Action adCallback;

        public ModalRequest(string title, string content, ModalType modalType, InfoType infoType)
        {
            this.title = title;
            this.content = content;
            this.imageContent = null;
            this.modalType = modalType;
            this.infoType = infoType;
            this.callback = null;
            this.adCallback = null;
        }

        public ModalRequest(string title, string content, ModalType modalType, InfoType infoType, Action<bool> callback)
        {
            this.title = title;
            this.content = content;
            this.imageContent = null;
            this.modalType = modalType;
            this.infoType = infoType;
            this.callback = callback;
            this.adCallback = null;
        }

        public ModalRequest(string wordPackName, int numberOFCards, Action<bool> callback)
        {
            this.title = wordPackName;
            this.content = numberOFCards + " WORDS";
            this.imageContent = null;
            this.modalType = ModalType.Banner;
            this.callback = callback;
            this.adCallback = null;
            infoType = InfoType.Text;
        }

        public ModalRequest(string title, Sprite sprite, string description, ModalType modalType, Action<bool> callback)
        {
            this.title = title;
            this.content = description;
            this.imageContent = sprite;
            this.modalType = modalType;
            this.infoType = InfoType.Image;
            this.callback = callback;
            this.adCallback = null;
        }

        public ModalRequest(string title, string content, ModalType modalType, InfoType infoType, Action<bool> callback, Action watchedAdCallback)
        {
            this.title = title;
            this.content = content;
            this.imageContent = null;
            this.modalType = modalType;
            this.infoType = infoType;
            this.callback = callback;
            this.adCallback = watchedAdCallback;
        }

        public ModalRequest(ModalContent content, ModalType modalType, Action<bool> callback)
        {
            this.title = content.title;
            this.content = content.message;
            this.imageContent = content.sprite;
            this.infoType = content.type;
            this.modalType = modalType;
            this.callback = callback;
            this.adCallback = null;
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        GameSceneManager<Scene<TransitionData>>.SceneChanged += OnSceneLoaded;
    }


    // When we change scenes, we kill all tweens that were connected to rootRT
    // Since we buffer all display requests until the screen is uncovered, the
    // only tweens still connected to rootRT are paused by DoTween awaiting garbage collection.
    // These tweens are paused indefinitely since we set AutoKill on var tween to false in
    // the DisplayNextRequest function
    private void OnSceneLoaded()
    {
        rootRT.DOKill();
    }

    void OnDisable()
    {
        GameSceneManager<Scene<TransitionData>>.SceneChanged -= OnSceneLoaded;
    }

    private void Awake()
    {
        Instance = this;
        hideButton(buttonAffirm, buttonDecline, buttonWatchAd);
        rootRT.anchoredPosition = hidePos;
    }

    /// <summary>
    /// Displays a message that will clear automatically rather than the player needing to dismiss it
    /// </summary>
    public void DisplayNotification(string title, string content, InfoType infoType)
    {
        BufferRequest(new ModalRequest(title, content, ModalType.Notification, infoType));
    }

    /// <summary>
    /// Displays a message that will clear automatically rather than the player needing to dismiss it. (with image)
    /// </summary>
    public void DisplayNotification(string title, Sprite sprite, string description, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(title, sprite, description, ModalType.Notification, callback));
    }

    public void DisplayMessage(string content)
    {
        DisplayMessage("", content, InfoType.Text);
    }

    /// <summary>
    /// Displays a message that will clear when the player presses "OK"
    /// </summary>
    public void DisplayMessage(string title, string content, InfoType infoType)
    {
        BufferRequest(new ModalRequest(title, content, ModalType.Ok, infoType));
    }

    /// <summary>
    /// Displays a message that will clear when the player presses "OK"
    /// </summary>
    public void DisplayMessage(string title, string content, InfoType infoType, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(title, content, ModalType.Ok, infoType, callback));
    }

    /// <summary>
    /// Displays a message that will clear when the player presses "OK". (with image)
    /// </summary>
    public void DisplayMessage(string title, Sprite sprite, string description, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(title, sprite, description, ModalType.Ok, callback));
    }

    public void DisplayMessage(ModalContent content)
    {
        BufferRequest(new ModalRequest(content, ModalType.Ok, null));
    }

    public void DisplayMessage(ModalContent content, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(content, ModalType.Ok, callback));
    }

    public void DisplayQuestion(ModalContent content, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(content, ModalType.Confirmation, callback));
    }

    /// <summary>
    /// Displays a message that will clear after the player answers a yes/no questions
    /// </summary>
    public void DisplayQuestion(string title, string content, InfoType infoType, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(title, content, ModalType.Confirmation, infoType, callback));
    }

    /// <summary>
    /// Displays a message that will clear after the player answers a yes/no questions. (with image)
    /// </summary>
    public void DisplayQuestion(string title, Sprite sprite, string description, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(title, sprite, description, ModalType.Confirmation, callback));
    }

    /// <summary>
    /// Displays a banner message that will clear automatically rather than the player needing to dismiss it
    /// </summary>
    public void DisplayBanner(string title, string content)
    {
        BufferRequest(new ModalRequest(title, content, ModalType.Banner, InfoType.Banner));
    }

    /// <summary>
    /// Displays a Word Pack modal. This modal will dismiss itself.
    /// </summary>
    public void DisplayWordPackModal(string wordPackName, int numberOfCards, Action<bool> callback)
    {
        BufferRequest(new ModalRequest(wordPackName, numberOfCards, callback));
    }

    public void DisplayFeeModal(string title, string content, Action<bool, bool> callback)
    {
        bool requestedAd = false;
        Action watchedAd = () => { requestedAd = true; };
        Action<bool> modalCallback = paid =>
        {
            callback(paid, requestedAd);
        };
        BufferRequest(new ModalRequest(title, content, ModalType.Ad, InfoType.Text, modalCallback, watchedAd));
    }

    public void DisplayFeeModal(ModalContent content, Action<bool, bool> callback)
    {
        bool requestedAd = false;
        Action watchedAd = () => { requestedAd = true; };
        Action<bool> modalCallback = paid =>
        {
            callback(paid, requestedAd);
        };
        BufferRequest(new ModalRequest(content.title, content.message,ModalType.Ad, content.type, modalCallback, watchedAd));
    }
    
    void BufferRequest(ModalRequest request)
    {
        StartCoroutine(QueueRequest(request));
    }

    bool startedWaiting = false;

    IEnumerator QueueRequest(ModalRequest request)
    {
        while (ScreenIsCovered.value)
        {
            startedWaiting = true;

            yield return null;
        }

        if (requestQueue.Contains(request))
        {
            Debug.Log("Request already in queue. Clearing queue before continuing.");
            requestQueue.Clear();
        }

        OnModalAddedToQueue?.Invoke();
        requestQueue.Enqueue(request);
        if (requestQueue.Count == 1 || startedWaiting)
        {

            startedWaiting = false;
            DisplayNextRequest();
        }
        yield return null;


    }

    public void ClearQueue()
    {
        requestQueue.Clear();
    }


    void DisplayNextRequest()
    {
        if (requestQueue.Count == 0) return;

        var request = requestQueue.Peek();
        if (request.modalType != ModalType.Banner)
        {
            this.title.text = request.title;
            this.textContent.text = request.content;
            modalCanvasGroup.alpha = 1;
            modalCanvasGroup.blocksRaycasts = true;
            modalCanvasGroup.interactable = true;
            panel.color = new Color(0, 0, 0, 0.91f);
            panel.raycastTarget = true;

            bannerTitle.text = "";
            bannerContent.text = "";

            //big popup sound
            Services.AudioManager.PlayClip(Clips.MODAL);
        }
        else
        {
            modalCanvasGroup.alpha = 0;
            modalCanvasGroup.blocksRaycasts = false;
            modalCanvasGroup.interactable = false;
            bannerTitle.text = request.title;
            bannerContent.text = request.content;

            panel.color = Color.clear;
            panel.raycastTarget = false;

            this.title.text = "";
            this.textContent.text = "";

        }

        SetupTween(rootRT.DOAnchorPos(showPos, inDur), Ease.OutBack);

        switch (request.infoType)
        {
            case InfoType.Text:
                modelContent.gameObject.SetActive(false);
                imageContent.gameObject.SetActive(false);
                banner.SetActive(false);

                textContent.gameObject.SetActive(true);
                textContent.text = request.content;


                break;
            case InfoType.Image:
                modelContent.gameObject.SetActive(false);
                banner.SetActive(false);

                textContent.gameObject.SetActive(true);
                imageContent.gameObject.SetActive(true);
                imageContent.sprite = request.imageContent;
                textContent.text = request.content;

                break;
            case InfoType.Mesh:
                imageContent.gameObject.SetActive(false);
                banner.SetActive(false);

                textContent.gameObject.SetActive(true);
                modelContent.gameObject.SetActive(true);
                textContent.text = request.content;

                break;
            case InfoType.Banner:
                modelContent.gameObject.SetActive(false);
                imageContent.gameObject.SetActive(false);
                textContent.gameObject.SetActive(false);


                banner.SetActive(true);
                break;
        }


        if (request.modalType == ModalType.Notification || request.modalType == ModalType.Banner)
        {
            AllowInteractionEvent.Raise();
            var tween = rootRT.DOAnchorPos(hidePos, outDur);
            // auto kill is set to false so tweens always go to compelteion.
            // any paused tweens will be cleared on scene transition in the
            // CompleteLoadingGroup function in Scene Control
            tween.SetAutoKill(false);
            tween.SetDelay(inDur + notificationDelay);
            tween.onComplete += CompleteCurrentRequest;
            tween.onKill += CompleteCurrentRequest;
            SetupTween(tween, Ease.InBack);


        }
        else
        {
            PauseControl.Pause();
            // If we are blocking interaction with a modal, add an action to the
            // android back button action list. On OK modals, the back button
            // executes the Accept function. On Confirmation and Ad modals, the
            // Back button executes the decline function
            PushAndroidBackButtonAction(request);
            BlockInteractionEvent.Raise();

        }

        buttonWatchAd.transform.localScale = buttonDecline.transform.localScale = buttonAffirm.transform.localScale = buttonAffirm.transform.localScale = Vector3.zero;

        if (request.modalType == ModalType.Confirmation || request.modalType == ModalType.Ad)
        {
            buttonDecline.SetActive(true);
            buttonAffirm.SetActive(true);
            buttonWatchAd.SetActive(request.modalType == ModalType.Ad);

            buttonAffirm.GetComponent<Image>().raycastTarget = true;
            buttonDecline.GetComponent<Image>().raycastTarget = true;
            buttonWatchAd.GetComponent<Image>().raycastTarget = request.modalType == ModalType.Ad;
            if (request.modalType == ModalType.Ad)
            {
                standardModalButtonLayoutGroup.spacing = 10;

                buttonAffirm.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 50);
                buttonDecline.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 50);
                buttonAffirm.GetComponentInChildren<TextMeshProUGUI>().text = "PAY";
                buttonDecline.GetComponentInChildren<TextMeshProUGUI>().text = "CANCEL";
            }
            else
            {
                standardModalButtonLayoutGroup.spacing = 50;
                buttonAffirm.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
                buttonDecline.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
                buttonAffirm.GetComponentInChildren<TextMeshProUGUI>().text = "YES";
                buttonDecline.GetComponentInChildren<TextMeshProUGUI>().text = "NO";
            }

            RefreshLayoutGroup();

            SetupTween(buttonAffirm.transform.DOScale(Vector3.one, buttonTime), Ease.OutBack, inDur);
            SetupTween(buttonDecline.transform.DOScale(Vector3.one, buttonTime), Ease.OutBack, inDur);
            if (request.modalType == ModalType.Ad)
            {
                SetupTween(buttonWatchAd.transform.DOScale(Vector3.one, buttonTime), Ease.OutBack, inDur);
            }

        }
        else if (request.modalType == ModalType.Ok)
        {
            buttonDecline.SetActive(false);
            buttonWatchAd.SetActive(false);

            buttonAffirm.SetActive(true);
            buttonDecline.GetComponent<Image>().raycastTarget = false;
            buttonWatchAd.GetComponent<Image>().raycastTarget = false;
            buttonAffirm.GetComponent<Image>().raycastTarget = true;
            buttonAffirm.GetComponentInChildren<TextMeshProUGUI>().text = "OKAY!";
            SetupTween(buttonAffirm.transform.DOScale(Vector3.one, buttonTime), Ease.OutBack, inDur);
        }

    }

    void CompleteCurrentRequest()
    {
        if (requestQueue.Count < 1) return;

        var currentRequest = requestQueue.Dequeue();

        hideButton(buttonAffirm, buttonDecline, buttonWatchAd);
        SetupTween(rootRT.DOAnchorPos(hidePos, outDur), Ease.InBack);
        // only raise the shade if the queue is empty to ensure the player 
        // cannot press buttons while the modal display updates
        if (requestQueue.Count == 0)
        {
            // Pops android back button action
            PopAndroidBackButtonAction(currentRequest);
            if (currentRequest.modalType != ModalType.Notification && currentRequest.modalType != ModalType.Banner)
            {
                AllowInteractionEvent.Raise();
                PauseControl.UnPause();
            }
            OnModalQueueEmptied?.Invoke();
        }
        else
        {
            if ((currentRequest.modalType == ModalType.Notification &&
                 currentRequest.modalType == ModalType.Banner) ||
                 (currentRequest.modalType != ModalType.Notification &&
                 currentRequest.modalType != ModalType.Banner))
            {
                AllowInteractionEvent.Raise();
                PauseControl.UnPause();
            }
            StartCoroutine(WaitForClear(outDur));
        }
    }

    IEnumerator WaitForClear(float duration)
    {
        while (ScreenIsCovered.value)
        {
            yield break;
        }
        yield return new WaitForSecondsRealtime(duration);
        DisplayNextRequest();
    }

    void hideButton(params GameObject[] buttons)
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Image>().raycastTarget = false;
            SetupTween(button.transform.DOScale(Vector3.zero, buttonTime), Ease.InQuad);
        }
    }

    /// <summary>
    /// handle some boilerplate that needs to go on all tweens in this class.
    /// </summary>
    void SetupTween(Tween tween, Ease ease, float delay = 0f)
    {
        if (tween == null)
        {
            return;
        }

        tween.SetEase(ease);
        if (delay > 0f)
        {
            tween.SetDelay(delay);
        }

        // Making sure that tweens update independent from Time.deltaTime so that
        // modal display/hiding isn't haulted by pausing/unpausing the game
        tween.SetUpdate(UpdateType.Normal, true);
    }

    #region UI Events

    public void Accept()
    {
        Services.AudioManager.PlayClip(Clips.CONFIRM);
        currentRequest.callback?.Invoke(true);
        CompleteCurrentRequest();
    }

    public void Decline()
    {
        Services.AudioManager.PlayClip(Clips.DECLINE);
        currentRequest.callback?.Invoke(false);
        CompleteCurrentRequest();
    }

    public void WatchAd()
    {
        Services.AudioManager.PlayClip(Clips.CONFIRM);
        // access the callback since CompleteCurrentRequest() will clear it
        currentRequest.adCallback();
        Accept();
        CompleteCurrentRequest();
        //var callback = currentRequest.adCallback();
        //AdMobController.Instance.RequestRewardedAd(currentRequest.fee, watchedAd => { callback.Invoke(watchedAd); });
        //CompleteCurrentRequest();
    }

    #endregion // UI Events

    public void RefreshLayoutGroup()
    {
        // This is used to update the layout group
        Canvas.ForceUpdateCanvases();
        standardModalButtonLayoutGroup.enabled = false;
        standardModalButtonLayoutGroup.enabled = true;
    }

    void PushAndroidBackButtonAction(ModalRequest request)
    {
        if (request.modalType == ModalType.Confirmation ||
            request.modalType == ModalType.Ad)
        {
            AndroidBackButtonHandler.Instance.PushAction(Decline);
        }
        else if (request.modalType == ModalType.Ok)
        {
            AndroidBackButtonHandler.Instance.PushAction(Accept);
        }
    }

    void PopAndroidBackButtonAction(ModalRequest request)
    {
        if (request.modalType == ModalType.Confirmation ||
            request.modalType == ModalType.Ad)
        {
            AndroidBackButtonHandler.Instance.PopAction(Decline);
        }
        else if (request.modalType == ModalType.Ok)
        {
            AndroidBackButtonHandler.Instance.PopAction(Accept);
        }
    }
}
