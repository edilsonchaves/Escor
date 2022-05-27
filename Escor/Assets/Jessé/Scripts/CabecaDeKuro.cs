using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabecaDeKuro : MonoBehaviour
{
    public Movement movement;
    public Transform topoDaCabeca;

    bool podeFolhaNaCabeca = true;

    void Start()
    {
        gameObject.name = "TopoDaCabeca";
        movement.enabled = true;
        podeFolhaNaCabeca = true;
    }


    public void DerrubarTudo()
    {
        int childCount = topoDaCabeca.childCount;

        for (int c=0; c<childCount; c++)
        {
            // topoDaCabeca.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            topoDaCabeca.GetChild(0).SetParent(null);    
        }
    }


    public void DerrubarFolha()
    {
        int childCount = topoDaCabeca.childCount;

        for (int c=0; c<childCount; c++)
        {
            Transform folhaParent   = topoDaCabeca.GetChild(0);
            folhaParent.GetComponent<Folha>().DropFromKurosHead();
            // folhaParent.rotation    = Quaternion.Euler(0, 0, 0);
            // Transform folha         = folhaParent.GetChild(0).GetChild(0);
            // folhaParent.position    = folha.position;
            // folha.position          = Vector3.zero;    
            // folhaParent.SetParent(null);    
        }

    }


    public void SetPodeFolhaNaCabeca()
    {
        podeFolhaNaCabeca = true;
        movement.animator.SetBool("FolhaNaCabeca", false);
    }


    public void SetNaoPodeFolhaNaCabeca()
    {
        podeFolhaNaCabeca = false;
    }


    public bool GetPodeFolhaNaCabeca()
    {
        return podeFolhaNaCabeca;
    }


    // public static void SetTheParentOfAllChildren(Transform parent = null)
    // {
    //     int childCount = transform.childCount;

    //     for (int c=0; c<childCount; c++)
    //         transform.GetChild(0).SetParent(parent);    
    // }
}
