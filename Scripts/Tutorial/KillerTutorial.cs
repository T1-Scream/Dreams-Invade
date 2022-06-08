using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerTutorial : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotateSpeed;
    private void LookPlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position
    - transform.position), rotateSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void FixedUpdate()
    {
        LookPlayer();
    }
}
