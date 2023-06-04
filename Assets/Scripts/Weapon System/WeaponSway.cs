using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSway : MonoBehaviour
{
    public float intensity;
    public float smooth;

    private Quaternion originRotation;

    MouseControl mouseControl;

    private void Start()
    {
        originRotation = transform.localRotation;
        EventsDispatcher.Instance.onMouseControlChanged += (mCtrl) => { mouseControl = mCtrl; };
    }

    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        if (mouseControl != MouseControl.Shooting)
            return;

        //controls
        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");

        //calculate target rotation
        Quaternion t_x_adj = Quaternion.AngleAxis(-intensity * t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(intensity * t_y_mouse, Vector3.right);
        Quaternion t_z_adj = Quaternion.AngleAxis(intensity * t_x_mouse, Vector3.forward);
        Quaternion targetRotation = originRotation * t_x_adj * t_y_adj * t_z_adj;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }
}