using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ToolTipsManager : MonoBehaviour
{
    public static ToolTipsManager instance;

    [SerializeField] private TextMeshProUGUI tooltipText = null;

    [HideInInspector] public float showTextTime = 0f;
    [HideInInspector] public float DisableTextDelay = 5f;

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
        tooltipText.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ShowToolTip(string tip)
    {
        tooltipText.gameObject.transform.parent.gameObject.SetActive(true);
        tooltipText.text = tip;
    }

    public void HideToolTip()
    {
        tooltipText.gameObject.transform.parent.gameObject.SetActive(false);
        tooltipText.text = string.Empty;
    }
}
