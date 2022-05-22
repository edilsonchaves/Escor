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
        Debug.Log("Ola Vim aqui no level: " + Manager_Game.Instance.sectionGameData.GetCurrentLevel());

        if (Manager_Game.Instance.sectionGameData!= null)
        {
            Debug.Log("Ola Vim aqui no level: " + Manager_Game.Instance.sectionGameData.GetCurrentLevel());

            _levelSelected = Manager_Game.Instance.sectionGameData.GetCurrentLevel();
            
        }
        StartCoroutine(LoadAsyncScene(6)); // acredito que o certo é carregar a cena "GameLevel"
    }





    // [Jessé]
    IEnumerator LoadAsyncScene(int level)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        Debug.Log("Ola Vim aqui no level: " + level);
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
