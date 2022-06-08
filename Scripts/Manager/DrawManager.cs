using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject[] obj;
    [SerializeField] private Material lightMaterial;
    [SerializeField] public Animator missionDoorAnimator;

    public bool startRandom;

    [Header("Tutorial")]
    [SerializeField] private PuzzleVideoIsClear clearObj;
    public bool tutorialMode;

    private PlayerMovement movement;
    private float[] probabilityValue;
    private int pressedCount;

    Dictionary<int, string> objValue = new Dictionary<int, string>();

    private void Awake()
    {
        if (!tutorialMode) {
            movement = player.GetComponent<PlayerMovement>();
            movement.Speed = 2f;
            movement.canJump = true;
            movement.canCrouch = true;
            movement.jumpForce = 3.5f;
        }
        pressedCount = 0;
        button.tag = "MissionButton2";
        lightMaterial.color = Color.red;

        for (int i = 0; i < obj.Length; i++) {
            obj[i].SetActive(false);
        }
    }

    private void Start()
    {
        Init();
        IntProbabilityValue();
    }

    private void Init()
    {
        if (tutorialMode)
            probabilityValue = new float[5] { 2.4f, 2.4f, 2.4f, 2.4f, 0.4f }; // 1
        else {
            probabilityValue = new float[36] {2.849f, 2.849f, 2.849f, 2.849f, 2.849f, 2.849f, 2.849f, 2.849f, 2.849f, 2.849f,
            2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,
            2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,2.849f,
            2.849f,2.849f,2.849f,2.849f,2.849f,0.3f }; // 100
        }
    }

    private void IntProbabilityValue()
    {
        if (tutorialMode) {
            objValue.Add(0, "obj1");
            objValue.Add(1, "obj2");
            objValue.Add(2, "obj3");
            objValue.Add(3, "obj4");
            objValue.Add(4, "clear");
        }
        else {
            objValue.Add(0, "obj1");
            objValue.Add(1, "obj2");
            objValue.Add(2, "obj3");
            objValue.Add(3, "obj4");
            objValue.Add(4, "obj5");
            objValue.Add(5, "obj6");
            objValue.Add(6, "obj7");
            objValue.Add(7, "obj8");
            objValue.Add(8, "obj9");
            objValue.Add(9, "obj10");
            objValue.Add(10, "obj11");
            objValue.Add(11, "obj12");
            objValue.Add(12, "obj13");
            objValue.Add(13, "obj14");
            objValue.Add(14, "obj15");
            objValue.Add(15, "obj16");
            objValue.Add(16, "obj17");
            objValue.Add(17, "obj18");
            objValue.Add(18, "obj19");
            objValue.Add(19, "obj20");
            objValue.Add(20, "obj21");
            objValue.Add(21, "obj22");
            objValue.Add(22, "obj23");
            objValue.Add(23, "obj24");
            objValue.Add(24, "obj25");
            objValue.Add(25, "obj26");
            objValue.Add(26, "obj27");
            objValue.Add(27, "obj28");
            objValue.Add(28, "obj29");
            objValue.Add(29, "obj30");
            objValue.Add(30, "obj31");
            objValue.Add(31, "obj32");
            objValue.Add(32, "obj33");
            objValue.Add(33, "obj34");
            objValue.Add(34, "obj35");
            objValue.Add(35, "missionDoor");
        }
    }

    private int Draw(float[] probability)
    {
        float total = 0;

        for (int i = 0; i < probability.Length; i++) {
            total += probability[i];
        }
        float rdNum = Random.Range(0, total);
        for (int i = 0; i < probability.Length; i++) {
            if (rdNum < probability[i])
                return i;
            else
                rdNum -= probability[i];
        }

        return probability.Length - 1;
    }

    public void DrawRandomEvent()
    {
        if (startRandom) {

            if (tutorialMode) { 

                for (int i = 0; i < 1; i++) {
                    if (pressedCount == 4) {
                        button.tag = "Untagged";
                        pressedCount = -1;
                        lightMaterial.color = Color.green;
                        clearObj.clear = true;
                    }

                    string name = objValue[Draw(probabilityValue)];
                    switch (name) {
                        case "obj1":
                            obj[0].SetActive(true);
                            break;
                        case "obj2":
                            obj[1].SetActive(true);
                            break;
                        case "obj3":
                            obj[2].SetActive(true);
                            break;
                        case "obj4":
                            break;
                        case "clear":
                            button.tag = "Untagged";
                            lightMaterial.color = Color.green;
                            clearObj.clear = true;
                            break;
                    }
                    pressedCount += 1;
                }
            }
            else {
                for (int i = 0; i < 1; i++) {

                    if (pressedCount == 35) {
                        missionDoorAnimator.Play("DoorOpen", 0, 0.0f);
                        button.tag = "Untagged";
                        pressedCount = -1;
                        lightMaterial.color = Color.green;
                    }

                    string name = objValue[Draw(probabilityValue)];
                    switch (name) {
                        case "obj1":
                            obj[0].SetActive(true);
                            break;
                        case "obj2":
                            obj[1].SetActive(true);
                            break;
                        case "obj3":
                            obj[2].SetActive(true);
                            break;
                        case "obj4":
                            obj[3].SetActive(true);
                            break;
                        case "obj5":
                            obj[4].SetActive(true);
                            break;
                        case "obj6":
                            obj[5].SetActive(true);
                            break;
                        case "obj7":
                            obj[6].SetActive(true);
                            break;
                        case "obj8":
                            obj[7].SetActive(true);
                            break;
                        case "obj9":
                            obj[8].SetActive(true);
                            break;
                        case "obj10":
                            obj[9].SetActive(true);
                            break;
                        case "obj11":
                            obj[10].SetActive(true);
                            break;
                        case "obj12":
                            obj[11].SetActive(true);
                            break;
                        case "obj13":
                            obj[12].SetActive(true);
                            break;
                        case "obj14":
                            obj[13].SetActive(true);
                            break;
                        case "obj15":
                            obj[14].SetActive(true);
                            break;
                        case "obj16":
                            obj[15].SetActive(true);
                            break;
                        case "obj17":
                            obj[16].SetActive(true);
                            break;
                        case "obj18":
                            obj[17].SetActive(true);
                            break;
                        case "obj19":
                            obj[18].SetActive(true);
                            break;
                        case "obj20":
                            obj[19].SetActive(true);
                            break;
                        case "obj21":
                            obj[20].SetActive(true);
                            break;
                        case "obj22":
                            obj[21].SetActive(true);
                            break;
                        case "obj23":
                            obj[22].SetActive(true);
                            break;
                        case "obj24":
                            obj[23].SetActive(true);
                            break;
                        case "obj25":
                            obj[24].SetActive(true);
                            break;
                        case "obj26":
                            obj[25].SetActive(true);
                            break;
                        case "obj27":
                            obj[26].SetActive(true);
                            break;
                        case "obj28":
                            obj[27].SetActive(true);
                            break;
                        case "obj29":
                            obj[28].SetActive(true);
                            break;
                        case "obj30":
                            obj[29].SetActive(true);
                            break;
                        case "obj31":
                            break;
                        case "obj32":
                            break;
                        case "obj33":
                            break;
                        case "obj34":
                            break;
                        case "obj35":
                            break;
                        case "missionDoor":
                            FindObjectOfType<AudioManager>().Play("AutoDoor", 0f);
                            missionDoorAnimator.Play("DoorOpen", 0, 0.0f);
                            button.tag = "Untagged";
                            lightMaterial.color = Color.green;
                            break;
                    }
                    pressedCount += 1;
                }
            }
            startRandom = !startRandom;
        }
    }

    private void Update()
    {
        DrawRandomEvent();
    }
}
