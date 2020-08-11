using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class DoorController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Open = Animator.StringToHash("Open");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && LevelManager.Manager.isKeyCollected && LevelManager.Manager.gameState != LevelManager.GameState.OnRecording)
            {
                _animator.SetTrigger(Open);
            }
        }

        public void FinishGame()
        {
            LevelManager.Manager.gameState = LevelManager.GameState.OnGameFinished;
        }
    }
}
