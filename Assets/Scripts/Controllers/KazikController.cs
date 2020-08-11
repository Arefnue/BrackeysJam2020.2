using System;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class KazikController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.Manager.PlayOneShot(AudioManager.Manager.kazikDeath);
            }
        }

    }
}
