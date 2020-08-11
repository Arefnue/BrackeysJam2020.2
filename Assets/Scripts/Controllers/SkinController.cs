using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class SkinController : MonoBehaviour
    {
        public SkinSO skinProfile;
        public Image skinImage;
        public GameObject selectObject;
        public GameObject selectedObject;
        public GameObject buyObject;
        public TextMeshProUGUI buyText;
        
        public bool CanSelect { get; set; }
        public bool CanBuy { get; set; }

        private void Start()
        {
            skinImage.sprite = skinProfile.normalSprite;
        }


        public void SetController(bool isSelected = false,bool canSelect = false, bool canBuy = false)
        {
            selectedObject.SetActive(isSelected);
            selectObject.SetActive(canSelect);
            buyObject.SetActive(canBuy);
            CanSelect = canSelect;
            CanBuy = canBuy;

            if (canBuy)
            {
                buyText.text = $"{skinProfile.price.ToString()}x";
            }
        }
        
    }
}
