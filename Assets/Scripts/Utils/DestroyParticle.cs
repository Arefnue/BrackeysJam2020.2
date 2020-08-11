using UnityEngine;

namespace Utils
{
    public class DestroyParticle : MonoBehaviour
    {


        public void DisableObject()
        {
            gameObject.SetActive(false);
        }
        
        public void Death()
        {
            Destroy(gameObject);
        }
    }
}
