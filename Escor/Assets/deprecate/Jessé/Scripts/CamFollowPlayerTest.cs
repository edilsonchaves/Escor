using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayerTest : MonoBehaviour
{
    [SerializeField]
    public Transform player;
    public float speed;


    // Update is called once per frame
    void Update()
    {
        Vector3 offset = (transform.position-player.position).normalized*Time.deltaTime*speed;
        offset.z = 0;
        transform.position -= offset;
    }
}
