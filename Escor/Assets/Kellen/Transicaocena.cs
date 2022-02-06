using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transicaocena : MonoBehaviour
{
[SerializeField] private Animator animator;
private int menu_inicial;

private void update ()
{
    if(Input.GetKeyDown(KeyCode.Space))
    {
        IniciaTransicao(1);
    }
}

public void IniciaTransicao(int _cenaIndice)
{
    menu_inicial = _cenaIndice;
    animator.SetTrigger("Inicia");
}
public void TrocaCena()
{
    SceneManager.LoadScene(menu_inicial);
}
}