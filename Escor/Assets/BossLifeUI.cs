using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossLifeUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image _spriteRenderer;
    [SerializeField] Sprite[] spriteStatusLife;
    [SerializeField] Transform target;
    public void InitializeBossLife()
    {
        _spriteRenderer.gameObject.SetActive(true);
        StartCoroutine(ShowLifeBoss());
    }

    IEnumerator ShowLifeBoss()
    {
        foreach(var sprite in spriteStatusLife)
        {
            _spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void UpdateUI(int valueLife)
    {
        Debug.Log(valueLife);
        _spriteRenderer.sprite = spriteStatusLife[valueLife];
    }

    private void OnEnable()
    {        
        ManagerEvents.Boss.oninitialBattle += InitializeBossLife;
        ManagerEvents.Boss.onUpdateLifeUI += UpdateUI;
    }
    private void OnDisable()
    {
        ManagerEvents.Boss.oninitialBattle -= InitializeBossLife;
        ManagerEvents.Boss.onUpdateLifeUI -= UpdateUI;

    }
}
