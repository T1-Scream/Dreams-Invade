using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static class Wait
    {
        public static readonly WaitForSeconds OneSecond = new WaitForSeconds(1f);
        public static readonly WaitForSeconds TwoSecond = new WaitForSeconds(2f);
        public static readonly WaitForSeconds ThreeSecond = new WaitForSeconds(3f);
    }

    public static TutorialManager instance;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject killer;
    [SerializeField] private GameObject button;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private TMP_Text tutorialText = null;
    [SerializeField] private PuzzleVideoIsClear[] clearObj;

    private PlayerMovement movement;
    private bool doorIsOpened;

    [HideInInspector] public bool tutorialIsFinished = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BeginTutorial();
    }

    private void BeginTutorial()
    {
        tutorialText.fontSize = 50;
        tutorialText.alignment = TextAlignmentOptions.Center;
        movement = player.GetComponent<PlayerMovement>();
        movement.canCrouch = true;
        movement.canJump = true;
        movement.jumpForce = 3.8f;
        gameManager.ShowTimerUI = false;
        gameManager.startTimer = false;
        gameManager.crossHair.SetActive(true);
        doorIsOpened = false;
        StartCoroutine(TutorialTips());
    }

    public void ShowTutorialText(string TranslationText)
    {
        tutorialText.text = Lean.Localization.LeanLocalization.GetTranslationText(TranslationText);
    }

    private bool AllItemInputComplete()
    {
        for (int i = 0; i < clearObj.Length; i++) {
            if (clearObj[i].clear == false)
                return false;
        }

        return true;
    }

    private void Update()
    {
        if (doorIsOpened) return;

        if (AllItemInputComplete()) {
            FindObjectOfType<AudioManager>().Play("AutoDoor", 0f);
            doorAnimator.Play("DoorOpen", 0, 0.0f);
            doorIsOpened = true;
        }
    }

    IEnumerator TutorialTips()
    {
        yield return new WaitForSeconds(2f);
        ShowTutorialText("Tutorial");
        yield return new WaitForSeconds(5f);
        tutorialText.text = string.Empty;
        StopCoroutine(TutorialTips());
    }
}
