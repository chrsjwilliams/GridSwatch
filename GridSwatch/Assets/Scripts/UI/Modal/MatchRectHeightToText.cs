using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TMP_Text))]
public class MatchRectHeightToText : MonoBehaviour
{

    RectTransform rect;
    TMP_Text text;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        var sizeDelta = rect.sizeDelta;
        sizeDelta.y = text.GetPreferredValues().y;
        rect.sizeDelta = sizeDelta;
    }
}
