using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsScene : MonoBehaviour
{
    public void BTN_BackButton()
    {
        SceneManager.LoadScene("menu_inicial");
    }
}
