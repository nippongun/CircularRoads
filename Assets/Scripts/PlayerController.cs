using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum DriveMode { All, Front, Rear };
    public Transform gravityCenter;
    public Rigidbody rigidbody;
    public WheelCollider wheelFrontRight;
    public WheelCollider wheelFrontLeft;
    public WheelCollider wheelRearRight;
    public WheelCollider wheelRearLeft;

    public DriveMode dm = DriveMode.Rear;

    public float maxRPM = 2000f;
    public float optimalRPM = 1500f;
    public float torque = 250f;
    public float brakeTorque = 100f;
    public float turnRadius = 25f;

    public float rollThreshold;

    public float RPM
    {
        get
        {
            return wheelRearLeft.rpm;
        }
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = gravityCenter.localPosition;
    }

    private void FixedUpdate()
    {
        float inputTorque = Input.GetAxis("Vertical") * torque;

        if (RPM < optimalRPM)
        {
            inputTorque = Mathf.Lerp(inputTorque / 10f, inputTorque, RPM / optimalRPM);
        }
        else
        {
            inputTorque = Mathf.Lerp(inputTorque, 0, (RPM - optimalRPM) / (maxRPM - optimalRPM));
        }

        RollBar(wheelFrontLeft, wheelFrontRight);
        RollBar(wheelRearLeft, wheelRearRight);

        wheelFrontRight.steerAngle = Input.GetAxis("Horizontal") * turnRadius;
        wheelFrontLeft.steerAngle = Input.GetAxis("Horizontal") * turnRadius;

        wheelFrontRight.motorTorque = dm == DriveMode.Rear ? 0 : inputTorque;
        wheelFrontLeft.motorTorque = dm == DriveMode.Rear ? 0 : inputTorque;
        wheelRearRight.motorTorque = dm == DriveMode.Front ? 0 : inputTorque;
        wheelRearLeft.motorTorque = dm == DriveMode.Front ? 0 : inputTorque;

        if (Input.GetButton("Fire1"))
        {
            wheelFrontRight.brakeTorque = brakeTorque;
            wheelFrontLeft.brakeTorque = brakeTorque;
            wheelRearRight.brakeTorque = brakeTorque;
            wheelRearLeft.brakeTorque = brakeTorque;
        }
        else
        {
            wheelFrontRight.brakeTorque = 0;
            wheelFrontLeft.brakeTorque = 0;
            wheelRearRight.brakeTorque = 0;
            wheelRearLeft.brakeTorque = 0;
        }
    }

    void RollBar(WheelCollider wheelLeft, WheelCollider wheelRight)
    {
        WheelHit hit;
        float travelRight = 1.0f;
        float travelLeft = 1.0f;

        bool groundedRight = wheelRight.GetGroundHit(out hit);
        if (groundedRight)
        {
            travelRight = (-wheelRight.transform.InverseTransformPoint(hit.point).y - wheelRight.radius) / wheelRight.suspensionDistance;
        }
        bool groundedLeft = wheelLeft.GetGroundHit(out hit);
        if (groundedLeft)
        {
            travelLeft = (-wheelLeft.transform.InverseTransformPoint(hit.point).y - wheelLeft.radius) / wheelLeft.suspensionDistance;

        }

        float antiRollForce = (travelLeft - travelRight) * rollThreshold;

        if (groundedRight)
        {
            rigidbody.AddForceAtPosition(wheelRight.transform.up * antiRollForce, wheelRight.transform.position);
        }
        if (groundedLeft)
        {
            rigidbody.AddForceAtPosition(wheelLeft.transform.up * -antiRollForce, wheelLeft.transform.position);
        }
    }
}
