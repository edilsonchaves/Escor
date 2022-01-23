using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEvents : MonoBehaviour
{
    public static class GameConfig
        
    {
        public static event Action<int> onChangeLanguage;

        public static void ChangedLanguage(int value)
        {
            
            if (onChangeLanguage!=null)
            {
                onChangeLanguage(value);
            }
        }
    }

    public static class UIConfig
    {
        public static event Action<int, int, int> onPauseGame;
        public static event Action onResumeGame;

        public static void PausedGame(int PercentualGame, int CoinGame, int Life)
        {
            if(onPauseGame != null)
            {
                onPauseGame(PercentualGame, CoinGame, Life);
            }
        }

        public static void ResumedGame()
        {
            if(onResumeGame != null)
            {
                onResumeGame();
            }
        }
                
    }        
}
