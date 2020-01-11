using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public WheelCollider wheel;
    void FixedUpdate()
    {
        //	transform.position = wheelCollider.su

        UpdateWheel(transform, wheel);
    }
    void UpdateWheel(Transform wheel, WheelCollider collider)
    {
        Vector3 position = wheel.localPosition;

        WheelHit hit = new WheelHit();

        if (collider.GetGroundHit(out hit))
        {
            float hitY = collider.transform.InverseTransformPoint(hit.point).y;

            position.y = hitY + collider.radius;
        } else
        {
            position = Vector3.Lerp(position, -Vector3.up * collider.suspensionDistance, 0.05f);
        }


        //wheel.localPosition = position;

        Quaternion quaternion = Quaternion.Euler(0, collider.steerAngle, 90);
        wheel.localRotation = quaternion;
        collider.GetWorldPose(out position, out quaternion);
    }
    
}
