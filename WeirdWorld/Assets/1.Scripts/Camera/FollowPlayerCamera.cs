using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [Header("Camera Follow Player")]
    [SerializeField] private Vector2 MinMaxClampY;
    private Vector2 Angle = new Vector2(90 * Mathf.Deg2Rad, 0);
    private Vector2 NearPlaneSize;

    [SerializeField] private Transform Follow;
    [SerializeField] private float MaxDistance;
    [SerializeField] private Vector2 Sensitivity;//độ nhạy
    [SerializeField] private Camera Camera;


    private void Start()
    {
        CalculateNearPlaneSize();
        UserInputController.Instance.OnCameraAxisChange += FollowPlayer;

    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnCameraAxisChange -= FollowPlayer;

    }
    private void CalculateNearPlaneSize()
    {
        float height = Mathf.Tan(Camera.fieldOfView * Mathf.Deg2Rad / 2) * Camera.nearClipPlane;
        float width = height * Camera.aspect;

        NearPlaneSize = new Vector2(width, height);
    }
    private Vector3[] GetCameraCollisionPoints(Vector3 direction)
    {
        Vector3 position = Follow.position;
        Vector3 center = position + direction * (Camera.nearClipPlane + 0.2f);

        Vector3 right = transform.right * NearPlaneSize.x;
        Vector3 up = transform.up * NearPlaneSize.y;

        return new Vector3[]
        {
            center - right + up,
            center + right + up,
            center - right - up,
            center + right - up
        };
    }
    private void FollowPlayer(float hor, float ver)
    {
        Angle.x += hor * Mathf.Deg2Rad * Sensitivity.x;
        Angle.y += ver * Mathf.Deg2Rad * Sensitivity.y;
        Angle.y = Mathf.Clamp(Angle.y, MinMaxClampY.x * Mathf.Deg2Rad, MinMaxClampY.y * Mathf.Deg2Rad);



        Vector3 direction = new Vector3(
            Mathf.Cos(Angle.x) * Mathf.Cos(Angle.y),
            -Mathf.Sin(Angle.y),
            -Mathf.Sin(Angle.x) * Mathf.Cos(Angle.y));

        RaycastHit hit;
        float distance = MaxDistance;
        Vector3[] points = GetCameraCollisionPoints(direction);

        foreach (Vector3 point in points)
        {
            if (Physics.Raycast(point, direction, out hit, MaxDistance))
            {
                distance = Mathf.Min((hit.point - Follow.position).magnitude * 0.8f, distance);
            }
        }

        transform.position = Follow.position + direction * distance;

        //transform.position = Vector3.Lerp(transform.position, Follow.position + direction * distance, 10* Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(Follow.position - transform.position);
    }
}
