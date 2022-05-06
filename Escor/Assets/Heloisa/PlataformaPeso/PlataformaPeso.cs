using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaPeso : Ativador
{
    [SerializeField] private string javali = "Javali";
    // [SerializeField] private GameObject[] cols;
    [SerializeField] private int qntsEmCima = 0;


    void Start()
    {
        print("PlataformaPeso");
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer || col.tag == javali)
        {
            // desativarColider();

            ActivateAll();

            qntsEmCima++;
        }


    }


    void OnTriggerExit2D(Collider2D col)
    {
        if(!(col.tag == tagOfPlayer || col.tag == javali))
            return;

        qntsEmCima--;

        if (qntsEmCima <= 0) {
            // ativarColider();

            DisableAll();

            qntsEmCima = 0;
        }
    }


    // void  ativarColider()
    // {
    //     foreach(GameObject colider in cols)
    //     {
    //         colider.SetActive(true);
    //     }
    // }
    //
    //  void  desativarColider()
    // {
    //     foreach(GameObject colider in cols)
    //     {
    //         colider.SetActive(false);
    //     }
    // }






}
