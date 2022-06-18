using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlatform : MonoBehaviour
{
    [SerializeField] private Sprite     leftSprite, midleSprite, rightSprite;
    [SerializeField] private GameObject blockModel; // modelo de bloco - está nos prefabs de Jessé como block
    [SerializeField] private int        size;
    [SerializeField] private int        orderLayer;


    private int currentSize=0;
    List<GameObject> blocks = new List<GameObject>();


    // void OnDrawGizmos()
    // {
    //     print(midleSprite.bounds.size*transform.localScale.x);
    //     // Debug.DrawLine(transform.position+Vector3.up*0.5, transform.position+Vector3.right*size);
    //     // Debug.DrawLine(transform.position-Vector3.up*0.5, transform.position+Vector3.right*size);
    //     // Debug.DrawLine(transform.position+Vector3.up*0.5, transform.position+Vector3.right*size);
    //
    //
    // }


    void Create()
    {
        // print("oi");
        float _x=0;
        Vector2 firstSpriteSize = Vector2.zero;

        while(currentSize < size)
        {
            GameObject newBlock = Instantiate(blockModel, transform.GetChild(0)) as GameObject;
            // newBlock.transform.SetParent(transform);

            if(currentSize == 0)
            {
                newBlock.GetComponent<SpriteRenderer>().sprite = leftSprite?leftSprite:midleSprite;
                newBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = leftSprite?leftSprite:midleSprite;
                firstSpriteSize = newBlock.GetComponent<SpriteRenderer>().bounds.size;
            }
            else if(currentSize == size-1)
            {
                newBlock.GetComponent<SpriteRenderer>().sprite = rightSprite?rightSprite:midleSprite;
                newBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = rightSprite?rightSprite:midleSprite;
            }
            else
            {
                newBlock.GetComponent<SpriteRenderer>().sprite = midleSprite;
                newBlock.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = midleSprite;
            }

            newBlock.transform.localPosition = new Vector3(_x,0,0);
            newBlock.transform.localScale = transform.localScale;
            _x += newBlock.GetComponent<SpriteRenderer>().bounds.size.x/transform.localScale.x;

            blocks.Add(newBlock);

            currentSize++;
        }

        float SizeOfColliderY = 0.2f;
        transform.GetChild(0).GetComponent<BoxCollider2D>().size    = new Vector2(_x,SizeOfColliderY);
        transform.GetChild(0).GetComponent<BoxCollider2D>().offset  = new Vector2(_x/2-firstSpriteSize.x/2, firstSpriteSize.y/2-SizeOfColliderY/2-0.025f);

    }


    // Start is called before the first frame update
    void Start()
    {
        Create();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     Change();
    // }

    void Change()
    {
        if(currentSize > size)
        {
            DestroyImmediate(transform.GetChild(currentSize != 0 ? 1 : 0).gameObject);
            currentSize--;
        }
    }


    public void FadeInAllBlocks()
    {
        foreach(GameObject gob in blocks)
        {
            gob.GetComponent<Animator>().SetBool("StartAnimation", true);
        }
    }
}
