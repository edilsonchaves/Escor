using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentoController : MonoBehaviour
{
    public Material ventoTopMaterial;
    public Transform[] ventos2;
    public Animator[] ventos3;
    public float speed = 1;
    float offset = 0;
    // Start is called before the first frame update
    void Start()
    {
        int c=0;
        foreach(Animator anim in ventos3)
        {
            anim.SetFloat("Speed", Random.Range(1.5f, 2f));
            anim.Play("Vento3", -1, (float)(c%4)/4f);
            c++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime*speed;
        ventoTopMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        // MoveVentos2();
    }

    void MoveVentos2()
    {
        foreach(Transform t in ventos2)
        {
            t.position -= Vector3.right*Time.deltaTime*speed;
        }
    }

}
