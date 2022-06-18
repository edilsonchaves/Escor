using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMouseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // print("_> "+LevelManager.levelstatus);
#if !UNITY_EDITOR
        SetCursorVisible(LevelManager.levelstatus != LevelManager.LevelStatus.Game);
#endif
    }

    public void SetCursorVisible(bool vis) 
    {

        Cursor.lockState = vis ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible   = vis;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;   
    }
}
