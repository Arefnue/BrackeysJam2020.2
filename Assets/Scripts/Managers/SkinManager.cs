using System;
using System.Collections.Generic;
using Controllers;
using Data;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class SkinManager : MonoBehaviour
    {
        #region Singleton

        public static SkinManager Manager;


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

        public List<SkinController> skinList;
        public TextMeshProUGUI coinText;


        private void Start()
        {
            SetSkins();
            SetCoin();
        }


        public void SelectSkin(SkinController controller)
        {
            if (controller.CanSelect)
            {
                GameManager.Manager.playerProfile.CurrentSkin = controller.skinProfile;
            }
            else
            {
                if (controller.CanBuy)
                {
                    if (GameManager.Manager.playerProfile.TotalCoin >= controller.skinProfile.price)
                    {
                        GameManager.Manager.playerProfile.TotalCoin -= controller.skinProfile.price;
                        SetCoin();
                        GameManager.Manager.playerProfile.OpenSkinList.Add(controller.skinProfile);
                    }
                }
            }
            SetSkins();
            
        }
        
        public void SetSkins()
        {
            foreach (var skinController in skinList)
            {
                if (GameManager.Manager.playerProfile.OpenSkinList.Contains(skinController.skinProfile))
                {
                    if (GameManager.Manager.playerProfile.CurrentSkin == skinController.skinProfile)
                    {
                        skinController.SetController(isSelected:true);
                    }
                    else
                    {
                        skinController.SetController(canSelect:true);
                    }
                }
                else
                {
                    skinController.SetController(canBuy:true);
                }
            }
        }

        private void SetCoin()
        {
            coinText.text = $"{GameManager.Manager.playerProfile.TotalCoin.ToString()}x ";
        }
        
    }
}
