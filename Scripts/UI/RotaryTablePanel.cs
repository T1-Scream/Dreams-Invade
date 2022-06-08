using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using QuantumTek.EncryptedSave;

public class RotaryTablePanel : MonoBehaviour
{
    [SerializeField] private Image loadingScreen;
    [SerializeField] private Button drawBtn;
    [SerializeField] private Transform drawParent;
    [SerializeField] private Transform HaloImage;
    [SerializeField] private Transform[] drawEvent;

    private bool isInitState;
    private bool drawEnd;
    private bool drawChoice;

    private float drawTime = 0.8f;
    private float drawTiming = 0f;

    private float loadScreenTime = 0f;
    private float loadDelayTime = 2f;
    private bool loadScreen;

    private int haloIndex = 0;
    private int drawIndex = 0;

    private bool isPlaying;

    private void OnEnable()
    {
        drawBtn.onClick.AddListener(Draw);
        drawEvent = new Transform[drawParent.childCount];
        for (int i = 0; i < drawParent.childCount; i++) {
            drawEvent[i] = drawParent.GetChild(i);
        }

        drawTime = 0.6f;
        drawTiming = 0;

        isInitState = false;
        drawEnd = false;
        drawChoice = false;
        isPlaying = false;
        loadScreen = false;
    }

    private void Update()
    {
        if (loadScreen) {

            loadScreenTime += Time.deltaTime;

            if (loadScreenTime >= loadDelayTime) {
                loadScreenTime = 0f;
                loadingScreen.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        if (drawEnd) return;

        drawTiming += Time.deltaTime;

        if (drawTiming >= drawTime && isInitState) {
            drawTiming = 0f;
            haloIndex++;

            if (haloIndex >= drawEvent.Length)
                haloIndex = 0;

            SetHaloPos(haloIndex);
        }
    }

    private void SetHaloPos(int index)
    {
        HaloImage.position = drawEvent[index].position;
        FindObjectOfType<AudioManager>().PlayLoopFasting("Draw", 0f);

        if (drawChoice && index == drawIndex) {
            isInitState = !isInitState;
            isPlaying = !isPlaying;
            drawEnd = true;
            loadScreen = true;
            ES_Save.Save(drawIndex, "masterDrawIndex");
        }
    }

    public void Draw()
    {
        if (!isPlaying) {
            drawIndex = Random.Range(0, drawEvent.Length);
            isInitState = true;
            isPlaying = true;
            drawEnd = false;
            drawChoice = false;
            StartCoroutine(StartDrawAni());
        }
    }

    IEnumerator StartDrawAni()
    {
        drawTime = 0.8f;

        for (int i = 0; i < 7; i++) {
            yield return new WaitForSeconds(0.1f);
            drawTime -= 0.1f;
        }


        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(0.1f);
            drawTime += 0.1f;
        }

        yield return new WaitForSeconds(1f);
        drawChoice = true;
        StopCoroutine(StartDrawAni());
    }
}
