using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ResetScene()
        {
            var scene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(scene.name);
        }

        public void NextLevel()
        {
            var next = GameManager.Manager.currentLevel.levelId + 1;
            if (next >= GameManager.Manager.playerProfile.AllLevels.Count)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                foreach (var levelSO in GameManager.Manager.allLevels)
                {
                    if (levelSO.levelId == next)
                    {
                        GameManager.Manager.currentLevel = levelSO;
                        break;
                    }
                }
                SceneManager.LoadScene($"Level {next.ToString()}");
            }
            
            
        }
        

        public void ExitGame()
        {
            Application.Quit();
        }
        
    }
}
