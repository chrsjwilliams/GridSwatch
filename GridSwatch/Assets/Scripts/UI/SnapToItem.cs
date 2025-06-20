using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using  UnityEngine.UI;
using UnityEngine.UIElements;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(ScrollView))]
public class SnapToItem : MonoBehaviour
{
    public Action<SnapToItem, int> ItemsAdded;
    public Action<int> SelectedItemIndex;
    
    public enum ScrollMode {HORIZONTAL, VERTICAL}

    public ScrollMode scrollMode;
    [ReadOnly, SerializeField] ScrollRect scrollRect;
    [ReadOnly, SerializeField] RectTransform listItemPrefab;
    
    // Assign Content Child GameObject fro the ScrollView Parent class you added
    [SerializeField] RectTransform contentPanel;
    public RectTransform ContentPanel => contentPanel;

    // Add a LayoutGroup and ContentSizeFitter to the Content GameObject of your ScrollView.
    // Assign that LayoutGroup to this property in the Inspector
    [SerializeField] HorizontalOrVerticalLayoutGroup layoutGroup;

    private int totalItems;
    private bool _allItemsAdded = false;
    private bool isSnapped;
    private bool interrupt;
    
    [SerializeField] private float snapVelocityThreshold;
    [SerializeField] private float snapForce;
    private float snapVelocity;
    

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        isSnapped = false;
        _allItemsAdded = true;

    }

    public void Init(RectTransform itemPrefab, int items)
    {
        listItemPrefab = itemPrefab;
        totalItems = items;
        ItemsAdded?.Invoke(this, totalItems);
    }

    public void GoToIndex(int index)
    {
        if (index < 0) return;
        if (index > totalItems) return;

        SelectedItemIndex?.Invoke(index);

        interrupt = true;
        isSnapped = false;
        snapVelocity = 0;
        
        float itemDimension;
        // Set current position
        switch (scrollMode)
        {
            case ScrollMode.HORIZONTAL:
                
                itemDimension = listItemPrefab.rect.width;
                float offset = itemDimension *0.6f;

                float xValue = (0 - (index * (itemDimension + layoutGroup.spacing + offset)));
                contentPanel.transform.DOMoveX(xValue, 0.66f)
                    .SetEase(Ease.OutCirc).OnComplete(() =>
                    {
                        interrupt = false;
                        

                    });
                break;
            default:
                itemDimension = listItemPrefab.rect.height;
                offset = index == totalItems - 1 ? itemDimension *0.6f :0;
                float yValue = (0 - (index * (itemDimension + layoutGroup.spacing + offset)));

                contentPanel.transform.DOMoveY(yValue, 0.66f)
                    .SetEase(Ease.OutCirc)
                    .OnComplete(() =>
                        {
                            interrupt = false;
                            
                        }
                    );

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_allItemsAdded) return;
        if (interrupt) return;
        
        float currentPos;
        float itemDimension;
        switch (scrollMode)
        {
            case ScrollMode.HORIZONTAL:
                currentPos = contentPanel.localPosition.x;
                itemDimension = listItemPrefab.rect.width;
                break;
            default:
                currentPos = contentPanel.localPosition.y;
                itemDimension = listItemPrefab.rect.height;

                break;
        }

        int currentItem = Mathf.RoundToInt(0 - currentPos / (itemDimension + layoutGroup.spacing));
        SelectedItemIndex?.Invoke(currentItem);

        if (scrollRect.velocity.magnitude < snapVelocityThreshold && !isSnapped)
        {
            scrollRect.velocity = Vector2.zero;
            snapVelocity += snapForce * Time.deltaTime;
            Vector3 newPosition = scrollMode == ScrollMode.HORIZONTAL
                ? new Vector3(
                    Mathf.MoveTowards(currentPos, 0 - (currentItem * (itemDimension + layoutGroup.spacing)),
                        snapVelocity),
                    contentPanel.localPosition.y,
                    contentPanel.localPosition.z)
                : new Vector3(
                    contentPanel.localPosition.x,
                    Mathf.MoveTowards(currentPos, 0 - (currentItem * (itemDimension + layoutGroup.spacing)),
                        snapVelocity),
                    contentPanel.localPosition.z);

            contentPanel.localPosition = newPosition;

            isSnapped = currentPos == 0 - (currentItem * (itemDimension + layoutGroup.spacing));

        }
        
        if(scrollRect.velocity.magnitude > snapVelocityThreshold)
        {
            isSnapped = false;
            snapVelocity = 0;
        }
    }
}
