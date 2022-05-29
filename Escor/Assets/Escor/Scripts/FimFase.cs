using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FimFase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // LevelData instance ------

            LevelData levelDataInstance     = Manager_Game.Instance.levelData;

            // -------------------------
 
            // SectionData instance ----
            
                SectionData sectionDataInstance = Manager_Game.Instance.sectionGameData;

            // -------------------------


            int currentLevel = levelDataInstance.LevelGaming;


            // LevelData ---------------

                bool[] newPowers = new bool[]{
                    currentLevel >= 1,
                    currentLevel >= 2,
                    currentLevel >= 3,
                };
            string memoryLevel = GameObject.FindObjectOfType<MemoryShardScript>().CaptureMemoryShardInformation();
            Debug.Log(memoryLevel);
                levelDataInstance.SetLevelData(currentLevel, 0, 0, 3, newPowers,null,"", memoryLevel); // SetLevelData(int, float ,float , int, bool[], bool[])

            // -------------------------


            // SectionData -------------

            string[] fragmentMemorysSection = sectionDataInstance.GetMemoryFragment();
            Debug.Log(fragmentMemorysSection.Length+", "+currentLevel);
            fragmentMemorysSection[currentLevel-1] = levelDataInstance.FragmentMemoryStatus;
            if (currentLevel == sectionDataInstance.GetCurrentLevel()) 
            {
                Debug.Log("Desbloquiei um novo nivel");
                sectionDataInstance.SetSectionData(currentLevel + 1, 0, levelDataInstance.Powers, fragmentMemorysSection); // SetSectionData(int, int, bool[])
            }
            else
            {
                Debug.Log("Atualizando informações do nível: "+currentLevel );
                sectionDataInstance.SetSectionData(currentLevel, 0, levelDataInstance.Powers, fragmentMemorysSection); // SetSectionData(int, int, bool[])
            }

            // -------------------------


            // Save --------------------

            SaveLoadSystem.SaveFile<SectionData>(Manager_Game.Instance.sectionGameData);
                SaveLoadSystem.SaveFile<LevelData>(Manager_Game.Instance.levelData);
            
            // -------------------------


            SceneManager.LoadScene("SelectLevel");
        }
    }
}
