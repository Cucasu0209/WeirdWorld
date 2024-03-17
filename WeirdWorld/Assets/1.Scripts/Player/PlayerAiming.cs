using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{

    [SerializeField] Transform aimPos;
    [SerializeField] LayerMask aimMask;
    void Start()
    {
    }
    private void OnDestroy()
    {
    }
    private void Update()
    {
        //VirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(VirtualCamera.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        Vector2 screeenCentre = new Vector2(Screen.width, Screen.height) / 2;
        Ray ray = Camera.main.ScreenPointToRay(screeenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = hit.point;
        }
    }

}
