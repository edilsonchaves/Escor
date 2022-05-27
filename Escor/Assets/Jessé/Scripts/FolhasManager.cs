using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolhasManager : MonoBehaviour
{
    public GameObject folhaPrefab;
    int level;
    public float delayBetweenSpawn=4; // 4 segundos

    public float timeToStartFadeout = 4, durationOfFadeout = 10;

    public Transform limitLeft, limitRight, spawnY;

    private List<Folha> folhasAtivas = new List<Folha>();
    private List<Folha> folhasInativas = new List<Folha>();

    // Start is called before the first frame update
    void Start()
    {
        level = Manager_Game.Instance.levelData.LevelGaming;
        if(level <= 2) // não existe folha no nível 3
            StartCoroutine("KeepSpawning");
    }


    IEnumerator KeepSpawning()
    {
        yield return new WaitForSeconds(1); // esperar a camera se posicionar 

        while(true)
        {
            SpawnFolha();
            yield return new WaitForSeconds(delayBetweenSpawn);
        }
    }


    Vector3 GetRandomPositionInsideLimit()
    {
        // a posição dentro do limite é apenas no eixo x

        float x = Random.Range(limitLeft.position.x, limitRight.position.x);
        return new Vector3(x, spawnY.position.y, transform.position.z);
    }


    Folha GetNewFolha()
    {
        Folha folha;

        if(folhasInativas.Count == 0) // não existe folha desativada
        {
            // cria uma nova folha
            GameObject newFolha         = Instantiate(folhaPrefab, transform) as GameObject;
            folha                       = newFolha.GetComponent<Folha>();
            folha.timeToStartFadeout    = timeToStartFadeout;
            folha.durationOfFadeout     = durationOfFadeout;
            folha.controller            = this;
        }
        else // existe folha para reciclar
        {
            // reaproveita uma folha desativada
            folha = folhasInativas[0];
            folha.gameObject.SetActive(true);
            folhasInativas.RemoveAt(0);
        }


        folhasAtivas.Add(folha);

        return folha;
    }


    void SpawnFolha()
    {
        Vector3 randomPosition      = GetRandomPositionInsideLimit();
        Folha folha                 = GetNewFolha();
        folha.transform.position    = randomPosition;
        // folha.TurnOnMe();
        folha.startWithRandomOffset = false;
        // folha.myRb.simulated        = true;

        folha.SetSpriteColor(level-1); // level 1 -> color 0 | level 2 -> color 1
    }


    public void TurnOffFolha(Folha folha)
    {
        folhasAtivas.Remove(folha);
        folha.gameObject.SetActive(false);
        folhasInativas.Add(folha);
    }

}
