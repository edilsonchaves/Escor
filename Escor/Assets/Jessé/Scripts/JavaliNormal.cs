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
            ShowExclamation(false); // mostra a exclamacao quando o player entra na área de visão
            Movement(); // faz a movimentação do javali
            Attack();
        }
        else
        {
            // JavaliAnimator.Play("JavaliParado2"); // causa bug
        }
    }
}
