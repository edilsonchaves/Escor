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
        return lifeShard.CaptureLifeShardInformation();
    }

    public string LevelMemoryInfo()
    {
        return memoryShard.CaptureLifeShardInformation();
    }
}
