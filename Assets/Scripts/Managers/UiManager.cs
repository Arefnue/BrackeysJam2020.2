using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UiManager : MonoBehaviour
    {
        #region Singleton

        public static UiManager Manager;


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

        public TextMeshProUGUI recordCount;
        public Image playerRecordImage;
        public Image rewindFill;
        public GameObject gameFinishedPanel;
        public TextMeshProUGUI totalRecordText;

        private void Start()
        {
            LevelManager.Manager.onGameFinished += OpenGameFinishedPanel;
        }

        public void ChangeCounter(int count)
        {
            recordCount.text = "x " + count.ToString();
        }

        public void DisplayTotalRecord()
        {
            totalRecordText.text = "Total Record: " + LevelManager.Manager.totalUsageOfRecord.ToString();
        }

        public void OpenGameFinishedPanel()
        {
            DisplayTotalRecord();
            gameFinishedPanel.SetActive(true);
        }

    }
}
