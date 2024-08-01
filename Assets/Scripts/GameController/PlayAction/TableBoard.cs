using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MahJongController
{
    public class TableBoard : MonoBehaviour
    {
        public GameObject[] CurrentPanels;
        public GameObject[] Points;
        public GameObject[] Differents;
        public GameObject[] Richii;
        private int lastDealerTablePosition = -1;
        
            public void ShowCurrentUser(int order)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == order)
                {
                    CurrentPanels[i].SetActive(true);
                }
                else CurrentPanels[i].SetActive(false);
            }
        }

        public void SetRemindTilesDisplay(int cnt)
        {
            if(cnt >= 14)
            {
                Text tileCount = transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
                string tilecounts = string.Format("余 {0}", (cnt - 14));
                tileCount.text = tilecounts;
            }
        }
        
        public void SetRoundDisplay(int cnt)
        {
            if(cnt > 0)
            {
                Text tileCount = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
                string tilecounts = $"東{cnt}局";
                tileCount.text = tilecounts;
            }
        }

        public void SetDirection(int index)
        {
            float angle = (lastDealerTablePosition > -1 ? index - lastDealerTablePosition : index) * 90.0f;
            Debug.Log(index + ": TableBoard.cs, 35 : lastDealerPosition : " + lastDealerTablePosition + ", " + angle );
            transform.Find("Positions").Rotate(0.0f, 0.0f, angle, Space.Self);
            lastDealerTablePosition = index;
        }

        public void SetRichiiStatus(int index)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == index)
                {
                    Richii[i].SetActive(true);
                }
            }
        }

        public void ClearAllStatus()
        {
            for (int i = 0; i < 4; i++)
               Richii[i].SetActive(false);
        }

        public void SetPoints(GameInfoData gameInfoData, int myPlace)
        {
            List<UserData> users = new List<UserData> { gameInfoData.user1, gameInfoData.user2, gameInfoData.user3, gameInfoData.user4 };

            foreach (var user in users)
            {
                int positionIndex = (user.playerplace >= myPlace)? user.playerplace - myPlace : 4 + user.playerplace - myPlace;
                transform.Find($"Points/Point{positionIndex}").GetComponent<Text>().text = user.score.ToString();
            }
        }
    }
}