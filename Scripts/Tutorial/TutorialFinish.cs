using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialFinish : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText = null;

    private bool finish = true;
    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (finish) {
            tutorialText.text = Lean.Localization.LeanLocalization.GetTranslationText("Finished");
            gameManager = FindObjectOfType<GameManager>();
            StartCoroutine("FinishTutorial");
            finish = false;
        }
    }

    private void Awake()
    {
        tutorialText = GameObject.Find("TipsText").GetComponent<TMP_Text>(); // load scene missing object
    }

    private IEnumerator FinishTutorial()
    {
        yield return TutorialManager.Wait.ThreeSecond;
        tutorialText.gameObject.SetActive(false);
        gameManager.GoToMenu();
        StopCoroutine("FinishTutorial");
    }
}
