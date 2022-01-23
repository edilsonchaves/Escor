using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regionLocalization : MonoBehaviour
{
    [SerializeField]private int _id;
    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Execute action
        }
    }
}
