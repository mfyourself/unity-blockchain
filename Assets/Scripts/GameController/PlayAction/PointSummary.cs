using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MahJongController
{
    public class PointSummary : MonoBehaviour
    {
        private bool animationCompleted;

        public void ShowPointSummary(string[] handTiles, string[] uraDoraTiles, string[] hiddenDoraTiles, bool AgariType, string playerName, string roundScore, string playerCutIn, string[] yakuList, int roomRound, int han, int fu)
        {
            var Summary = this.transform.GetChild(0).gameObject;
            Summary.SetActive(true);
            var Content = Summary.transform.GetChild(1).gameObject;
            var playerImage = Content.transform.GetChild(0).gameObject;
            var handTilesObj = Content.transform.GetChild(1).gameObject;
            var uraDoraTilesObj = Content.transform.GetChild(2).gameObject;
            var hiddenDoraTilesObj = Content.transform.GetChild(3).gameObject;
            var hanObj = Content.transform.GetChild(4).gameObject;
            var winMethodObj = Content.transform.GetChild(5).gameObject;
            var pointObj = Content.transform.GetChild(7).gameObject;
            var playerNameObj = Content.transform.GetChild(8).gameObject;
            var yakuObj = Content.transform.GetChild(9).gameObject;
            var yakuItem = yakuObj.transform.GetChild(0).gameObject;
            var roundItem = yakuObj.transform.GetChild(1).gameObject;
            var fuObj = Content.transform.GetChild(10).gameObject;
            var winTitleObj = Content.transform.GetChild(6).gameObject;

            if (Convert.ToInt32(roundScore) < 8000)
            {
                winTitleObj.SetActive(false);
            } else
            {
                winTitleObj.SetActive(false);

            }

            for (int i = 0; i < 14; i++)
            {
                GameObject handTilesObject = handTilesObj.transform.GetChild(i).gameObject;
                if (handTiles.Length > 13)
                {
                    Image image = handTilesObject.GetComponent<Image>();
                    image.sprite = GetImageResource(handTiles[i], "UITextures/GameUI/Room/ui");
                }
            }

            for (int i = 0; i < yakuList.Length; i++)
            {
                yakuItem.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = yakuList[i];
                roundItem.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = string.Format("{0}翻", roomRound.ToString());
            }

            if (AgariType == true)
            {
                winMethodObj.transform.GetChild(0).gameObject.SetActive(false);
                winMethodObj.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                winMethodObj.transform.GetChild(0).gameObject.SetActive(true);
                winMethodObj.transform.GetChild(1).gameObject.SetActive(false);
            }

            for (int i = 0; i < 5; i++)
            {
                var uradoraTilesObject = uraDoraTilesObj.transform.GetChild(i + 1).gameObject;
                var hiddendoraTilesObject = hiddenDoraTilesObj.transform.GetChild(i + 1).gameObject;
                Image uraimage = uradoraTilesObject.GetComponent<Image>();
                Image hiddenimage = hiddendoraTilesObject.GetComponent<Image>();
                if (uraDoraTiles.Length > i)
                    uraimage.sprite = GetImageResource(uraDoraTiles[i], "UITextures/GameUI/Room/ui");
                if (hiddenDoraTiles.Length > i)
                    hiddenimage.sprite = GetImageResource(hiddenDoraTiles[i], "UITextures/GameUI/Room/ui");
            }

            fuObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = string.Format("{0} 符", fu.ToString());
            hanObj.transform.GetChild(0).gameObject.GetComponent<PointSettings>().SetPoint(han.ToString());
            pointObj.transform.GetChild(0).gameObject.GetComponent<PointSettings>().SetPoint(roundScore);
            var playerNameObject = playerNameObj.transform.GetChild(0).gameObject;
            Text playerNameText = playerNameObject.GetComponent<Text>();
            playerNameText.text = playerName;
            StartCoroutine(FadeInAndOut(playerCutIn, playerImage));
        }

        private IEnumerator FadeInAndOut(string avatarImage, GameObject imageObject)
        {
            Image image = imageObject.GetComponent<Image>();
            image.sprite = GetImageResource(avatarImage, "UITextures/GameUI/ingame/chara_cutin");
            if (image == null)
            {
                yield break; // No Image component found on the GameObject
            }

            Color originalColor = image.color;
            float fadeInDuration = 0.5f;
            float displayDuration = 3f;
            animationCompleted = false;
            // Fade in
            for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / fadeInDuration;
                image.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, normalizedTime));
                yield return null;
            }
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);

            yield return new WaitForSeconds(displayDuration);
            animationCompleted = true;
            var Summary = this.transform.GetChild(0).gameObject;
            Summary.SetActive(false);
        }

        public static Sprite GetImageResource(string image, string source)
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

        public bool IsAnimationCompleted()
        {
            return animationCompleted;
        }

    }
}