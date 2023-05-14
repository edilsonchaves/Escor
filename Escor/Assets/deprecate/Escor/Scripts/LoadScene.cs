using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    public string SceneName="";
    public bool LoadWhenAwake=false;

    void Awake()
    {
        if(!LoadWhenAwake)
            return;

        Load();
    }


    public void Load()
    {
        SceneManager.LoadScene(SceneName);
    }
}
