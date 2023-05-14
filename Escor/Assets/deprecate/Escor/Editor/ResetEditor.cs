using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ResetEditor : MonoBehaviour
{
    [MenuItem("Game Controll/Reset/Reset Game Data Save")]
    public static void ResetGameData()
    {
        SaveLoadSystem.ResetGameData();
    }
}
