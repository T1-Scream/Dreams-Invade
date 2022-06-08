using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGreenManager : MonoBehaviour
{
    public static RedGreenManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private Material lightMaterial;
    [SerializeField] private float stationaryTolerance = 0.005f;

    [Header("Tutorial")]
    [SerializeField] private Transform teleportPoint;
    public bool tutorialMode;

    private PlayerMovement movement;
    private Rigidbody rb;
    private float time;
    private float time2;
    private float delayTime;
    private bool canMove = true;
    private bool dead = false;

    public bool IsStationary
    {
        get { return rb.velocity.sqrMagnitude < stationaryTolerance * stationaryTolerance; }
    }

    private void OnTriggerEnter(Collider other)
    {
        movement.Speed = 1f;
    }

    private void OnTriggerStay(Collider other)
    {
        time += + 1f * Time.deltaTime;
        delayTime = Random.Range(3f, 5f);

        if (!canMove) // if red light then start count
            time2 += + 1f * Time.deltaTime;
        else
            time2 = 0f;

        if (time >= delayTime) {
            if (canMove)
                RedLight();
            else
                GreenLight();

            time = 0f;
        }

        if (!canMove && !IsStationary && !dead && time2 >= 0.5f) // if gameover then setActive only once time
            GameOver();
    }

    private void OnTriggerExit(Collider other)
    {
        movement.Speed = 2f;
        movement.canCrouch = true;
    }

    private void Awake()
    {
        movement = player.GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        lightMaterial.color = Color.green;
    }

    private void RedLight()
    {
        FindObjectOfType<AudioManager>().Play("Scan", 0f);
        lightMaterial.color = Color.red;
        canMove = !canMove;
    }

    private void GreenLight()
    {
        lightMaterial.color = Color.green;
        canMove = true;
    }

    private void GameOver()
    {
        if (tutorialMode) {
            player.transform.position = teleportPoint.position;
            GreenLight();
        }
        else {
            dead = true;
            FindObjectOfType<GameManager>().GameOver(dead);
        }
    }
}
