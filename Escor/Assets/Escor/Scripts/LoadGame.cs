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
        {
            _levelSelected = Manager_Game.Instance.sectionGameData.GetCurrentLevel();

        }
        // Debug.Log($"_levelSelected: {_levelSelected}");
        // StartCoroutine(LoadScene(_levelSelected)); // está sempre abrindo o menu
        StartCoroutine(LoadAsyncScene(6)); // acredito que o certo é carregar a cena "GameLevel"
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

        // [Jessé]
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        yield return new WaitUntil(() => !asyncLoad.isDone);

        /*AsyncOperation operationLoad = SceneManager.LoadSceneAsync("GameLevel");
        while (!operationLoad.isDone)
        {
            float progress = Mathf.Clamp01(operationLoad.progress / 0.9f);
            Debug.Log("Load Progress: " + progress);
            yield return null;
        }*/
    }


    // [Jessé]
    IEnumerator LoadAsyncScene(int level)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            imageFillBar.fillAmount     = asyncLoad.progress;
            imageBGFillBar.fillAmount   = asyncLoad.progress;
            textProgress.text           = "Loading("+Mathf.FloorToInt(asyncLoad.progress*100)+"%)";
            yield return null;
        }
    }
}
