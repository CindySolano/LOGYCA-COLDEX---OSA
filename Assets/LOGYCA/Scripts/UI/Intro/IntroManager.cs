using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroManager : MonoBehaviour
{

    public CanvasGroup cvGroupIntro, cvGroupInstruction;
    [SerializeField] private float fadeDuration = 0.5f;

    public GameObject cameraIntro, cameraGame;
    public List<GameObject> wallObjs = new List<GameObject>();

    public void OnStartIntroButton()
    {
        StartCoroutine(StartIntro());
    }

    private IEnumerator StartIntro()
    {
        yield return FadeCanvasGroup(cvGroupIntro, 1f, 0f, fadeDuration);

        cvGroupIntro.gameObject.SetActive(false);
        cvGroupInstruction.gameObject.SetActive(true);


        cvGroupInstruction.interactable = true;
        cvGroupInstruction.blocksRaycasts = true;
        yield return FadeCanvasGroup(cvGroupInstruction, 0f, 1f, fadeDuration);
       
    }

    public void OnStartGame()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        cvGroupInstruction.interactable = false;
        cvGroupInstruction.blocksRaycasts = false;
        yield return FadeCanvasGroup(cvGroupInstruction, 1f, 0f, fadeDuration);
        cvGroupInstruction.gameObject.SetActive(false);
        cameraIntro.SetActive(false);
        cameraGame.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        TurnOffObjects();

    }

    public void TurnOffObjects()
    {
        foreach (GameObject obj in wallObjs)
        {
            obj.SetActive(false);
        }

    }


    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            canvasGroup.alpha = Mathf.Lerp(from, to, t);

            yield return null;
        }

        canvasGroup.alpha = to;
    }


}
