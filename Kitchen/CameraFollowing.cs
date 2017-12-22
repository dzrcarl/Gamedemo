using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour {

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desierdPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desierdPosition, smoothSpeed);
        transform.position = smoothPosition;
    }
}
