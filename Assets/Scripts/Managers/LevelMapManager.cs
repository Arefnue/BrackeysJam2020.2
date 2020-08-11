using System;
using System.Collections.Generic;
using Controllers;
using Data;
using UnityEngine;

namespace Managers
{
    public class LevelMapManager : MonoBehaviour
    {
        #region Singleton

        public static LevelMapManager Manager;


        private void Awake()
        {
            if (Manager == null)
            {
                Manager = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        


        #endregion

        public List<LevelController> allLevels;
        public PlayerDataSO playerProfile;


        private void Start()
        {
            foreach (var levelController in allLevels)
            {
                if (playerProfile.OpenLevelList.Contains(levelController.levelProfile.levelId))
                {
                    levelController.OpenLevel();
                    levelController.SetCoinImage(
                        !playerProfile.CoinCollectedLevelList.Contains(levelController.levelProfile.levelId));
                }
                else
                {
                    levelController.CloseLevel();
                    levelController.SetCoinImage(false);
                }
            }
        }
    }
}
