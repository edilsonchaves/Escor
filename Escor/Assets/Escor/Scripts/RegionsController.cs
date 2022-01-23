using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionsController : MonoBehaviour
{
    [SerializeField]private List<Regions> regions;

    private void Start()
    {
        InitializeRegionsLevel();
    }
    public void InitializeRegionsLevel()
    {
        Debug.Log("Teste");
        regionLocalization[] regionsInLevel = GetComponentsInChildren<regionLocalization>();
        Debug.Log(regionsInLevel.Length);
        regions = new List<Regions>();
        foreach(var regionInLevel in regionsInLevel)
        {
            Debug.Log(regionInLevel.gameObject.name);
            regionInLevel.ID = regions.Count;
            regions.Add(new Regions(regionInLevel.gameObject,false));
            
        }
    }
}
[System.Serializable]
public class Regions {
    [SerializeField]GameObject region;
    [SerializeField]bool isVisit;
    public Regions(GameObject newRegion,bool newIsVisit)
    {
        Debug.Log("OLA");
        region = newRegion;
        isVisit = newIsVisit;
    }
}