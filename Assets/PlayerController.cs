using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 10;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 4f;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed);
        transform.eulerAngles = Vector3.up * angle;

        transform.Translate(transform.forward * speed * smoothInputMagnitude * Time.deltaTime, Space.World);
    }
}
