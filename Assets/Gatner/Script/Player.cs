using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _direction;
    private bool _jump = false;
    private bool _jumpUpDown = false;
    public float speed = 2f;
    public float speedJump = 2f;
    public float speedRotate = 25f;
    private bool _isSprint = false;

    private bool allowFattyCannon = false;
    private bool allowFattyMortar = false;
    private bool allowGatelingGun = false;

    public FattyCannon _fattyCannon;
    public FattyMortar _fattyMortar;
    public GatelingGun _gatelingGun;

    private Camera MainCamera;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GetComponent<Camera>();
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (allowFattyCannon)
            {
                MainCamera.enabled = !MainCamera.enabled;
                _fattyCannon.enable = !_fattyCannon.enable;
            }

            if (allowFattyMortar)
            {
                MainCamera.enabled = !MainCamera.enabled;
                _fattyMortar.enable = !_fattyMortar.enable;
            }

            if (allowGatelingGun)
            {
                MainCamera.enabled = !MainCamera.enabled;
                _gatelingGun.enable = !_gatelingGun.enable;
            }
        }

        if (MainCamera.enabled)
        {
            _direction.x = Input.GetAxis("Horizontal");
            _direction.z = Input.GetAxis("Vertical");


            if (_jump == false)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    _jump = true;
                }
            }
        }
        else
        {
            if (_fattyCannon.IsNeedChangeView())
            {
                _fattyCannon.ResetState();
                MainCamera.enabled = !MainCamera.enabled;
            }

            if (_fattyMortar.IsNeedChangeView())
            {
                _fattyMortar.ResetState();
                MainCamera.enabled = !MainCamera.enabled;
            }

            if (_gatelingGun.IsNeedChangeView())
            {
                _gatelingGun.ResetState();
                MainCamera.enabled = !MainCamera.enabled;
            }
        }
    }

    void FixedUpdate()
    {
        if (MainCamera.enabled)
        {
            Move(Time.deltaTime);
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * speedRotate /** Time.fixedDeltaTime*/, 0));

            Jump();
        }
    }
    
    private void Move(float delta)
    {
        //transform.position += _direction * speed * delta;
        var fixedDirection = transform.TransformDirection(_direction.normalized);
        transform.position += fixedDirection * (_jump ? speed * 3 : speed) * delta;
    }

    private void Jump()
    {
        if(_jump == true)
        {
            
            if (_jumpUpDown == false)
            {
                if (transform.position.y < 1)
                {
                    _direction.y += 1;
                }
                else
                {
                    _jumpUpDown = true;
                }       
            }
            else
            {
                if (transform.position.y > 0)
                {
                    _direction.y -= 1;
                }
                else
                {
                    _direction.y = 0;
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                    _jumpUpDown = false;
                    _jump = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FattyCannon"))
        {
            allowFattyCannon = true;
        }

        if (other.CompareTag("FattyMortar"))
        {
            allowFattyMortar = true;
        }

        if (other.CompareTag("GatelingGun"))
        {
            allowGatelingGun = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FattyCannon"))
        {
            allowFattyCannon = false;
        }

        if (other.CompareTag("FattyMortar"))
        {
            allowFattyMortar = false;
        }

        if (other.CompareTag("GatelingGun"))
        {
            allowGatelingGun = false;
        }
    }
}
