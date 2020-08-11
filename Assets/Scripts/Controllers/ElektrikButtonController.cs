using System;
using UnityEngine;

namespace Controllers
{
    public class ElektrikButtonController : MonoBehaviour
    {
        public ElektrikDoorController targetDoor;
        private Animator _animator;
        private static readonly int Open = Animator.StringToHash("Open");

        public bool canControlSecondDoor;
        public bool secondDoorIsOpen;
        public ElektrikDoorController secondDoor;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Record"))
            {
                targetDoor.OpenDoor();
                if (canControlSecondDoor)
                {
                    if (secondDoorIsOpen)
                    {
                        secondDoor.OpenDoor();
                    }
                    else
                    {
                        secondDoor.CloseDoor();
                    }
                }
                _animator.SetBool(Open,true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Record"))
            {
                targetDoor.CloseDoor();
                
                if (canControlSecondDoor)
                {
                    if (secondDoorIsOpen)
                    {
                        secondDoor.CloseDoor();
                    }
                    else
                    {
                        secondDoor.OpenDoor();
                    }
                }
                _animator.SetBool(Open,false);
                
            }
        }
    }
}
