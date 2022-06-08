using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using QuantumTek.EncryptedSave;

public class KeyMapButton : MonoBehaviour, IPointerDownHandler
{
    public static KeyMapButton instance;

    public KeyCode keyInput = KeyCode.None;
    public TMP_Text keyText;
    public string loadKey;

    [HideInInspector] public bool keyMapping = false;
    Event keyEvent;

    private void Awake()
    {
        keyText = gameObject.GetComponentInChildren<TMP_Text>();

        if (ES_Save.Exists(loadKey)) {
            keyInput = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>(loadKey));
            keyText.text = keyInput.ToString();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        keyMapping = true;
    }

    private void OnGUI()
    {
        keyEvent = Event.current;
        if (keyEvent.isKey && keyMapping) {
            keyInput = keyEvent.keyCode;

            if (keyInput == KeyCode.None) return;

            keyText.text = keyInput.ToString();
            keyMapping = false;
        }
    }
}
