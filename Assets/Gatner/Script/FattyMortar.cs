using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FattyMortar : MonoBehaviour
{
    public bool enable = false;
    private Vector3 _direction;
    public float speed = 2f;
    public float speedJump = 2f;
    public float speedRotate = 25f;

    [SerializeField] public Camera MortarCamera;

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
            if (!MortarCamera.enabled)
            {
                MortarCamera.enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                pressedKeyE = true;
            }
        }
        else
        {
            if (MortarCamera.enabled)
            {
                MortarCamera.enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (enable)
        {
            angleX += Input.GetAxis("Mouse X") * speedRotate;
            angleY += Input.GetAxis("Mouse Y") * speedRotate;
            if (angleY <= 0)
                angleY = 0;
            if (angleY >= 45)
                angleY = 45;

            Quaternion rotateX = Quaternion.AngleAxis(angleX, Vector3.up);
            Quaternion rotateY = Quaternion.AngleAxis(angleY * -1, Vector3.right);

            transform.rotation = originRotation * rotateX * rotateY;
        }
    }

    public bool IsNeedChangeView()
    {
        return pressedKeyE;
    }

    public void ResetState()
    {
        pressedKeyE = false;
    }
}
