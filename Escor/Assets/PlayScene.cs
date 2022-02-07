using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    public bool prologo=true;

    void OnEnable()
    {
        SceneManager.LoadScene(prologo?"SelectLevel":"CreditsScene");
        Destroy(gameObject);
    }
}
