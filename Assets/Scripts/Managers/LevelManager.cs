using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton

        public static LevelManager Manager;


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

        public enum GameState
        {
            Normal,
            OnRecordStart,
            OnRecording,
            OnRecordEnd,
            OnGameStart,
            OnRewind,
            OnGameFinished,
            OnRewindFinished,
            OnBusy,
            OnRewindStart

        }
        
        [Header("State")]
        public GameState gameState;
        public Action onRecordStart;
        public Action onRecordEnd;
        public Action onGameStart;
        public Action onGameFinished;
        private bool _isPressed;
    
        [Header("PlayerSpawn")] 
        public GameObject playerPrefab;
        public Transform spawnPoint;
        public float spawnDuration;
        public Ease spawnEase;
        
        [Header("Record")]
        public List<PlayerController> recordList;
        public int maxRecordCount;
        public int totalUsageOfRecord;

        [Header("Level Config")]
        public bool isKeyCollected;
        public bool isCoinCollected;

        private void Start()
        {
            Time.timeScale = 1f;
            gameState = GameState.OnGameStart;
            onGameFinished += AddCoin;
            onGameFinished += AddNextLevel;
            CountRecord();
        }

        private void Update()
        {
            if (gameState == GameState.OnBusy)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetScene();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (recordList.Count > 0)
                {
                    Destroy(recordList[recordList.Count-1].gameObject);
                    recordList.RemoveAt(recordList.Count-1);
                    CountRecord();
                }
                
            }

            if (recordList.Count > maxRecordCount)
            {
                Destroy(recordList[0].gameObject);
                recordList.RemoveAt(0);
                CountRecord();

            }
        
            switch (gameState)
            {
                case GameState.Normal:

                    if (Input.GetKeyDown(KeyCode.Space) && !_isPressed)
                    {
                        _isPressed = true;
                        gameState = GameState.OnRecordStart;
                    }
                
                    break;
                case GameState.OnRecordStart:
                    
                    AudioManager.Manager.PlayOneShot(AudioManager.Manager.recordStart);
                    PostManager.Manager.recordPost.SetActive(true);
                    PostManager.Manager.rewindPost.SetActive(false);
                    onRecordStart?.Invoke();
                    totalUsageOfRecord++;
                    _isPressed = false;
                    gameState = GameState.OnRecording;
                    AudioManager.Manager.PlayMusic(AudioManager.Manager.recording);
                    break;
                case GameState.OnRecording:
                    
                    break;
                case GameState.OnRecordEnd:
                    AudioManager.Manager.PlayMusic(AudioManager.Manager.normalMusic);
                    onRecordEnd?.Invoke();
                    CountRecord();
                    gameState = GameState.OnRewindStart;
                    AudioManager.Manager.PlayOneShot(AudioManager.Manager.recordEnd);
                    break;
                case GameState.OnGameStart:
                    
                    onGameStart?.Invoke();
                    gameState = GameState.Normal;
                    break;
                case GameState.OnRewind:
                    break;
                case GameState.OnGameFinished:
                    
                    onGameFinished?.Invoke();
                    Time.timeScale = 0f;
                    gameState = GameState.OnBusy;
                    break;
                case GameState.OnRewindFinished:
                    PostManager.Manager.recordPost.SetActive(false);
                    PostManager.Manager.rewindPost.SetActive(false);
                    AudioManager.Manager.PlayMusic(AudioManager.Manager.normalMusic);

                    gameState = GameState.Normal;
                    break;
                case GameState.OnBusy:
                    break;
                case GameState.OnRewindStart:
                    AudioManager.Manager.PlayMusic(AudioManager.Manager.rewindClip);
                    PostManager.Manager.recordPost.SetActive(false);
                    PostManager.Manager.rewindPost.SetActive(true);
                    gameState = GameState.OnRewind;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        public void SpawnPlayer(Vector3 playerSpawnPos)
        {
            Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
        }
        
        public void ResetScene()
        {
            var scene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(scene.name);
        }

        private void CountRecord()
        {
            var value = maxRecordCount - recordList.Count;
            UiManager.Manager.ChangeCounter(value);
        }

        private void AddCoin()
        {
            if (isCoinCollected)
            {
                GameManager.Manager.playerProfile.CoinCollectedLevelList.Add(GameManager.Manager.currentLevel.levelId);
                GameManager.Manager.playerProfile.TotalCoin += 1;
            }
        }

        private void AddNextLevel()
        {
            if (!GameManager.Manager.playerProfile.OpenLevelList.Contains(GameManager.Manager.currentLevel.levelId+1))
            {
                GameManager.Manager.playerProfile.OpenLevelList.Add(GameManager.Manager.currentLevel.levelId+1);
            }
            
        }


    }
}
