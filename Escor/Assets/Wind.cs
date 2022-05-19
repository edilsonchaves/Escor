using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindDirection { right=1, left=-1 }

public class Wind : MonoBehaviour
{   
    [SerializeField] WindDirection direction;
    public float windSpeed = 0.1f;
    // Start is called before the first frame update

    void OnTriggerStay2D(Collider2D playerCollider) {
        if (playerCollider.gameObject.CompareTag("Player"))
        {
            Transform player = playerCollider.gameObject.transform;
            Vector3 movement = new Vector2(windSpeed * ((int)direction), 0f);
            Movement movementInstance = playerCollider.gameObject.GetComponent<Movement>();
            player.position += movement * Time.fixedDeltaTime * (movementInstance.speed * 1.5f);
        }
    }
}
