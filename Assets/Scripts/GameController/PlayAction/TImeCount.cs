using CommonUtils.Serializables.SerializableArray2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;


namespace MahJongController
{

    public class TimeCount : MonoBehaviour
    {
        public GameObject BonusTime, Plus, BaseTime;
        public GameInfoData gameInfoData;
        public void DisplaySeonds(int now, int baseTIme, int BonusTime)
        {
            gameInfoData = HandTiles.gameInfoData;
            if (baseTIme == now)
            {
                if (gameInfoData.roomInfo.roomEventHint == Convert.ToInt32(Constant.GAME_EVENTS_RON))
                {
                    FindObjectOfType<HandTiles>().DisableAllNotification();
                    FindObjectOfType<HandTiles>().Ron();

                }
                else if (gameInfoData.roomInfo.roomEventHint == Convert.ToInt32(Constant.GAME_EVENTS_TSUMO))
                {
                    FindObjectOfType<HandTiles>().DisableAllNotification();
                    FindObjectOfType<HandTiles>().Tusmo();

                }
                else if (gameInfoData.roomInfo.roomEventHint == Convert.ToInt32(Constant.GAME_EVENTS_RIICHI))
                {
                    //FindObjectOfType<HandTiles>().DisableAllNotification();
                    //FindObjectOfType<HandTiles>().Tusmo();

                }
                else
                {
                    FindObjectOfType<HandTiles>().DisableAllNotification();
                    //FindObjectOfType<HandTiles>().CancelCPK();
                }
            }

            if (now < baseTIme)
            {
                string image_name = $"num1_{baseTIme - now}";
                BaseTime.SetActive(true);
                BaseTime.GetComponent<Image>().sprite = PointSettings.GetPointImage(image_name);
                ShowBonusTime(BonusTime);
            }
            else
            {
                BaseTime.gameObject.SetActive(false);
                Plus.SetActive(false);
                ShowBonusTime(BonusTime + baseTIme - now);
            }
            Plus.gameObject.SetActive(BaseTime.gameObject.activeSelf);
            if (now >= baseTIme || GameController.myPlusTime == 0)
                Plus.gameObject.SetActive(false);
            if (BonusTime + baseTIme - now == 0)
                Clear();
        }

        public void Clear()
        {
            BaseTime.SetActive(false);
            Plus.SetActive(false);
            BonusTime.transform.GetChild(0).gameObject.SetActive(false);
            BonusTime.transform.GetChild(1).gameObject.SetActive(false);
        }

        public void ShowBonusTime(int second)
        {
            GameController.myPlusTime = second;
            if (second > 0)
            {

                BonusTime.transform.GetChild(0).gameObject.SetActive(true);
                string img_tenposition = $"num2_{(int)(second / 10)}";
                BonusTime.transform.GetChild(0).GetComponent<Image>().sprite = PointSettings.GetPointImage(img_tenposition);
                if (second < 10)
                    BonusTime.transform.GetChild(0).gameObject.SetActive(false);

                BonusTime.transform.GetChild(1).gameObject.SetActive(true);
                string secondLetterImg = $"num2_{second - (((int)(second / 10)) * 10)}";
                BonusTime.transform.GetChild(1).GetComponent<Image>().sprite = PointSettings.GetPointImage(secondLetterImg);
            }
            else
            {
                BonusTime.transform.GetChild(0).gameObject.SetActive(false);
                BonusTime.transform.GetChild(1).gameObject.SetActive(false);
            }

        }

    }

}