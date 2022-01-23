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

    public static class PlayerMovementsEvents
    {
        public static event Action<float> onLookDirection;

        public static void LookedDirection(float value)
        {
            
            if (onLookDirection!=null)
            {
                onLookDirection(value);
            }
        }
    }
}
