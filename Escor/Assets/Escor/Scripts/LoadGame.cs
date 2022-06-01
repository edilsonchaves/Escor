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
    [SerializeField] Image[] BgColorLevels;
    [SerializeField] TextMeshProUGUI textProgress;

    bool travou;


    void Awake()
    {
        travou = true;
        StartCoroutine(ShowIfStuck());

        _levelSelected = 1;
        Debug.Log("Ola Vim aqui no level: " + Manager_Game.Instance.sectionGameData.GetCurrentLevel());

        if (Manager_Game.Instance.sectionGameData!= null)
        {
            Debug.Log("Ola Vim aqui no level: " + Manager_Game.Instance.sectionGameData.GetCurrentLevel());

            _levelSelected = Manager_Game.Instance.sectionGameData.GetCurrentLevel();
            
        }

        BgColorLevels[_levelSelected-1].transform.parent.gameObject.SetActive(true); // ativa o background correspondente ao nível

        if(Manager_Game.Instance.levelData.LevelGaming!=-1)
            StartCoroutine(LoadAsyncScene("GameLevel"));// acredito que o certo é carregar a cena "GameLevel"
        else
            StartCoroutine(LoadAsyncScene("SelectLevel"));
    }





    // [Jessé]
    IEnumerator LoadAsyncScene(string scene)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            travou = false;
            imageFillBar.fillAmount                         = asyncLoad.progress;
            BgColorLevels[_levelSelected-1].fillAmount      = asyncLoad.progress;
            textProgress.text                               = "Loading("+Mathf.FloorToInt(asyncLoad.progress*100)+"%)";
            yield return null;
        }

        imageFillBar.fillAmount                         = 1;
        BgColorLevels[_levelSelected-1].fillAmount      = 1;
        textProgress.text                               = "Loading(100%)";
    }



    IEnumerator ShowIfStuck()
    {
        yield return new WaitForSeconds(4f);

        if(travou)
        {
            textProgress.text  = "Travou :(";
            textProgress.color = Color.red;
        }

    }
}
