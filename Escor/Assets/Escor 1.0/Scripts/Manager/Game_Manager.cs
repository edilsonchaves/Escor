using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using QFSW.QC;
using System;
using Utils;
public class Game_Manager : Singletons<Game_Manager>
{
    public LevelSelected.LevelEnum levelOpen;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _quantumConsole;
    public static Action _saveAction;
    public static Action _loadAction;
    private void Start()
    {
        if (QuantumConsole.Instance == null)
        {
            Instantiate(_quantumConsole,Vector3.zero,Quaternion.identity);
        }
        StartCoroutine(LoadGameLevel());   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _saveAction.Invoke();
        }
    }
    IEnumerator LoadGameLevel()
    {

       var loadSceneAsync =  SceneManager.LoadSceneAsync(levelOpen.ToString(), LoadSceneMode.Additive);
        while (!loadSceneAsync.isDone) 
        {
    
            Debug.Log($"Carregando cenário {Mathf.Clamp01(loadSceneAsync.progress)}");
            yield return null;
        }
        Debug.Log($"Carregando cenário {loadSceneAsync.isDone}");

        Debug.Log("Fim Carregamento Cenário");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Fim Carregamento Dados Cenário");
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(SetupPlayer());
        Debug.Log("Fim Carregamento Dados Player");
        yield return StartCoroutine(SetupEnemys());
        Debug.Log("Fim Carregamento Dados Inimigos");
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Fim do Load");
    }

    IEnumerator SetupPlayer()
    {
        Debug.Log("Instanciando o player");
        Instantiate(_player,Vector3.zero,Quaternion.identity);
        StartCoroutine(Player.SetupPlayer());
        yield return new WaitUntil(() => Player.IsPlayerLoaded);
        Debug.Log("Setup Finalizado");
        yield return new WaitForSecondsRealtime(1f);

    }
    IEnumerator SetupEnemys()
    {
        Debug.Log("Setup Enemy");
        yield return new WaitForSecondsRealtime(1f);
    }
}
