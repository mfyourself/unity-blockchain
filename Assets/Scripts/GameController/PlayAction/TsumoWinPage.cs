using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MahJongController
{
    public class TsumoWinPage : MonoBehaviour
    {
        public GameObject cutInImage;
        public GameObject backGround;
        private bool animationCompleted;

        public void StartAnimation(string avatarImage)
        {

            backGround.SetActive(true);
            cutInImage.SetActive(true);

            var image = cutInImage.GetComponent<Image>();
            image.sprite = GetAvatarImage(avatarImage);

            var sequence = DOTween.Sequence();
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(500, rectTransform.anchoredPosition.y);
            animationCompleted = false; // Reset the flag
            sequence.Append(image.DOFade(1, 0.5f))
                    .Join(rectTransform.DOAnchorPosX(0, 0.5f))
                    .AppendInterval(1.0f)
                    .Append(image.DOFade(0, 0.5f))
                    .Join(rectTransform.DOAnchorPosX(-500, 0.5f))
                    .OnComplete(() =>
                    {
                        rectTransform.anchoredPosition = new Vector2(0, rectTransform.anchoredPosition.y);
                        animationCompleted = true; // Set the flag to true when animation completes
                        backGround.SetActive(false);
                        cutInImage.SetActive(false);
                    });
        }

        public static Sprite GetAvatarImage(string image)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("UITextures/GameUI/ingame/chara_cutin");
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