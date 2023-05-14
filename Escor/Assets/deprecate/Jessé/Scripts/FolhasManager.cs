using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolhasManager : MonoBehaviour
{
    public int maximumAmount = 20;
    public GameObject folhaPrefab;
    int level;
    public float delayBetweenSpawn=4; // 4 segundos

    public float timeToStartFadeout = 4, durationOfFadeout = 10;

    public Transform limitLeft, limitRight, spawnY;

    private List<Folha> folhasAtivas = new List<Folha>();
    private List<Folha> folhasInativas = new List<Folha>();

    Movement playerMvt;

    // Start is called before the first frame update
    void Start()
    {   
        try
        {
            level = Manager_Game.Instance.levelData.LevelGaming;
        }
        catch
        {
            level = 1;
        }

        if(level <= 2) // não existe folha no nível 3
            StartCoroutine("KeepSpawning");
    }

    // é chamado depois que tudo já foi desenhado na tela
    void OnPostRender()
    {

    }


    IEnumerator KeepSpawning()
    {
        yield return new WaitForSeconds(1); // esperar a camera se posicionar 
     
        playerMvt = GameObject.FindWithTag("Player").GetComponent<Movement>();

        //  melhor usar o if fora do loop para não ficar verificando várias vezes sem necessidade
        Vector3 randomPosition;
        if(playerMvt)
        {
            while(true)
            {
                yield return new WaitUntil(() => !playerMvt.insideCave); // só deve spawnar folhas quando o player estiver fora da caverna
                yield return new WaitUntil(() => folhasAtivas.Count < maximumAmount); // só deve spawnar folhas quando o player estiver fora da caverna
                
                do 
                {
                    randomPosition = GetRandomPositionInsideLimit();
                    yield return null;
                } while(!PositionInsideSpawnArea(randomPosition)); // fica procurando uma posição disponível
        
                SpawnFolha(randomPosition);
                yield return new WaitForSeconds(delayBetweenSpawn);
            }
        }

        while(true)
        {
            yield return new WaitUntil(() => folhasAtivas.Count < maximumAmount); // só deve spawnar folhas quando o player estiver fora da caverna
            do 
            {
                randomPosition = GetRandomPositionInsideLimit();
                yield return null;
            } while(!PositionInsideSpawnArea(randomPosition)); // fica procurando uma posição disponível
    
            SpawnFolha(randomPosition);
            yield return new WaitForSeconds(delayBetweenSpawn);
        }
    }


    // verifica se a posição está dentro de um colisor do tipo ground
    bool PositionIsInsideGround(Vector2 position)
    {      
        Collider2D[] colisoes = Physics2D.OverlapCircleAll(position, 0f);
        // foreach(Collider2D colisao in colisoes); // quando eu tento usar colisao dentro do loop diz que não existe
        for(int c=0; c<colisoes.Length; c++)
        {
            if(colisoes[c].gameObject.tag == "ground")
                return true;
            // print("93271   "+colisoes[c].gameObject.tag);
        }    

        return false;
    }


    // verifica se a posição está dentro do colisor onde pode nascer folhas
    bool PositionInsideSpawnArea(Vector2 position)
    {      
        Collider2D[] colisoes = Physics2D.OverlapCircleAll(position, 0f);
        for(int c=0; c<colisoes.Length; c++)
        {
            if(colisoes[c].gameObject.tag == "PodeNascerFolha")
                return true; // está dentro
        }    

        return false; // está fora
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


    void SpawnFolha(Vector3 position)
    {
        Folha folha                 = GetNewFolha();
        folha.transform.position    = position;
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
