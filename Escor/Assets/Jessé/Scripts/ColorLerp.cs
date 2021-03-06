using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    public float lerpTime, transparence;

    SpriteRenderer spt;
    public Color[] myColor;

    int colorIdx = 0;

    float t=.0f;

    // Start is called before the first frame update
    void Start()
    {
        // for(int c=0; c<myColor.Length; c++)
        // {
        //     myColor[c][3] = transparence;
        // }

        colorIdx = Random.Range(0, myColor.Length);

        spt = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spt.color = Color.Lerp(spt.color, myColor[colorIdx], lerpTime*Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime*Time.deltaTime);

        if(t>.9f)
        {
            t = 0;
            colorIdx++;
            colorIdx = (colorIdx >=  myColor.Length)?0:colorIdx;
        }
        // print(transform.lossyScale.x);
        float minScale = 1f, maxScale = 6;
        float newTransp = (transform.lossyScale.x * 3 - minScale) / (maxScale - minScale);

        spt.color = new Color(spt.color[0], spt.color[1], spt.color[2], newTransp);
    }


}
