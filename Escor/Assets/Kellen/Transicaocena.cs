using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transicaocena : MonoBehaviour
{
[SerializeField] private Animator animator;
private int cenaIndice;

private void update ()
{
    if(Input.GetKeyDown(KeyCode.Space))
    {
        IniciaTransicao(0);
    }
}

public void IniciaTransicao(int _cenaIndice)
{
    cenaIndice = _cenaIndice;
    animator.SetTrigger("Inicia");
}
public void TrocaCena()
{
    SceneManager.LoadScene(cenaIndice);
}
}