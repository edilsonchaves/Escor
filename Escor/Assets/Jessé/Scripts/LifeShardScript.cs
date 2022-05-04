using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeShardScript : MonoBehaviour
{
    [SerializeField]private GetLife[] countChild;
    // Start is called before the first frame update
    public void InitializeLifeShard(string lifesStatus)
    {
        ReadLifeLevel();
        foreach (char lifeStatus in  lifesStatus)
        {
            Debug.Log(lifeStatus);
        }
    }

    public string CaptureLifeShardInformation()
    {
        string shardStatus="";
        ReadLifeLevel();
        foreach (GetLife lifeStatus in countChild)
        {
            if (lifeStatus.gameObject.activeSelf)
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
