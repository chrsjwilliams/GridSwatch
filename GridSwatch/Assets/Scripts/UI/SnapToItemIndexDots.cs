using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SnapToItemIndexDots : MonoBehaviour
{
    [SerializeField] private SnapToItem _snapToItem;
    [SerializeField] private Button indexDotsPrefab;

    private int prevIndex = -1;
    private List<Button> indexDots = new List<Button>();
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _snapToItem.ItemsAdded += OnItemsAdded;
        _snapToItem.SelectedItemIndex += OnSelectedIndexChanged;
    }

    private void OnItemsAdded(SnapToItem snapToItem, int totalItems)
    {
        if (snapToItem != _snapToItem) return;

        for (int i = 0; i < totalItems; i++)
        {
            Button indexDot = Instantiate(indexDotsPrefab, transform);
            indexDot.onClick.AddListener(() =>
            {
                snapToItem.GoToIndex(indexDots.IndexOf(indexDot));
            });
            indexDots.Add(indexDot);
        }
    }

    private void OnSelectedIndexChanged(int newIndex)
    {
        // TODO: When there are more than 5 dots, are we only able to go 2 above and 2 belox the current index?
        if (prevIndex == newIndex) return;
        if (prevIndex != -1)
        {
            indexDots[prevIndex].transform.DOScale(Vector3.one, 0.33f).SetEase(Ease.OutCirc);
        }

        indexDots[newIndex].transform.DOScale(Vector3.one * 1.6f, 0.33f).SetEase(Ease.OutCirc);

        prevIndex = newIndex;
    }
}
