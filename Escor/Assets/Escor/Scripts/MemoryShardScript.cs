using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryShardScript : MonoBehaviour
{
    [SerializeField] private GetMemory[] countChild;
    [SerializeField] private string memoryCountTest = "";
    // Start is called before the first frame update
    public void InitializeShard(string shardsStatus)
    {
        ReadMemoryLevel();
        for (int i = 0; i < shardsStatus.Length; i++)
        {
            countChild[i].PrefabActive(shardsStatus[i] == '1');
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            memoryCountTest = CaptureMemoryShardInformation();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            InitializeShard(memoryCountTest);
        }
    }
    public string CaptureMemoryShardInformation()
    {
        string shardLevelStatus = "";
        ReadMemoryLevel();
        foreach (GetMemory shardStatus in countChild)
        {
            if (shardStatus.IsPrefabActive())
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

    private void ReadMemoryLevel()
    {
        if (countChild.Length != 0)
            return;
        countChild = GetComponentsInChildren<GetMemory>();

    }
}
