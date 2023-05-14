using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionAsMousePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SetPosition");
    }

    // void Update()
    // {
    //
    // }

    IEnumerator SetPosition()
    {
        while(true)
        {
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = Camera.main.transform.position.z + 100;
            transform.position = pos;
            yield return null;
        }
    }
}
