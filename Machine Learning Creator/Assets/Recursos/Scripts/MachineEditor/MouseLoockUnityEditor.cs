using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseLoockUnityEditor : MonoBehaviour
{
    
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;
    
    public float velocityMove = 2f;
    public bool smooth = false;
    public float smoothnes = 3f;

    float rotationX = 0F;
    float rotationY = 0F;
    
    Quaternion originalRotation;
    Vector3 pos;

    // Chunk System
    [Range(1, 100)] public int chunkSize = 100;
    [Range(1, 100)] public int viewSize = 4;

    public Chunk chunkPrefab;
    public List<Chunk> chunks = new List<Chunk>();

    void Start()
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
        originalRotation = transform.localRotation;

        chunks.Add(Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity).SetDir(DirChunk.center));
    }

    void LateUpdate()
    {
        if (Input.GetButton("Fire2"))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            if (smooth)
                transform.localRotation = Quaternion.Lerp(transform.rotation, originalRotation * xQuaternion * yQuaternion, Time.deltaTime * smoothnes);
            else
                transform.localRotation = originalRotation * xQuaternion * yQuaternion;

            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * velocityMove * Time.deltaTime);

            if (Input.GetKey(KeyCode.S))
                transform.Translate(-Vector3.forward * velocityMove * Time.deltaTime);

            if (Input.GetKey(KeyCode.A))
                transform.Translate(-Vector3.right * velocityMove * Time.deltaTime);

            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.right * velocityMove * Time.deltaTime);

            if (Input.GetKey(KeyCode.E))
                transform.Translate(Vector3.up * velocityMove * Time.deltaTime);

            if (Input.GetKey(KeyCode.Q))
                transform.Translate(-Vector3.up * velocityMove * Time.deltaTime);
        }
        chunks[0].transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

}