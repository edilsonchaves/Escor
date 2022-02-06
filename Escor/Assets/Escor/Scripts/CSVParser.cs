using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CSVParser : MonoBehaviour
{
    private static List<string> LanguageList = new List<string>();
    private static Dictionary<string, List<string>> languageDictionary = new Dictionary<string, List<string>>();
    private static TextAsset csvFile;
    public static string[] SplitLine(string line)
    {

        return line.Split(';');
    }
    [RuntimeInitializeOnLoadMethod]
    public static void  LoadText()
    {
        csvFile = Resources.Load<TextAsset>("Localization/Localization");
    }

    public static List<string> GetLineRow(string key)
    {
        string[] lines = csvFile.text.Split("\n"[0]);
        List<string> lineResult = new List<string>();
        foreach(var line in lines)
        {
            var rows = line.Split(';');

            if (rows[0] == key)
            {
                lineResult = new List<string>();
                foreach (var element in rows)
                {
                    if (element != "")
                    {
                        lineResult.Add(element);
                    }
                } 
            }
        }
        lineResult.RemoveAt(lineResult.Count);
        return lineResult;
    }
    public static List<string> GetAvaibleLanguages()
    {
        string[] lines = csvFile.text.Split("\n"[0]);
        if (LanguageList.Count == 0)
        {
            var languages = lines[0].Split(';');
            LanguageList = new List<string>(languages);
            LanguageList.RemoveAt(0);
            LanguageList.RemoveAt(LanguageList.Count-1);
            Debug.Log(LanguageList.Count);
        }
        List<string> languagesInGame = new List<string>();
        languagesInGame = new List<string>(lines[1 + Manager_Game.Instance.saveGameData.LanguageSelect].Split(';'));
        languagesInGame.RemoveAt(0);
        languagesInGame.RemoveAt(languagesInGame.Count - 1);
        foreach (string language in languagesInGame)
        {
            Debug.Log("Languages escolhidas: " + language);
        }
               
        return languagesInGame;
    }
    
    public static string GetTextFromID(string id, int languageIndex)
    {
        if (languageDictionary.Count == 0)
        {
            string[] lines = csvFile.text.Split("\n"[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] row = SplitLine(lines[i]);
                if (row.Length > 1)
                {
                    List<string> words = new List<string>(row);
                    words.RemoveAt(0);
                    Debug.Log(row[0]+": "+words[0]+", "+words[1]+", "+words[2]);
                    languageDictionary.Add(row[0], words);
                }
            }
        }
        Debug.Log("Key ID: "+id);
        var values = languageDictionary[id];
        return values[languageIndex];
    }
}
 