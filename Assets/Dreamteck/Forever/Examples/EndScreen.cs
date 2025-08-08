namespace Dreamteck.Forever
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class EndScreen : MonoBehaviour
    {
        public delegate void EmptyHandler();
        static EndScreen instance;
        public CanvasGroup screenGroup;
        public static event EmptyHandler onRestartClicked;

        private void Awake()
        {
            instance = this;
        }

        public void Restart()
        {
            if (onRestartClicked != null) onRestartClicked();
            StopAllCoroutines();
            screenGroup.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        public static void Open()
        {
            instance.StartCoroutine(instance.FadeIn());
        }

        IEnumerator FadeIn()
        {
            screenGroup.gameObject.SetActive(true);
            screenGroup.alpha = 0f;
            while (screenGroup.alpha < 1f)
            {
                screenGroup.alpha = Mathf.MoveTowards(screenGroup.alpha, 1f, Time.unscaledDeltaTime);
                Time.timeScale = 1f - screenGroup.alpha;
                yield return null;
            }
        }

    }
}
