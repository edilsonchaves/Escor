using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformation : MonoBehaviour
{
    [SerializeField] private Transform levelSpawn;
    //[SerializeField] private 
    //public Regions[] levelRegion;

    public void initializeLevelInformation(out Transform spawn)
    {
        spawn = levelSpawn;
    }
}
