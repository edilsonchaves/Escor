using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEvents : MonoBehaviour
{
    public static class GameConfig

    {
        public static event Action<int> onChangeLanguage;
        public static event Action<int> onChangeLanguageSize;

        public static void ChangedLanguage(int value)
        {

            if (onChangeLanguage != null)
            {
                onChangeLanguage(value);
            }
        }

        public static void ChangedLanguageSize(int value)
        {

            if (onChangeLanguageSize != null)
            {
                onChangeLanguageSize(value);
            }
        }
    }

    public static class UIConfig
    {
        public static event Action<int, int, int> onPauseGame;
        public static event Action onResumeGame;
        public static event Action onReturnMenu;
        public static event Action onExitMenu;
        public static event Action onSaveGame;
        public static void PausedGame(int PercentualGame, int CoinGame, int Life)
        {
            if (onPauseGame != null)
            {
                onPauseGame(PercentualGame, CoinGame, Life);
            }
        }

        public static void ResumedGame()
        {
            if (onResumeGame != null)
            {
                onResumeGame();
            }
        }

        public static void ReturnedMenu()
        {
            if (onReturnMenu != null)
            {
                onReturnMenu();
            }
        }

        public static void ExitedMenu()
        {
            if (onExitMenu != null)
            {
                onExitMenu();
            }
        }
        public static void SavedGame()
        {
            if (onSaveGame != null)
            {
                onSaveGame();
            }
        }
    }

    public static class PlayerDeadUI
    {
        public static event Action onReplayLevel;

        public static void ReplayedLevel()
        {
            if (onReplayLevel != null)
            {
                onReplayLevel();
            }
        }
    }

    public static class PlayerMovementsEvents
    {
        public static event Action<float> onLookDirection;
        public static event Action<int> onLifePlayer;
        public static event Action onDiePlayer;
        public static event Action<int> onPlayerGetPower;
        public static event Action<float, float> onPlayerDefenseTime;
        public static void LookedDirection(float value)
        {
            
            if (onLookDirection!=null)
            {
                onLookDirection(value);
            }
        }
        public static void LifedPlayer(int value)
        {
            if (onLifePlayer != null)
            {
                onLifePlayer(value);
            }
        }

        public static void DiedPlayer()
        {
            if (onDiePlayer != null)
            {
                onDiePlayer();
            }
        }

        public static void PlayerGetedPower(int value)
        {
            if (onPlayerGetPower != null)
            {
                onPlayerGetPower(value);
            }
        }

        public static void PlayerDefensedPower(float valueAmount,float valueMax)
        {
            if (onPlayerDefenseTime != null)
            {
                onPlayerDefenseTime(valueAmount,valueMax);
            }
        }
    }

}
