using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Level_1", LoadSceneMode.Single);
    }
}
