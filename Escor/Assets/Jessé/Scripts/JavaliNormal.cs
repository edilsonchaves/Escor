using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaliNormal : IA_Javali
{

    void Update()
    {
        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game)
        {
            isGrounded = CheckIsGrounded();
            Movement(); // faz a movimentação do javali
            Attack();
        }
    }
}
