using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverCanvas : MonoBehaviour
{
    [SerializeField]GameObject bg;
    [SerializeField] VerticalLayoutGroup _verticalLayout;

    private void OnEnable()
    {
        ManagerEvents.PlayerMovementsEvents.onDiePlayer += ActiveDieBG;
    }
    private void OnDisable()
    {
        ManagerEvents.PlayerMovementsEvents.onDiePlayer -= ActiveDieBG;
    }

    public void ActiveDieBG()
    {
        bg.SetActive(true);
        if (Manager_Game.Instance.levelData == null)
            _verticalLayout.transform.GetChild(1).gameObject.SetActive(false);
        int childNumber=0;
        foreach (var child in _verticalLayout.transform.GetComponentsInChildren<LayoutElement>())
        {
            Debug.Log(child.gameObject.activeSelf);
            if (child.gameObject.activeSelf)
                childNumber++;
        }
        int paddingReturnTop = (childNumber * 120 + 80) / 2;
        _verticalLayout.padding.top = paddingReturnTop * -1;
    }
    public void BTN_ReturnMenu()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void BTN_LoadLevel()
    {
        Manager_Game.Instance.LoadLevelGame();
        SceneManager.LoadScene("GameLevel");
    }

    public void BTN_NewLevel()
    {
        ManagerEvents.PlayerDeadUI.ReplayedLevel();
    }
}
