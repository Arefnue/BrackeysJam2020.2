using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Skin", fileName = "Skin 0")]
    public class SkinSO : ScriptableObject
    {
        public int skinId;
        public Sprite normalSprite;
        public Sprite recordSprite;

        [Header("Purchase")] 
        public bool canBuy;
        public int price;
    }
}
