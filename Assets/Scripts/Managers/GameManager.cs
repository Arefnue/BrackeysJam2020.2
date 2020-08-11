using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
       
        #region Singleton

        public static GameManager Manager;


        private void Awake()
        {
            if (Manager == null)
            {
                Manager = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        


        #endregion

        public PlayerDataSO playerProfile;
        public List<LevelSO> allLevels;
        public LevelSO currentLevel;

    }
}
