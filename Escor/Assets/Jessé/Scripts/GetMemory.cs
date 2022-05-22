using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMemory : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Collider2D collider;

    public void PrefabActive(bool value) 
    {
        _prefab.SetActive(value);
        collider.enabled = value;
    }

    public bool IsPrefabActive()
    {
        return _prefab.activeSelf;
    }
}
