using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GameData;
using TMPro;

namespace GameScreen
{
    public class GameScreenUIController : MonoBehaviour
    {

        [SerializeField] ColorGoal_UI colorGoalPrefab;
        [SerializeField] Transform colorGoalParent;
        [SerializeField, ReadOnly] List<ColorGoal_UI> colorGoals = new List<ColorGoal_UI>();
        [SerializeField] RectTransform canvas;
        [SerializeField] GameOverBanner gameOverBanner;
        [SerializeField] TextMeshProUGUI mapText;
        [SerializeField] TextMeshProUGUI solvedText;
        
        MapData map;

        [SerializeField] private bool uiSet = false;

        public void HideBanner()
        {
            gameOverBanner.HideBanner();
        }
        
        public void SetGameUI(MapData mapData)
        {
            mapText.text = mapData.mapName;
            bool finished = Convert.ToBoolean(PlayerPrefs.GetInt(mapData.name));

            solvedText.text = finished ? "SOLVED" : "";
            List<ColorGoal_UI> uiToDeleteList = new List<ColorGoal_UI>();

            foreach (var uiIcon in colorGoalParent.GetComponentsInChildren<ColorGoal_UI>())
            {
                uiToDeleteList.Add(uiIcon);
            }

            foreach (var uiToDelete in uiToDeleteList)
            {
                Destroy(uiToDelete.gameObject);
            }
            
            gameOverBanner.HideBanner();

            colorGoals = new List<ColorGoal_UI>();
            
            
            
            map = mapData;
            for (int i = 0; i < mapData.colorGoals.Count; i++)
            {
                var colorGoal = Instantiate(colorGoalPrefab, colorGoalParent);

               
                colorGoal.Init(Services.ColorManager.GetColor(mapData.colorGoals[i].colorMode),
                                Services.Board.CurrentFillAmount[(int)mapData.colorGoals[i].colorMode],mapData.colorGoals[i].amount);
                float xPos = Screen.width * ((i + 1) / (float)(mapData.colorGoals.Count + 1));
                colorGoal.rectTransform.localPosition = new Vector3(xPos, 0, 0);

                colorGoals.Add(colorGoal);
            }

            uiSet = true;
        }

        private void OnDestroy()
        {
  
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

            gameOverBanner.ShowBanner();

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