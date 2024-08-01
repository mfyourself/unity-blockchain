using DG.Tweening;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MahJongController
{
    public class EffecctPanel : MonoBehaviour
    {
        public GameObject[] RonImage;
        private bool animationCompleted;

        public void RonRiichiNotification(int order)
        {
            // Assuming 'order' corresponds to the index of RonImage array
            if (order >= 0 && order < RonImage.Length)
            {
                RonImage[order].SetActive(true);
                StartCoroutine(FadeInAndOut(RonImage[order]));
            }
        }

        private IEnumerator FadeInAndOut(GameObject imageObject)
        {
            Image image = imageObject.GetComponent<Image>();
            if (image == null)
            {
                yield break; // No Image component found on the GameObject
            }

            Color originalColor = image.color;
            float fadeInDuration = 0.3f;
            float displayDuration = 1f;
            float fadeOutDuration = 0.7f;
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

            // Fade out
            for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
            {
                float normalizedTime = t / fadeOutDuration;
                image.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, normalizedTime));
                yield return null;
            }
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            imageObject.SetActive(false);
            animationCompleted = true;
        }

        public bool IsAnimationCompleted()
        {
            return animationCompleted;
        }

    }
}