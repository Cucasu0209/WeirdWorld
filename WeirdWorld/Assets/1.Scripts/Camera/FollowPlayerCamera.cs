using DG.Tweening;
using UnityEngine;


public class FollowPlayerCamera : MonoBehaviour
{
    [Header("Camera Follow Player")]
    [SerializeField] private Vector2 MinMaxClampY;
    private Vector2 Angle = new Vector2(90 * Mathf.Deg2Rad, 0);
    private Vector2 NearPlaneSize;

    [SerializeField] private Transform TransformFollow;
    [SerializeField] private Transform Player;

    private float MaxDistance;
    private float TargetMaxDistance;
    [SerializeField] private float NormalMaxDistance;
    [SerializeField] private float AimingMaxDistance;

    private Vector2 Sensitivity;//độ nhạy
    [SerializeField] private Vector2 NormalSensitivity;
    [SerializeField] private Vector2 AimingSensitivity;
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform CenterUI;
    [SerializeField] private Transform CamEffect;


    private bool IsAiming = false;

    private void Start()
    {
        MaxDistance = NormalMaxDistance;
        TargetMaxDistance = NormalMaxDistance;
        Sensitivity = NormalSensitivity;
        CalculateNearPlaneSize();
        UserInputController.Instance.OnCameraAxisChange += FollowPlayer;
        UserInputController.Instance.OnBtnShootDown += StartAiming;
        UserInputController.Instance.OnBtnShootUp += CancelAiming;

    }
    private void Update()
    {
        MaxDistance = Mathf.Lerp(MaxDistance, TargetMaxDistance, 10 * Time.deltaTime);
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnCameraAxisChange -= FollowPlayer;
        UserInputController.Instance.OnBtnShootDown -= StartAiming;
        UserInputController.Instance.OnBtnShootUp -= CancelAiming;

    }
    private void CalculateNearPlaneSize()
    {
        float height = Mathf.Tan(Camera.fieldOfView * Mathf.Deg2Rad / 2) * Camera.nearClipPlane;
        float width = height * Camera.aspect;

        NearPlaneSize = new Vector2(width, height);
    }
    private Vector3[] GetCameraCollisionPoints(Vector3 direction)
    {
        Vector3 position = TransformFollow.position;
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
                distance = Mathf.Min((hit.point - TransformFollow.position).magnitude * 0.8f, distance);
            }
        }

        transform.position = TransformFollow.position + direction * distance;

        //transform.position = Vector3.Lerp(transform.position, Follow.position + direction * distance, 10* Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(TransformFollow.position - transform.position);
        if (IsAiming)
        {
            Player.rotation = transform.rotation;
        }
    }

    public void StartAiming()
    {
        TargetMaxDistance = AimingMaxDistance;
        TransformFollow.DOLocalMoveX(0.4f, 0.2f);
         CamEffect.gameObject.SetActive(true);

        CenterUI.DOScale(1, 0.2f);
        Sensitivity = AimingSensitivity;
        IsAiming = true;
    }
    public void CancelAiming()
    {
        TargetMaxDistance = NormalMaxDistance;
        TransformFollow.DOLocalMoveX(0, 0.2f);
        CamEffect.gameObject.SetActive(false);

        CenterUI.DOScale(0, 0.2f);
        Sensitivity = NormalSensitivity;
        IsAiming = false;
    }
}
