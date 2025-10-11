// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class SceneFader : MonoBehaviour
// {
//     private CanvasGroup _canvasGroup;
//     
//     public float fadeDuration;
//     public float fadeoutDuration;
//
//     private void Awake()
//     {
//         _canvasGroup = GetComponent<CanvasGroup>();
//         _canvasGroup.alpha = 0f;
//         _canvasGroup.interactable = false;
//         _canvasGroup.blocksRaycasts = false;
//
//         DontDestroyOnLoad(gameObject);
//     }
//
//     public IEnumerator FadeOut(float time)
//     {
//         _canvasGroup.interactable = true;
//         _canvasGroup.blocksRaycasts = true;
//
//         float timer = 0f;
//         while (timer < time)
//         {
//             timer += Time.deltaTime;
//             _canvasGroup.alpha = Mathf.Clamp01(timer / time);
//             yield return null;
//         }
//     }
//
//     public IEnumerator FadeIn(float time)
//     {
//         float timer = 0f;
//         while (timer < time)
//         {
//             timer += Time.deltaTime;
//             _canvasGroup.alpha = Mathf.Clamp01(1f - (timer / time));
//             yield return null;
//         }
//
//         _canvasGroup.alpha = 0f;
//         _canvasGroup.interactable = false;
//         _canvasGroup.blocksRaycasts = false;
//
//         Destroy(gameObject);
//     }
//     public IEnumerator FadeOutAndIn()
//     {
//         yield return FadeOut(fadeDuration);
//         yield return FadeIn(fadeDuration);
//     }
// }
//
