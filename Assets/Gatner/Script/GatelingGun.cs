using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatelingGun : MonoBehaviour
{
    public bool enable = false;
    private Vector3 _direction;
    public float speed = 2f;
    public float speedJump = 2f;
    public float speedRotate = 25f;

    [SerializeField] public Camera GatelingGunCamera;

    private bool pressedKeyE = false;
    Quaternion originRotation;
    float angleX;
    float angleY;

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (!GatelingGunCamera.enabled)
            {
                GatelingGunCamera.enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                pressedKeyE = true;
            }
        }
        else
        {
            if (GatelingGunCamera.enabled)
            {
                GatelingGunCamera.enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (enable)
        {
            angleX += Input.GetAxis("Mouse X") * speedRotate;
            Quaternion rotateX = Quaternion.AngleAxis(angleX, Vector3.up);

            transform.rotation = originRotation * rotateX;
        }
    }

    public bool IsNeedChangeView()
    {
        return pressedKeyE;
    }

    public void ResetState()
    {
        enable = false;
        pressedKeyE = false;
    }
}
