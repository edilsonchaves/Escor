using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLife : MonoBehaviour
{
    [SerializeField] GameObject _prefab;

    public void PrefabActive(bool value)
    {
        _prefab.SetActive(value);
    }

    public bool IsPrefabActive()
    {
        return _prefab.activeSelf;
    }
}
