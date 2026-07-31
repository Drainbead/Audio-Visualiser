using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform[] cameraPoints;

    public float moveSpeed = 3f;
    public float rotateSpeed = 3f;

    private int currentPoint = 0;

    void Update()
    {
        if (cameraPoints.Length == 0)
            return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentPoint++;

            if (currentPoint >= cameraPoints.Length)
                currentPoint = 0;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            cameraPoints[currentPoint].position,
            Time.deltaTime * moveSpeed
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            cameraPoints[currentPoint].rotation,
            Time.deltaTime * rotateSpeed
        );
    }
}