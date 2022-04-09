using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadGame : MonoBehaviour
{
    int _levelSelected;
    float currentTime = 0;
    [SerializeField]Image imageFillBar;
    [SerializeField] Image imageBGFillBar;
    [SerializeField] TextMeshProUGUI textProgress;
    // Start is called before the first frame update
    void Start()
    {
        if(Manager_Game.Instance.sectionGameData!= null)
            _levelSelected = Manager_Game.Instance.sectionGameData.GetCurrentLevel();
        Debug.Log("Level Selected:"+_levelSelected);
        StartCoroutine(LoadScene(_levelSelected));
    }

    IEnumerator LoadScene(int level)
    {
        float progress = 0;
        while (currentTime < 2)
        {
            Debug.Log(currentTime);
            currentTime += Time.deltaTime;
            progress = (currentTime / 2);
            Debug.Log("Progress: " + progress);
            imageFillBar.fillAmount=progress;
            imageBGFillBar.fillAmount = progress;
            textProgress.text = "Loading("+Mathf.FloorToInt(progress*100)+"%)";
            yield return null;
        }

        /*AsyncOperation operationLoad = SceneManager.LoadSceneAsync("GameLevel");
        while (!operationLoad.isDone)
        {
            float progress = Mathf.Clamp01(operationLoad.progress / 0.9f);
            Debug.Log("Load Progress: " + progress);
            yield return null;
        }*/
    }
}
