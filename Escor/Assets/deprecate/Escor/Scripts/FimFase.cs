using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FimFase : MonoBehaviour
{
    [SerializeField] bool unnofficialEndPhase;
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


            // int currentLevel = sectionDataInstance._currentLevelLocal;
            int currentLevel = levelDataInstance.LevelGaming;


            // LevelData ---------------

                bool[] newPowers = new bool[]{
                    currentLevel >= 1,
                    currentLevel >= 2,
                    currentLevel >= 3,
                };
            string memoryLevel = GameObject.FindObjectOfType<MemoryShardScript>().CaptureMemoryShardInformation();
            levelDataInstance.SetLevelData(currentLevel, 0, 0, 3, newPowers,null,"", memoryLevel); // SetLevelData(int, float ,float , int, bool[], bool[])

            // -------------------------


            // SectionData -------------

            string[] fragmentMemorysSection = sectionDataInstance.GetMemoryFragment();
            Debug.Log(fragmentMemorysSection.Length+", "+currentLevel);
            fragmentMemorysSection[currentLevel-1] = levelDataInstance.FragmentMemoryStatus;
            if (!unnofficialEndPhase) // final n�o oficial
            {
                if (currentLevel == sectionDataInstance.GetCurrentLevel())
                {
                    Debug.Log("Desbloquiei um novo nivel");
                    sectionDataInstance.SetSectionData(currentLevel + 1, 0, levelDataInstance.Powers, fragmentMemorysSection); // SetSectionData(int, int, bool[])
                }
                else
                {
                    Debug.Log("Atualizando informa��es do n�vel: " + currentLevel);
                    sectionDataInstance.SetSectionData(sectionDataInstance.GetCurrentLevel(), 0, levelDataInstance.Powers, fragmentMemorysSection); // SetSectionData(int, int, bool[])
                }
            }
            else
            {
                sectionDataInstance.SetSectionData(currentLevel, 0, levelDataInstance.Powers, fragmentMemorysSection); // SetSectionData(int, int, bool[])
            }


            // -------------------------


            // Save --------------------

            SaveLoadSystem.SaveFile<SectionData>(Manager_Game.Instance.sectionGameData);
            Manager_Game.Instance.bossData?.DeleteBossData();
            Manager_Game.Instance.levelData?.DeleteLevelData();
            // -------------------------


            SceneManager.LoadScene("SelectLevel");
        }
    }
}
