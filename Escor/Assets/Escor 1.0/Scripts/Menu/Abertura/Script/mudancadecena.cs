using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mudancadecena : MonoBehaviour
{
   public string NomeDaCena = "Menu_Iniciar";
    public float tempoParaCarregar = 6.5f;

    float cronometro = 0;

   void Update () {
        cronometro += Time.deltaTime;
        if(cronometro > tempoParaCarregar){
            SceneManager.LoadScene(NomeDaCena);
        }
   }
}
