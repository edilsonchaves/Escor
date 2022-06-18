using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlatformManager : MonoBehaviour
{
    [SerializeField]
    public bool isAbove;
    public bool playerIsAbove;
    public bool javaliIsAbove;

    public List<IA_Javali> javalisAbove = new List<IA_Javali>();

    public int numberOfAbove=0;
    // private Movement mvt;
    private Rigidbody2D objAboveRigidbody;
    BoxCollider2D myCol;
    // private MovePlataform movePlatform;

    List<Transform> allAboveMe = new List<Transform>();

    Vector2 myVelocity;

    void Start()
    {
        // TryGetComponent(out MovePlataform movePlatform);

        // if (!TryGetComponent(out BoxCollider2D myCol))
            // this.enabled = false;

        myCol = GetComponent<BoxCollider2D>();
        //
        if(myCol == null)
            this.enabled = false;

        // m_Center = m_Collider.bounds.center;

    }

    // void Update()
    // {
    //     print("_> "+myCol.bounds.center);
    // }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // print("_> collision:  "+collision.contacts[0].normal);
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Javali") && Mathf.Round(collision.contacts[0].normal.y) == -1)
        {
            if(!playerIsAbove && collision.gameObject.tag == "Player")
                playerIsAbove = true;

            if(!javaliIsAbove && collision.gameObject.tag == "Javali")
            {
                // javaliIsAbove = true;
                javalisAbove.Add(collision.gameObject.GetComponent<IA_Javali>());
            }

            allAboveMe.Add(collision.transform);
            isAbove = true;
            collision.transform.SetParent(transform);
            numberOfAbove++;
        }

    }


    void OnCollisionExit2D(Collision2D collision) {

        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Javali"))
        {
            if(collision.gameObject.tag == "Player")
                playerIsAbove = false;

            if(collision.gameObject.tag == "Javali")
            {
                // javaliIsAbove = true;
                javalisAbove.Remove(collision.gameObject.GetComponent<IA_Javali>());
            }
            // if(collision.gameObject.TryGetComponent(out Rigidbody2D objAboveRigidbody))
            // {
                // objAboveRigidbody.velocity = new Vector2(myVelocity.x, objAboveRigidbody.velocity.y); // a velocidade no eixo y não muda
            // }
            if(collision.transform.parent == transform)
                collision.transform.SetParent(null);


            numberOfAbove--;
            isAbove = (numberOfAbove != 0);
        }

    }


    public bool JavaliIsOnCenterOfPlataform()
    {
        if(javalisAbove.Count == 0)
            return false;
        //
        foreach(IA_Javali ia in javalisAbove)
            if(Mathf.Abs(ia.transform.position.x - myCol.bounds.center.x) <= 1f)
                return true;

        return false;
    }


    public void SetVelocity(Vector2 vel)
    {
        myVelocity = vel;
    }


    public void StopJavalis()
    {
        foreach(IA_Javali ia in javalisAbove)
            ia.Move = false;
    }


    public void MoveJavalis()
    {
        foreach(IA_Javali ia in javalisAbove)
            ia.Move = true;
    }


    // void OnCollisionExit2D(Collision2D collision) {

    //     if (collision.gameObject.tag == "Player")
    //     {
    //         if(collision.transform.parent == transform)
    //         {
    //             collision.transform.SetParent(null);
    //         }
    //         mvt = collision.gameObject.GetComponent<Movement>();
    //         mvt.noChao = false;
    //         isAbove = false;
    //     }

    // }

}
