using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    public static GamaManager gm;
    private int life;

    void Awake()
    {
        if(gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

   

    public void SetLife(int life)
    {
        life += life;
    }

    public int GetLife()
    {
        return life;
    }
}
