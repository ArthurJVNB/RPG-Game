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
        [SerializeField] float fadeTime = 1f;

        CanvasGroup canvasGroup;
        Scene originalScene;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            originalScene = SceneManager.GetActiveScene();
        }

        private void OnEnable() => Portal.onStartedLoading += StartFade;

        private void OnDisable() => Portal.onStartedLoading -= StartFade;

        private void StartFade()
        {
            StartCoroutine(FadeRoutine());
        }

        private IEnumerator FadeRoutine()
        {
            transform.SetParent(null, false);
            DontDestroyOnLoad(gameObject);

            canvasGroup.alpha = 0;

            yield return FadeInRoutine();
            yield return FadeOutRoutine();

            Destroy(gameObject);
        }

        private IEnumerator FadeInRoutine()
        {
            print("Started to fade in.");
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeTime;
                yield return null;
            }
            print("Ended fade in.");
        }

        private IEnumerator FadeOutRoutine()
        {
            print("Started to fade out.");
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeTime;
                yield return null;
            }
            print("Ended fade out.");
        }
    } 
}
