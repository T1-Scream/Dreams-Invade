using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnywhereDoor : MonoBehaviour
{
    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;
    Vector3 offset;

    private void Update()
    {
        offset = playerCamera.position - otherPortal.position;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.right;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
