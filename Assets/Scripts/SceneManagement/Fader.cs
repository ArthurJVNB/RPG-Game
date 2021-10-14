using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        //public void StartFade()
        //{
        //    StartCoroutine(FadeRoutine());
        //}

        //private IEnumerator FadeRoutine()
        //{
        //    transform.SetParent(null, false);
        //    DontDestroyOnLoad(gameObject);

        //    canvasGroup.alpha = 0;

        //    yield return FadeInRoutine();
        //    yield return FadeOutRoutine();

        //    Destroy(gameObject);
        //}

        public IEnumerator FadeInRoutine(float time)
        {
            print("Started to fade in.");
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
            print("Ended fade in.");
        }

        public IEnumerator FadeOutRoutine(float time)
        {
            print("Started to fade out.");
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
            print("Ended fade out.");
        }
    } 
}
