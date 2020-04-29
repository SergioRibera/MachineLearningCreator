using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSensor { Box, Sphere, Ray }
public class SensorMLObject : MonoBehaviour
{
    public TypeSensor typeSensor;
    public Vector3 direction;

    public ObstacleML Obstacle { get; private set; }

    private void FixedUpdate()
    {
        switch (typeSensor)
        {
            case TypeSensor.Box:
                break;
            case TypeSensor.Sphere:
                break;
            case TypeSensor.Ray:
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, 999999))
                {
                    if (hit.collider.CompareTag("Obstacle"))
                    {
                        Obstacle = hit.collider.GetComponent<ObstacleML>();
                        Obstacle.distanceToNeural = hit.distance;
                    }
                }
                break;
        }
    }
}