using System;
using System.Collections;
using Data;
using DG.Tweening;
using Managers;
using Unity.Mathematics;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(TimeController))]
    public class PlayerController : MonoBehaviour
    {
        // Config
        [Header("References")]
        public Rigidbody2D myRigidBody;

        [Header("Jump")]
        [SerializeField] private int maxJumpCombo = 2;
        [SerializeField] private float jumpSpeed = 10;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 10;
    
        [Header("Ground")] 
        public LayerMask groundLayer;
        public Transform groundCheckPosition;
        public float groundCheckRadius;

        [Header("Crushed")] 
        public LayerMask crushedLayer;
        public float crushedRadius;
        
        [Header("Record")]
        public LayerMask recordLayer;
        public float recordRadius;

        [Header("FX")] 
        public Transform runFxTransform;
        public GameObject runFx;
        
        
        // States
        private int _curJumpCombo = 0;
        private float _horizontal = 0f;
        private float _jumpDirection;
        private bool _isGrounded;
        private bool _isFacingRight;

        private TimeController _timeController;
        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;
        private BoxCollider2D _boxCollider;
        private SkinSO _playerSkin;

        private bool _isOnRecord;
        private bool _isCrushed;
        private bool _playerIsOnRecordObject;
        private bool _canStopRecord;
        private bool _playRoutineOnce;
        private bool _isTriggered;
        private bool _isDeath;
        private bool _startRun;
        private bool _runLeft;
        private bool _runRight;
        private bool _doorOpened;

        private void Start()
        {
            
            transform.parent = null;
            SetComponents();
            _playerSkin = GameManager.Manager.playerProfile.CurrentSkin;
            _renderer.sprite = _playerSkin.normalSprite;
            MovePlayerToStartLocation();
        }

        #region Setup

        private void SetComponents()
        {
            _renderer = GetComponent<SpriteRenderer>();
            LevelManager.Manager.onRecordStart += OnRecordHappened;
            _timeController = GetComponent<TimeController>();
            _rb = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void MovePlayerToStartLocation()
        {
            _boxCollider.enabled = false;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            
            transform.DOMove(LevelManager.Manager.spawnPoint.position, LevelManager.Manager.spawnDuration)
                .SetEase(LevelManager.Manager.spawnEase).OnComplete(
                    () =>
                    {
                        LevelManager.Manager.gameState = LevelManager.GameState.OnRewindFinished;
                        _boxCollider.enabled = true;
                        _rb.isKinematic = false;
                    });
        }

        #endregion
        
        #region Updates

        private void Update()
        {

            if (_doorOpened)
            {
                return;
            }
            
            switch (LevelManager.Manager.gameState)
            {
                case LevelManager.GameState.OnRewind:
                case LevelManager.GameState.OnGameFinished:
                    return;
                case LevelManager.GameState.OnRecording:
                {
                    if (!_playRoutineOnce)
                    {
                        _playRoutineOnce = true;
                        StartCoroutine(CanStopRecordRoutine());
                    }

                    if (Input.GetKeyDown(KeyCode.Space) && _canStopRecord)
                    {
                        StopAllCoroutines();
                        ChangeToRecord();
                    }

                    break;
                }
                case LevelManager.GameState.Normal:
                    break;
                case LevelManager.GameState.OnRecordStart:
                    break;
                case LevelManager.GameState.OnRecordEnd:
                    break;
                case LevelManager.GameState.OnGameStart:
                    break;
                case LevelManager.GameState.OnRewindFinished:
                    break;
                case LevelManager.GameState.OnBusy:
                    break;
                case LevelManager.GameState.OnRewindStart:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            


            if (_isDeath)
            {
                return;
            }

            if (_isGrounded)
            {
                _curJumpCombo = 0;
                
                if (Input.GetKeyDown(KeyCode.A))
                {
                    _runLeft = true;
                    
                
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _runRight = true;
                    
                }
            }
            
            Jump();
        }

        
        
        private void FixedUpdate()
        {
            if (LevelManager.Manager.gameState == LevelManager.GameState.OnRewind || LevelManager.Manager.gameState == LevelManager.GameState.OnGameFinished)
            {
                return;
            }

            if (_isDeath || _doorOpened)
            {
                return;
            }
           
            CheckGround();
            CheckPlayerIsCrushed();
            CheckPlayerOnRecord();
            Move();
            PlayRunFx();
        }

        private void PlayRunFx()
        {
            if (_runLeft)
            {
                _runLeft = false;
                var instance = Instantiate(runFx, runFxTransform.position, Quaternion.identity);
                AudioManager.Manager.PlayOneShot(AudioManager.Manager.runClip);
            }

            if (_runRight)
            {
                _runRight = false;
                
                var instance = Instantiate(runFx, runFxTransform.position, Quaternion.identity);
                AudioManager.Manager.PlayOneShot(AudioManager.Manager.runClip);
                var scale = instance.transform.localScale;
                scale.x *= -1;
                instance.transform.localScale = scale;
            }
        }



        #endregion
        
        #region RecordSystem

        private IEnumerator CanStopRecordRoutine()
        {
            yield return new WaitForSeconds(0.3f);
            _canStopRecord = true;
        }
        private void OnRecordHappened()
        {
            if (!_isOnRecord)
            {
                StartCoroutine(RecordRoutine());
            }
        }

        private IEnumerator RecordRoutine()
        {
            _isOnRecord = true;
            _timeController.enabled = true;
            yield return new WaitUntil(() => UiManager.Manager.rewindFill.fillAmount <= 0.01f);
            ChangeToRecord();
            
        }

        private void ChangeToRecord()
        {
            //Calculations
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            _timeController.isReversing = true;
            _timeController.firstRun = true;
            
            //Visual Changes

            _renderer.sprite = _playerSkin.recordSprite;
            
            //Change tag and layer to record
            GameObject o;
            (o = gameObject).layer = LayerMask.NameToLayer("Record");
            o.tag = "Record";
            
            //Reports
            LevelManager.Manager.SpawnPlayer(transform.position);
            LevelManager.Manager.gameState = LevelManager.GameState.OnRecordEnd;
            LevelManager.Manager.recordList.Add(this);
            this.enabled = false;
        }
        

        #endregion
        
        #region BasicMovement

        private void Move()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            
            if (_horizontal < 0 && !_isFacingRight)
            {
                Flip();
            }
            else if (_horizontal > 0 && _isFacingRight)
            {
                Flip();
            }
            
            myRigidBody.velocity = new Vector2(_horizontal*moveSpeed,myRigidBody.velocity.y);

        }
        
        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.W) && _curJumpCombo < maxJumpCombo)
            {
                
                myRigidBody.velocity = Vector2.up * jumpSpeed;
                _curJumpCombo++;
            }
            else if (Input.GetKeyDown(KeyCode.W) && _isGrounded && _curJumpCombo == maxJumpCombo)
            {
                myRigidBody.velocity = Vector2.up * jumpSpeed;
                
            }
        
        }

    
        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            var transform1 = transform;
            var scale = transform1.localScale;
            scale.x *= -1;
            transform1.localScale = scale;
        }


        #endregion
        
        #region CheckSystems

        private void CheckGround()
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundLayer);
            if (_isGrounded)
            {
                _curJumpCombo = 0;
            }
        }

        private void CheckPlayerIsCrushed()
        {
            _isCrushed = Physics2D.OverlapCircle(transform.position, crushedRadius, crushedLayer);
            if (_isCrushed)
            {
            }
        }

        private void CheckPlayerOnRecord()
        {
            
            _playerIsOnRecordObject = Physics2D.OverlapCircle(groundCheckPosition.position, recordRadius, recordLayer);

        }


        private IEnumerator DelayReset()
        {
            yield return new WaitForSeconds(1f);
            LevelManager.Manager.ResetScene();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle") && !_isTriggered)
            {
                _isTriggered = true;
                if (LevelManager.Manager.gameState == LevelManager.GameState.OnRecording)
                {
                    Debug.Log("Obstacle hit on record");
                    _rb.velocity = Vector2.zero;;
                    _rb.isKinematic = true;
                    _isDeath = true;
                }
                else
                {
                    Debug.Log("Obstacle hit");
                    _rb.velocity = Vector2.zero;;
                    _rb.isKinematic = true;
                    if (!_isDeath)
                    {
                        StartCoroutine(DelayReset());
                    }
                    _isDeath = true;
                    
                    
                }
            }

            // if (gameObject.CompareTag("Player") && LevelManager.Manager.gameState != LevelManager.GameState.OnRecording)
            // {
            //     if (other.CompareTag("Key"))
            //     {
            //         LevelManager.Manager.isKeyCollected = true;
            //         Destroy(other.gameObject);
            //     }
            //
            //     if (other.CompareTag("Coin"))
            //     {
            //         LevelManager.Manager.isCoinCollected = true;
            //         Destroy(other.gameObject);
            //     }
            // }
            
            

            if (other.CompareTag("Door") && gameObject.CompareTag("Player") && LevelManager.Manager.gameState != LevelManager.GameState.OnRecording)
            {
                if (LevelManager.Manager.isKeyCollected && !_doorOpened)
                {
                    _doorOpened = true;
                    StopAllCoroutines();
                    transform.DOMove(other.transform.position, 1f).SetEase(Ease.Linear);
                }
            }
            
        }

        private IEnumerator DelayFinish()
        {
            yield return new WaitForSeconds(1.5f);
        }
        


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Record") && _playerIsOnRecordObject)
            {
                transform.SetParent(other.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Record"))
            {
                transform.parent = null;
            }
        }

        #endregion
        
        
    }
}
