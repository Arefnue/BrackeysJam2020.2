using Managers;
using UnityEngine;

namespace Controllers
{
    public class KeyController : MonoBehaviour
    {
        private void Start()
        {
            LevelManager.Manager.onRecordEnd += OpenKey;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && LevelManager.Manager.gameState != LevelManager.GameState.OnRecording)
            {
                CloseKey();
                AudioManager.Manager.PlayOneShot(AudioManager.Manager.collectSound);
            }
        }

        public void OpenKey()
        {
            LevelManager.Manager.isKeyCollected = false;
            gameObject.SetActive(true);
        }

        public void CloseKey()
        {
            LevelManager.Manager.isKeyCollected = true;
            gameObject.SetActive(false);
        }
    }
}
