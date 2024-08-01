using System.Collections;
using UI.ResourcesBundle;
using UnityEngine;
using MahjongAuthController;
using UnityAuthStructure;
using UnityEngine.UI;

//using Utils;

namespace MahJongController
{

    public class PointSettings : MonoBehaviour
    {
        [Header("Number settings")]
        public Transform NumberParent;
        public GameObject DigitPrefab;
        // Use this for initialization

        public void SetPoint(string str_Point)
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++) {
                if(i >= 0)
                    Destroy(transform.GetChild(0).GetChild(i).gameObject);
            }
            for (int i = 0; i < str_Point.Length; i++)
            {
                var obj = Instantiate(DigitPrefab, NumberParent);
                obj.name = $"Digit{i}";
                var image = obj.GetComponent<Image>();
                string image_name = string.Format("num1_{0}", str_Point[i].ToString());
                Sprite img = GetPointImage(image_name);
                if(img)
                    image.sprite = img;
                else
                    obj.gameObject.SetActive(false);
            }

        }
        public void SetGoldPoint(string str_Point)
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                if (i >= 0)
                    Destroy(transform.GetChild(0).GetChild(i).gameObject);
            }
            for (int i = 0; i < str_Point.Length; i++)
            {
                var obj = Instantiate(DigitPrefab, NumberParent);
                obj.transform.localScale = NumberParent.transform.localScale;
                obj.name = $"Digit{i}";
                var image = obj.GetComponent<Image>();
                string image_name = string.Format("num2_{0}", str_Point[i].ToString());
                image.sprite = GetPointImage(image_name);
            }

        }

        public static Sprite GetPointImage(string image)
        {
            if (image != "")
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("UITextures/GameUI/ingame/");
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