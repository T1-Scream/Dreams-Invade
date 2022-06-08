using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleVideoIsClear : MonoBehaviour
{
    public static PuzzleVideoIsClear instance;

    [SerializeField] private Material lightMaterial;

    public bool clear = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            lightMaterial.color = Color.green;
            clear = true;
        }
    }

    private void Awake()
    {
        clear = false;
        lightMaterial.color = Color.red;
    }
}
