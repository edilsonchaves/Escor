using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conferirMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] poder = new GameObject[3];

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
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

    
}

