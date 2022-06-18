using UnityEngine;

// Gerencia todos os javalis
public class JavalisManager : MonoBehaviour
{
    public  static JavalisManager instance;   //Singleton
    private        IA_Javali[]    allJavalis;


    // Awake is called before Start
    void Awake()
    {
        instance = this; // this == JavalisManager
        SetJavalis();
    }


    // encontra os javalis e coloca o script 'IA_Javali' em allJavalis
    private void SetJavalis()
    {
        GameObject[] javalis = GameObject.FindGameObjectsWithTag("Javali"); // encontra os javalis pela tag 'Javali'
        allJavalis           = new IA_Javali[javalis.Length];

        for(int c=0; c<javalis.Length; c++)
            allJavalis[c]    = javalis[c].GetComponent<IA_Javali>();
    }


    // muda o movimento do javali entre parado e andando
    public void StopMovementOfJavalis(bool stop)
    {
        foreach(IA_Javali javaliScript in allJavalis)
            javaliScript.Move = !stop;
    }

}
