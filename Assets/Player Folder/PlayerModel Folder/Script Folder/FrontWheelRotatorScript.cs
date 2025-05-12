using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWheelRotatorScript : MonoBehaviour
{
    public Transform frontWheel;
    public float wheelRadius = 0.35f;

    private float frontWheelAngle = 0f;

    public void Rotate(float speed)
    {
        float circumference = 2f * Mathf.PI * wheelRadius;
        float distance = speed * Time.deltaTime;
        float deltaAngle = (distance / circumference) * 360f;

        frontWheelAngle += deltaAngle;

        if (frontWheel != null)
            frontWheel.localRotation = Quaternion.Euler(frontWheelAngle, 0f, 0f);
    }
}
