using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BGImageControllLevel : MonoBehaviour
{
    [SerializeField] Image _bg;
    [SerializeField] Sprite[] _spritesBG;
    Manager_Game manager;
    private void Start()
    {
        UpdateBGImage(Manager_Game.Instance.sectionGameData.GetCurrentLevel());
    }

    void UpdateBGImage(int level)
    {
        _bg.sprite = _spritesBG[level-1];
    }
}
