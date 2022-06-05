using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindDirection { right=1, left=-1 }

public class Wind : MonoBehaviour
{   
    [SerializeField] WindDirection direction;
    public float windSpeed = 0.1f;
    public SpriteRenderer mySpt;


    bool playingSound;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("PlaySoundEffect");
    }


    void OnTriggerStay2D(Collider2D playerCollider) {
        if (playerCollider.gameObject.CompareTag("Player"))
        {
            Transform player = playerCollider.gameObject.transform;
            Vector3 movement = new Vector2(windSpeed * ((int)direction), 0f);
            Movement movementInstance = playerCollider.gameObject.GetComponent<Movement>();
            player.position += movement * Time.fixedDeltaTime * (movementInstance.speed * 1.5f);
        }
    }


    IEnumerator PlaySoundEffect()
    {
        while(true)
        {
            

            if(Camera.main.IsObjectVisible(mySpt))
            {
                SfxManager.PlaySound(SfxManager.Sound.vento); // som de efeito
                yield return new WaitForSeconds(11.5f);
            }


            yield return null;
        }
    }
}
