using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject button;
    public Transform[] buttonPos;

    private int randomNum;

    private void Start()
    {
        Spawner();
    }

    void Spawner()
    {
        randomNum = Random.Range(0, buttonPos.Length);

        var LiftButton = Instantiate(button, buttonPos[randomNum].position, buttonPos[randomNum].rotation);
        LiftButton.transform.parent = buttonPos[randomNum].transform;
    }
}
