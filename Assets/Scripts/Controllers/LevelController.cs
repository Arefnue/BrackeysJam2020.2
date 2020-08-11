using System;
using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        public LevelSO levelProfile;
        public Button levelButton;
        public TextMeshProUGUI levelText;
        public Sprite openSprite;
        public Sprite lockedSprite;
        public Image coinImage;


        private void Start()
        {
            SetLevelName();
        }

        public void PlayLevel()
        {
            GameManager.Manager.currentLevel = levelProfile;
            SceneManager.LoadScene($"Level {levelProfile.levelId.ToString()}");
        }
        
        private void SetLevelName()
        {
            levelText.text = levelProfile.levelId.ToString();
        }

        public void SetCoinImage(bool open)
        {
            coinImage.gameObject.SetActive(open);
        }

        public void OpenLevel()
        {
            levelButton.interactable = true;
            levelText.gameObject.SetActive(true);
            levelButton.image.sprite = openSprite;
        }

        public void CloseLevel()
        {
            levelButton.interactable = false;
            levelText.gameObject.SetActive(false);
            levelButton.image.sprite = lockedSprite;
        }
        
    }
}
