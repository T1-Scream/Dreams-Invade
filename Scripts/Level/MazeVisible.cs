using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisible : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    private float time;
    private float delayTime;
    private bool isVisible = true;

    private void OnTriggerStay(Collider other)
    {
        time += + 1f * Time.deltaTime;
        delayTime = Random.Range(3f, 5f);

        if (time > delayTime) {
            if (isVisible)
                Disable();
            else
                Visible();

            time = 0f;
        }
    }

    private void Visible()
    {
        isVisible = true;
        obstacle.SetActive(true);
    }

    private void Disable()
    {
        isVisible = !isVisible;
        obstacle.SetActive(false);
    }
}
