using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/PlayerData",fileName = "PlayerData")]
    public class PlayerDataSO : ScriptableObject
    {
        
        [Header("Save")]
        [SerializeField]
        private string key;
        public bool testMode;
        
        [Header("Levels")]
        [SerializeField] private List<LevelSO> allLevels;
        [SerializeField] private List<int> openLevelList;
        [SerializeField] private List<int> coinCollectedLevelList;
        
        [Header("Scores")] 
        [SerializeField] private int totalCoin;
        
        [Header("Skins")]
        [SerializeField] private List<SkinSO> allSkins;
        [SerializeField] private SkinSO currentSkin;
        [SerializeField] private List<SkinSO> openSkinList;
        
        
        #region Properties

        public List<SkinSO> AllSkins
        {
            get => allSkins;
            set => allSkins = value;
        }

        public List<SkinSO> OpenSkinList
        {
            get => openSkinList;
            set => openSkinList = value;
        }

        public SkinSO CurrentSkin
        {
            get => currentSkin;
            set => currentSkin = value;
        }

        public int TotalCoin
        {
            get => totalCoin;
            set => totalCoin = value;
        }

        public List<LevelSO> AllLevels
        {
            get => allLevels;
            set => allLevels = value;
        }

        public List<int> OpenLevelList
        {
            get => openLevelList;
            set => openLevelList = value;
        }

        public List<int> CoinCollectedLevelList
        {
            get => coinCollectedLevelList;
            set => coinCollectedLevelList = value;
        }

        #endregion

        
        
        private void OnEnable()
        {
            if (testMode)
            {
                Debug.Log("Database is on test mode and SO is enabled");
            }
            else
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), this);
            }
            
        }

        private void OnDisable()
        {
            
            if (testMode)
            {
                Debug.Log("Database is on test mode and SO is disabled");
            }
            else
            {
                if (key == "")
                {
                    key = name;
                }
                
                string jsonData = JsonUtility.ToJson(this, true);
                PlayerPrefs.SetString(key, jsonData);
                PlayerPrefs.Save();
                
            }

            
        }
        
        
    }
}
