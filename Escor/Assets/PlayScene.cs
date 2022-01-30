using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("SelectLevel");
        Destroy(gameObject);
    }
}
