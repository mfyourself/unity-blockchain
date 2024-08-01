using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using TMPro;

namespace MahJongController
{
    public class StatusBoard : MonoBehaviour
    {
        public GameObject[] ImgDoras;
        public GameObject[] Textpanel;
        public Text Title;
        private bool flag = false;
        public Text HanCnt;
        public Text RiichtCnt;


        private int richCnt = 0;
        
        public void PutDoras(string[] arrDoras)
        {
            for (int i = 0; i < arrDoras.Length; i++)
            {
                Image image = ImgDoras[i].GetComponent<Image>();
                image.sprite = HandTiles.GetTileImage(arrDoras[i]);
                ImgDoras[i].SetActive(true);
            }



        }

        public void PutTitle(int roomType, int maxPlayer)
        {
            if (flag)
                return;
            string direction = (roomType == 1) ? "東" : "半";
            string memCount = (maxPlayer == 3) ? "三" : "四";
            Title.text = $"{direction}風戦 ・ {memCount}人打ち";
            flag = true;
        }

        public void AddRiichiCount()
        {
            RiichtCnt.text = Convert.ToString(Convert.ToInt32(RiichtCnt.text) + 1);
        }
        
        public void ClearRiichiCount()
        {
            RiichtCnt.text = "0";
        }

        public void AddHanCount()
        {
            HanCnt.text = Convert.ToString(Convert.ToInt32(HanCnt.text) + 1);
        }

    }
}