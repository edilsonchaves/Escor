using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryShardScript : MonoBehaviour
{
    [SerializeField] private GetMemory[] countChild;
    // Start is called before the first frame update
    public void InitializeShard(string shardsStatus)
    {
        ReadLifeLevel();
        foreach (char shardStatus in shardsStatus)
        {
            Debug.Log(shardStatus);
        }
    }

    public string CaptureLifeShardInformation()
    {
        string shardLevelStatus = "";
        ReadLifeLevel();
        foreach (GetMemory shardStatus in countChild)
        {
            if (shardStatus.gameObject.activeSelf)
            {
                shardLevelStatus += "1";
            }
            else
            {
                shardLevelStatus += "0";
            }
        }
        Debug.Log(shardLevelStatus);
        return shardLevelStatus;
    }

    private void ReadLifeLevel()
    {
        if (countChild.Length != 0)
            return;
        countChild = GetComponentsInChildren<GetMemory>();

    }
}
