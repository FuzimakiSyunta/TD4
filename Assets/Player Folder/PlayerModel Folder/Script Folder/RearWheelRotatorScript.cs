using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearWheelRotatorScript : MonoBehaviour
{
    public Transform rearWheel;
    public float wheelRadius = 0.35f;

    private float rearWheelAngle = 0f;

    public void Rotate(float speed)
    {
        float circumference = 2f * Mathf.PI * wheelRadius;
        float distance = speed * Time.deltaTime;
        float deltaAngle = (distance / circumference) * 360f;

        rearWheelAngle += deltaAngle;

        if (rearWheel != null)
            rearWheel.localRotation = Quaternion.Euler(rearWheelAngle, 0f, 0f);
    }
}
