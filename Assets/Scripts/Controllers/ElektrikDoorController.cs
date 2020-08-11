using System;
using UnityEngine;

namespace Controllers
{
    public class ElektrikDoorController : MonoBehaviour
    {

        public bool startOpen;
        private BoxCollider2D _collider;
        private Animator _animator;
        private static readonly int State = Animator.StringToHash("State");


        private void Start()
        {
            
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            if (startOpen)
            {
                OpenDoor();
            }
        }


        public void OpenDoor()
        {
            _animator.SetInteger(State,1);
            _collider.enabled = false;
        }

        public void CloseDoor()
        {
            _animator.SetInteger(State,0);
            _collider.enabled = true;
        }
        
        
    }
}
