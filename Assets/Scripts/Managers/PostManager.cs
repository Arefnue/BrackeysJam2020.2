using System;
using UnityEngine;

namespace Managers
{
    public class PostManager : MonoBehaviour
    {
        #region Singleton

        public static PostManager Manager;


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


        public GameObject recordPost;
        public GameObject rewindPost;
        public GameObject normalPost;

        
    }
}
