using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;
    public float followSpeed = 3;
    public float lookSpeed = 10;

    //Handles the direction the camera has to look
    public void LookAtTarget()
    {
        //Direction is set automatically
        Vector3 direction = follow.position - transform.position;
        Quaternion quaternion = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, quaternion ,lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 targetPosition = follow.position + follow.forward * offset.z + follow.right * offset.x + follow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

    }
    private void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }
}
