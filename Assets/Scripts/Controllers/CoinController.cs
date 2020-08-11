using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class CoinController : MonoBehaviour
    {
        private void Start()
        {
            if (GameManager.Manager.playerProfile.CoinCollectedLevelList.Contains(GameManager.Manager.currentLevel.levelId))
            {
                CloseCoin();
            }
            else
            {
                LevelManager.Manager.onRecordEnd += OpenCoin;
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && LevelManager.Manager.gameState != LevelManager.GameState.OnRecording)
            {
                CloseCoin();
                AudioManager.Manager.PlayOneShot(AudioManager.Manager.collectSound);
            }
        }

        public void OpenCoin()
        {
            LevelManager.Manager.isCoinCollected = false;
            gameObject.SetActive(true);
        }

        public void CloseCoin()
        {
            LevelManager.Manager.isCoinCollected = true;
            gameObject.SetActive(false);
        }
        
    }
}
