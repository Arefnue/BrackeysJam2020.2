using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        #region Singleton

        public static TimeManager Manager;
        

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


        
        public float maxRewindValue = 100f;
        public float decreaseRate = 1f;
        public float increaseRate = 1f;

        private Image _fillImage;
        private float _decreaseFill;
        private float _increaseFill;
        
        private void Start()
        {
            _fillImage = UiManager.Manager.rewindFill;
            _fillImage.fillAmount = 1f;

            _decreaseFill = (decreaseRate) / maxRewindValue;
            _increaseFill = (increaseRate) / maxRewindValue;

        }
        
        
        private void FixedUpdate()
        {
            if (LevelManager.Manager.gameState == LevelManager.GameState.OnRecording)
            {
                _fillImage.fillAmount = Mathf.MoveTowards(_fillImage.fillAmount, 0, _decreaseFill);
                
            }
            else
            {
                _fillImage.fillAmount = Mathf.MoveTowards(_fillImage.fillAmount, 1f, _increaseFill);
            }
        }
    }
}
