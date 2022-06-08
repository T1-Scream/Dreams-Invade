using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadScreen : MonoBehaviour
{
    public TMP_Text loadingText;
    public TMP_Text pKeyText;
    public TMP_Text pKeyFadeText;
    public string sceneName;

    private bool waitToLoad;

    private void OnEnable()
    {
        waitToLoad = false;
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously (string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);
        waitToLoad = true;

        while (!operation.isDone) {

            if (operation.progress >= .9f && !operation.allowSceneActivation) {

                if (Input.anyKeyDown)
                    operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void LateUpdate()
    {
        if (waitToLoad) {
            pKeyText.gameObject.SetActive(true);
            pKeyFadeText.gameObject.SetActive(true);
            loadingText.gameObject.SetActive(false);
            waitToLoad = !waitToLoad;
        }
    }
}
