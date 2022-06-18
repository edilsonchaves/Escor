using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeShardScript : MonoBehaviour
{
    [SerializeField]private GetLife[] countChild;
    [SerializeField] private string lifeCountTest="";
    // Start is called before the first frame update
    public void InitializeLifeShard(string lifesStatus)
    {
        ReadLifeLevel();
        for(int i = 0; i < lifesStatus.Length; i++)
        {
            countChild[i].PrefabActive(lifesStatus[i] =='1');
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            lifeCountTest=CaptureLifeShardInformation();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            InitializeLifeShard(lifeCountTest);
        }
    }
    public string CaptureLifeShardInformation()
    {
        string shardStatus="";
        ReadLifeLevel();
        foreach (GetLife lifeStatus in countChild)
        {
            if (lifeStatus.IsPrefabActive())
            {
                shardStatus += "1";
            }
            else
            {
                shardStatus += "0";
            }
        }
        Debug.Log(shardStatus);
        return shardStatus;
    }

    private void ReadLifeLevel()
    {
        if (countChild.Length != 0)
            return;
        countChild = GetComponentsInChildren<GetLife>();

    }
}
