using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    private Dictionary<string, string> links = new Dictionary<string, string>
    {
        {"Kellen",              "https://linktr.ee/sophilah"},
        {"Jessé",               "https://bit.ly/399PiEv"},
        {"Edilson",             "https://www.linkedin.com/in/edilson-chavesws/"},
        {"Guilherme Leandro",   "https://www.linkedin.com/in/guilhermedecicco/"},
        {"Wirlianny",           "https://www.linkedin.com/in/helo%C3%ADsa-rodrigues-012a577b/"},
        {"Guilherme Santos",    "https://www.instagram.com/guilherme.s.menezes/"},
        {"Patrícia",            "https://www.instagram.com/elestial.art/"},
        {"Alan",                "https://www.linkedin.com/in/alan-marinho-301b2324"},
        {"Gabriel",             "http://www.instagram.com/gabriel.b.pineiro"},
        {"Bruna",               "https://instagram.com/arts.bruna.queiroz"},
        {"Joismar",             "https://github.com/joismar"},        // <- não foi fornecido um link, então peguei esse do GitHub
        {"Charlotte",           "https://www.instagram.com/charlottebrunna/"},
        {"Fabiana",             "https://www.instagram.com/fabisalbuquerque/"},
        {"Thomás",              "https://www.linkedin.com/in/thom%C3%A1s-gomes-179305175"},
    };


    // quem chama essa função é algum botão
    public void OpenParticipant(string ptc)
    {
        GoURL(links[ptc]);
    }


    public void GoURL(string url)
    {
        Application.OpenURL(url);
    }
}
