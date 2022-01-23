using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum LevelStatus {Game, Pause, CutScene};
    public static LevelStatus levelstatus;
    private LevelStatus auxLevelStatus;
   
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (levelstatus == LevelStatus.Pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        ManagerEvents.UIConfig.ResumedGame();
        levelstatus = auxLevelStatus;

    }

    void Pause()
    {
        ManagerEvents.UIConfig.PausedGame(10, 20, 30);
        auxLevelStatus = levelstatus;
        levelstatus = LevelStatus.Pause;
    }


}
