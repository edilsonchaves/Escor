using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ScaleUpWhenMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleMultiply=1.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter");
        transform.localScale *= scaleMultiply;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        transform.localScale /= scaleMultiply;
    }


}
