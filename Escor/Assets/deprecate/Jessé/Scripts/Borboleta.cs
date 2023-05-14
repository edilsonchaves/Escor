using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CorDaBorboleta{vermelha=0, azul=1}

public class Borboleta : MonoBehaviour
{
    public Animator parentAnim;
    public Animator childAnim;

    public CorDaBorboleta cor;

    public int numberOfAnim = 2;    

    // Start is called before the first frame update
    void Start()
    {
        // print("81172   "+((int)cor));
        childAnim.Play($"BaterAsas{(int)cor}", -1, Random.Range(0.0f, 1.0f));
        ChangeToRandomAnimation();
    }


    int GetRandomAnimationIndex()
    {
        return Random.Range(0, numberOfAnim)+1; // +1 pq começa do 1 
    }


    public void ChangeToRandomAnimation()
    {
        parentAnim.SetFloat("Speed", Random.Range(0.5f, 1f));
        ChangeAnimation(GetRandomAnimationIndex());
    }


    public void ChangeAnimation(int idx=1)
    {
        parentAnim.Play($"BorboletaSeMovendo{idx}", -1, 0);
    }    

}
