using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager Manager;


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


        public AudioSource musicSource;
        public AudioSource oneShotSource;

        [Header("Clips")] 
        public AudioClip runClip;
        public AudioClip kazikDeath;
        public AudioClip recordStart;
        public AudioClip recording;
        public AudioClip recordEnd;
        public AudioClip normalMusic;
        public AudioClip rewindClip;
        public AudioClip collectSound;

        public void PlayOneShot(AudioClip clip)
        {
            oneShotSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        

    }
}
