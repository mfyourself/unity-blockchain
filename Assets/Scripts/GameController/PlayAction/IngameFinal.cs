using MahJongController;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

namespace MahJongController
{
    public class IngameFinal : MonoBehaviour
    {

        public void ShowFinalPoints(GameInfoData gameInfo, int myPlace)
        {
            GameObject ScorePanel = this.transform.GetChild(0).gameObject;
            ScorePanel.SetActive(true);
            for (int i = 0; i < gameInfo.roomInfo.maxOfPlayer; i++)
            {
                UserData user = GetRanks(gameInfo)[i];
                transform.Find($"ScorePanel/Rank{i}/Avatar").GetComponent<Image>().sprite = GetAvatarImage(string.Format("chara_b_{0}", user.avatar), "UITextures/GameUI/ingame/chara_b");                       // name
                transform.Find($"ScorePanel/Rank{i}/BlackPanel/UserName").GetComponent<Text>().text = user.userName;                       // name
                transform.Find($"ScorePanel/Rank{i}/BlackPanel/PointPanel").GetComponent<PointSettings>().SetPoint(user.score.ToString()); //score
                bool isMe = (myPlace == user.playerplace) ? true : false;
                transform.Find($"ScorePanel/Rank{i}/Me").gameObject.SetActive(isMe);                                                       //isMe
            }

        }

        private UserData[] GetRanks(GameInfoData gameInfo)
        {
            List<UserData> userDatas = new List<UserData>() { gameInfo.user1, gameInfo.user2, gameInfo.user3, gameInfo.user4 };
            List<UserData> SortedList = userDatas.OrderBy(o => o.score).ToList();
            SortedList.Reverse();
            return SortedList.ToArray();
        }

        private static Sprite GetAvatarImage(string image, string source)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>(source);
                Sprite titleSprite = null;
                foreach (Sprite sprite in sprites)
                {
                    if (sprite.name == image)
                    {
                        titleSprite = sprite;
                        break;
                    }
                }
                return titleSprite;
            }
            return null;
        }

    }
}