using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaPeso : MonoBehaviour
{
    [SerializeField] private string player, javali;    
    [SerializeField] private GameObject[] cols; 
    [SerializeField] private int qntsEmCima = 0;




    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == player || col.tag == javali)
        {
            desativarColider();
            qntsEmCima++;
        }

        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == player || col.tag == javali)
        {            
            qntsEmCima--;   

        } 

        if (qntsEmCima <= 0) {            
            ativarColider();        
            qntsEmCima = 0;
        } 
    }

    void  ativarColider() 
    {
        foreach(GameObject colider in cols)
        {
            colider.SetActive(true);
        }
    } 

     void  desativarColider() 
    {
        foreach(GameObject colider in cols)
        {
            colider.SetActive(false);
        }
    } 

    

    

    
}
