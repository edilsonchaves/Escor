using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformation : MonoBehaviour
{
    [SerializeField] private Transform levelSpawn;
    [SerializeField] private LifeShardScript lifeShard;
    [SerializeField] private MemoryShardScript memoryShard;
    //public Regions[] levelRegion;

    public void initializeLevelInformation(out Transform spawn)
    {
        spawn = levelSpawn;
        
    }

    public string LevelLifeInfo()
    {
        Debug.Log("Teste");
        return lifeShard.CaptureLifeShardInformation();
    }

    public string LevelMemoryInfo()
    {
        return memoryShard.CaptureMemoryShardInformation();
    }

    public void LoadLifeShardInformation(string lifeValue)
    {
        lifeShard.InitializeLifeShard(lifeValue);
    }
    public void LoadMemoryShardInformation(string memoryValue)
    {
        memoryShard.InitializeShard(memoryValue);
    }
}
