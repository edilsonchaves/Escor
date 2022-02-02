using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlatformManager : MonoBehaviour
{
    [HideInInspector]
    public bool isAbove;


    void OnCollisionEnter2D(Collision2D collision) {
        
            // print(Mathf.Round(collision.contacts[0].normal.y) +"       "+ Mathf.Round(-Vector2.up.y));
        // print(collision.gameObject.tag+"    "+(-Vector2.up) +"  "+collision.contacts[0].normal +"   "+(collision.contacts[0].normal.Equals(new Vector2(0.0f, -0.1f))));
        if (collision.gameObject.tag == "Player" && Mathf.Round(collision.contacts[0].normal.y) == Mathf.Round(-Vector2.up.y)) 
        {
            // print("Above");
            // StopAllCoroutines();
            // StartCoroutine(ChangeParent(collision.transform));
            collision.gameObject.GetComponent<Movement>().noChao = true;
            collision.transform.SetParent(transform);
            isAbove = true;
        }

    }


    void OnCollisionExit2D(Collision2D collision) {
    
        if (collision.gameObject.tag == "Player") 
        {
            if(collision.transform.parent == transform)
            {
                collision.transform.SetParent(null);
            }
            isAbove = false;
            collision.gameObject.GetComponent<Movement>().noChao = false;

        }

    }


    IEnumerator ChangeParent(Transform child, float wait=.05f)
    {
        yield return new WaitForSeconds(wait);
        child.SetParent(transform);
    }

}
