using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameScreen
{
    public class ColorGoal_UI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI iconText;
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

        public void UpdateText(int count, int goal)
        {
            _count = count;
            _goal = goal;
            iconText.text = Count + " / " + Goal;
        }

        public bool IsGoalMet()
        {
            return Count >= Goal;
        }
    }
}