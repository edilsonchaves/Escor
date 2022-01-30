using UnityEngine;
using UnityEngine.UI;


public  class conferirMenu : MonoBehaviour
{
   
    [SerializeField] 
    private GameObject[] poder = new GameObject[3];
    [SerializeField] private GameObject[] vidas = new GameObject[3];    
    
   


   
    private void Update()
    {
              
        /*if (Input.GetKeyDown(KeyCode.D))
        {
            ResetPoder();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdatePoder(0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            UpdatePoder(1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UpdatePoder(2);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            ResetVidas();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UpdateVidas(0);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            UpdateVidas(1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            UpdateVidas(2);
        }*/

       
    }

    public void UpdatePoder(int poderColor)
    {
        poder[poderColor].SetActive(true);


    }

    public void ResetPoder()
    {
        for (int i =0; i < poder.Length; i ++)
        {
            poder[i].SetActive(false);
        }
    }

    /*public void UpdateVidas(int vidasPegar)
    {
        vidas[vidasPegar].SetActive(true);
    }

    public void ResetVidas()
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            vidas[i].SetActive(false);
        }
    }*/
}

