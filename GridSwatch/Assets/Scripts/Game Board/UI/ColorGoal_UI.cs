using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace GameScreen
{
    public class ColorGoal_UI : MonoBehaviour
    {
        
        public RectTransform rectTransform;
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI iconText;
        [SerializeField] Color defaultColor;
        [SerializeField] Color finishedColor;
        
        private int _count;
        public int Count { get { return _count; } }
        private int _goal;
        public int Goal { get { return _goal; } }

        public void Init(Color color, int count, int goal)
        {
            _count = count;
            _goal = goal;
            icon.color = color;
            iconText.text = Count + " / " + Goal;
        }

        // ~TODO: Find a way to move this call out of an update function
        public void UpdateText(int count, int goal)
        {
            _count = count;
            _goal = goal;
            iconText.text = Count + " / " + Goal;
        }

        public bool IsGoalMet()
        {
            Color textColor = Count >= Goal ? finishedColor : defaultColor;
            iconText.color = textColor;
            return Count >= Goal;
        }
    }
}