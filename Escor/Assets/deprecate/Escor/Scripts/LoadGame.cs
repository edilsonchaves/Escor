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
        Debug.Log("Ola Vim aqui no level: " + Manager_Game.Instance.levelData.LevelGaming);

        if (Manager_Game.Instance.sectionGameData!= null)
        {
            print("1723_1");
            _levelSelected = Manager_Game.Instance.levelData.LevelGaming;
            print("1723_2");
            
        }
        print("1723_3");

        BgColorLevels[_levelSelected-1].transform.parent.gameObject.SetActive(true); // ativa o background correspondente ao nível

        print("1723_4");
        if(Manager_Game.Instance.levelData.LevelGaming!=-1)
            StartCoroutine(LoadAsyncScene("GameLevel"));// acredito que o certo é carregar a cena "GameLevel"
        else
            StartCoroutine(LoadAsyncScene("SelectLevel"));
    }





    // [Jessé]
    IEnumerator LoadAsyncScene(string scene)
    {
        print("1723_5");
        yield return new WaitForSeconds(0.5f);

        print("1723_6");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        print("1723_7");
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
