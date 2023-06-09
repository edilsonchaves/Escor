using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Game_Manager : MonoBehaviour
{
    public LevelSelected.LevelEnum levelOpen;
    private void Awake()
    {
        StartCoroutine(LoadGameLevel());   
    }

    IEnumerator LoadGameLevel()
    {
       var loadSceneAsync =  SceneManager.LoadSceneAsync(levelOpen.ToString(), LoadSceneMode.Additive);
        while (!loadSceneAsync.isDone) 
        {
    
            Debug.Log($"Carregando cenário {Mathf.Clamp01(loadSceneAsync.progress)}");
            yield return null;
        }
        Debug.Log("Fim Carregamento Cenário");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Fim Carregamento Dados Cenário");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Fim Carregamento Dados Player");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Fim Carregamento Dados Inimigos");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Fim do Load");
    }
}
