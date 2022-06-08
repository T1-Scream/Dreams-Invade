using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    private Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = FindObjectOfType<PlayerMovement>().transform;
            player.position = playerTrans.position;
        }
    }
}
