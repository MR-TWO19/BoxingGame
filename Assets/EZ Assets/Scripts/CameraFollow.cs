using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new(0, 5, -10);
    public float smoothSpeed = 5f;
    public float lookDownAngle = 30f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(lookDownAngle, 0, 0);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
