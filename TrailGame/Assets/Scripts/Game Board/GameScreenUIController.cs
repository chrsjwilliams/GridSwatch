using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GameData;

namespace GameScreen
{
    public class GameScreenUIController : MonoBehaviour
    {
        [SerializeField] ColorGoal_UI colorGoalPrefab;
        [SerializeField] Transform colorGoalParent;
        [SerializeField, ReadOnly] List<ColorGoal_UI> colorGoals = new List<ColorGoal_UI>();

        MapData map;

        [SerializeField] private bool uiSet = false;

        public void SetGameUI(MapData mapData)
        {
            map = mapData;
            for (int i = 0; i < mapData.colorGoals.Count; i++)
            {
                var colorGoal = Instantiate(colorGoalPrefab, colorGoalParent);

                float xPos = Screen.width * ((i + 1) / (float)(mapData.colorGoals.Count + 1));
                colorGoal.transform.localPosition = new Vector3(xPos, 0, 0);

                colorGoal.Init(Services.ColorManager.GetColor(mapData.colorGoals[i].colorMode),
                                Services.Board.CurrentFillAmount[(int)mapData.colorGoals[i].colorMode],mapData.colorGoals[i].amount);

                colorGoals.Add(colorGoal);
            }

            uiSet = true;
        }

        public void DestroyGameUI()
        {

        }

        public bool IsGoalMet()
        {
            if (!uiSet) return false;

            for (int i = 0; i < colorGoals.Count; i++)
            {
                if (!colorGoals[i].IsGoalMet())
                    return false;
            }

            return true;
        }

        void Update()
        {
            if (!uiSet) return;

            for (int i = 0; i < colorGoals.Count; i++)
            {
                colorGoals[i].UpdateText(Services.Board.CurrentFillAmount[(int)map.colorGoals[i].colorMode],map.colorGoals[i].amount);
                
            }
        }

    }
}