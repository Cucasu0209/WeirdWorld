using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    #region Camera
    [SerializeField] float mouseSense = 0.6f;
    public float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [SerializeField] Transform camFollowRoot;
    #endregion

    #region State   
    public Animator animator;
    public Camera VirtualCamera;
    public float adsFov = 22.64f;
    public float hipFov = 65.25f;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;
    #endregion

    [SerializeField] Transform aimPos;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;
    void Start()
    {
        UserInputController.Instance.OnCameraAxisChange += OnCameraAxisChange;
    }
    private void OnDestroy()
    {
        UserInputController.Instance.OnCameraAxisChange -= OnCameraAxisChange;
    }
    private void Update()
    {
        //VirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(VirtualCamera.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        Vector2 screeenCentre = new Vector2(Screen.width, Screen.height) / 2;
        Ray ray = Camera.main.ScreenPointToRay(screeenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        }
    }
  

    private void OnCameraAxisChange(float TouchDistX, float TouchDistY)
    {
        Debug.Log("asdasdas");
        xAxis = (xAxis + TouchDistX * mouseSense) % 360;
        yAxis = Mathf.Clamp(yAxis + TouchDistY * mouseSense, -30, 20);
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        camFollowRoot.eulerAngles = new Vector3(camFollowRoot.eulerAngles.x, xAxis, camFollowRoot.localEulerAngles.z);
    }
}
