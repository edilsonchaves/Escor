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
            

            int currentLevel = sectionDataInstance.GetCurrentLevel();


            // LevelData ---------------

                bool[] newPowers = new bool[]{
                    currentLevel >= 1,
                    currentLevel >= 2,
                    currentLevel >= 3,
                };

                levelDataInstance.SetLevelData(currentLevel, 0, 0, 3, newPowers, null); // SetLevelData(int, float ,float , int, bool[], bool[])

            // -------------------------


            // SectionData -------------

                sectionDataInstance.SetSectionData(currentLevel+1, 0, levelDataInstance.Powers); // SetSectionData(int, int, bool[])

            // -------------------------


            // Save --------------------
                
                SaveLoadSystem.SaveFile<SectionData>(Manager_Game.Instance.sectionGameData);
                SaveLoadSystem.SaveFile<LevelData>(Manager_Game.Instance.levelData);
            
            // -------------------------


            SceneManager.LoadScene("SelectLevel");
        }
    }
}
