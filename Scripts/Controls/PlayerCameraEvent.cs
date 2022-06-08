using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using QuantumTek.EncryptedSave;
using TMPro;

public class PlayerCameraEvent : MonoBehaviour
{
    public static PlayerCameraEvent instance;
    public Animator LiftDoorAnimator;
    public Animator missionDoorAnimator;
    [SerializeField] private TMP_Text keyText = null;
    [SerializeField] private TMP_Text tips = null;

    public bool canDestroy;
    public bool haveMission;
    public bool randomDraw;

    [HideInInspector] public bool opened = false;
    [HideInInspector] public bool item1Destroyed = false;
    [HideInInspector] public bool item2Destroyed = false;
    [HideInInspector] public bool item3Destroyed = false;

    private bool isButton;
    private bool isTarget;
    private bool isClear;
    private bool randomEvent;
    private bool doorIsOpened;
    private bool isComplete;
    private KeyCode keyInputItem;

    private float time;
    private readonly float delayTime = 10f;

    private readonly string clone1 = "printed-circuit(Clone)";
    private readonly string clone2 = "cross-wood(Clone)";
    private readonly string clone3 = "flashlight(Clone)";
    string SceneName;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1.5f);
    }

    private void Awake()
    {
        time = 10f;
        doorIsOpened = false;
        opened = false;
        SceneName = SceneManager.GetActiveScene().name;
        keyInputItem = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyItem"));
        keyText.text = keyInputItem.ToString();
    }

    bool FindObjectByTag(Transform Camera, float radius, string tag)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.position, transform.TransformDirection(Vector3.forward), out hit, radius)) {
            if (hit.collider.gameObject.CompareTag(tag))
                return true;
        }
        return false;
    }

    bool FindObjectByName(string name)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2)) {
            if (hit.collider.gameObject.name == name)
                return true;
        }

        return false;
    }

    private void OpenElevatorDoor()
    {
        isButton = FindObjectByTag(transform, 2, "Button");

        if (isButton && !doorIsOpened) {
            ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("Press"));

            if (Input.GetKey(keyInputItem)) {
                SwitchGameTips("Elevator");
                FindObjectOfType<AudioManager>().Play("PressButton", 0f);
                FindObjectOfType<AudioManager>().Play("Elevator", 0.5f);
                LiftDoorAnimator.Play("LiftDoorOpen", 0, 0.0f);
                FindObjectOfType<KillerMovement>().KillerRampage(80);
                FindObjectOfType<AudioManager>().Play("Stare90", 0f);
                doorIsOpened = true;
            }

            if (doorIsOpened) ToolTipsManager.instance.HideToolTip();
        }
        else
            ToolTipsManager.instance.HideToolTip();
    }

    private void DestroyObject()
    {
        isTarget = FindObjectByTag(transform, 2, "Destroy");

        if (isTarget) {
            ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("Destroy"));

            if (Input.GetKey(keyInputItem)) {
                if (FindObjectByName(clone1)) {
                    Destroy(GameObject.Find(clone1));
                    item1Destroyed = true;
                }
                else if (FindObjectByName(clone2)) {
                    Destroy(GameObject.Find(clone2));
                    item2Destroyed = true;
                }
                else if (FindObjectByName(clone3)) {
                    Destroy(GameObject.Find(clone3));
                    item3Destroyed = true;
                }
            }
        }
        else if (SceneName == "Scene7") {
            ToolTipsManager.instance.HideToolTip();
        }
    }

    private void MissionComplete()
    {
        isComplete = FindObjectByTag(transform, 2, "MissionButton");

        if (isComplete) {
            ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("Press"));
            if (Input.GetKey(keyInputItem)) {
                FindObjectOfType<AudioManager>().Play("PressButton", 0f);
                FindObjectOfType<AudioManager>().Play("AutoDoor", 0f);
                missionDoorAnimator.Play("DoorOpen", 0, 0.0f);
                opened = true;
            }

            if (opened) ToolTipsManager.instance.HideToolTip();
        }
    }

    private void RandomEvent()
    {
        time += + 1f * Time.deltaTime;

        if (time >= delayTime) {

            randomEvent = FindObjectByTag(transform, 2, "MissionButton2");

            if (randomEvent) {

                ToolTipsManager.instance.ShowToolTip(Lean.Localization.LeanLocalization.GetTranslationText("Press"));
                if (Input.GetKeyDown(keyInputItem)) {
                    time = 0f;
                    FindObjectOfType<AudioManager>().Play("PressButton", 0f);
                    FindObjectOfType<DrawManager>().startRandom = true;
                    opened = true;
                }

                if (opened) {
                    ToolTipsManager.instance.HideToolTip();
                    opened = !opened;
                }
            }
        }
    }

    public void SwitchGameTips(string currentEvent)
    {
        if (currentEvent == "None")  // switch case will break text to empty
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("Empty");
        else if (currentEvent == "4Room")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("4RoomClear");
        else if (currentEvent == "Prison")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("PrisonClear");
        else if (currentEvent == "Festival")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("FestivalClear");
        else if (currentEvent == "Egypt")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("EgyptClear");
        else if (currentEvent == "Find")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("FindTips");
        else if (currentEvent == "Elevator")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("ElevatorTips");
        else if (currentEvent == "Destroy")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("DestroyTips");
        else if (currentEvent == "Escape")
            tips.text = Lean.Localization.LeanLocalization.GetTranslationText("Escape");
    }

    private void Update()
    {
        if (randomDraw) RandomEvent();
    }

    private void FixedUpdate()
    {
        OpenElevatorDoor();

        if (haveMission) MissionComplete();

        if (canDestroy) {
            DestroyObject();
            isClear = GameIsClear.isClear;
        }
    }

}
