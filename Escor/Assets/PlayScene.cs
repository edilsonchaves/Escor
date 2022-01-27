using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Level_2", LoadSceneMode.Single);
    }
}
