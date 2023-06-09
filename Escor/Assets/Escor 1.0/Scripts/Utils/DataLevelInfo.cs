using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DataLevelInfo : Data<DataLevelInfo>
{
    
    [SerializeField]public List<Player> Teste3;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InitiateInstance(this);
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadData();
        }
    }
    [Serializable]
    public class Player
    {
        public string nomeInimigo;
        public int vidaInimigo;
        [SerializeField]private Vector3 position;
    }



}
