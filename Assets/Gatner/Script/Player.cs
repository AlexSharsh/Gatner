using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _healthLevel = 100f;
    [SerializeField] SlugAI _slugAI;
    [SerializeField] SlugBossAI _slugBossAI;
    [SerializeField] TextMesh _textHealth;

    private Vector3 _direction;
    private bool _jump = false;
    private bool _jumpUpDown = false;
    public float speed = 2f;
    public float speedJump = 2f;
    public float speedRotate = 15f;
    private bool _isSprint = false;
    private bool _isGameOver = false;

    private bool allowFattyCannon = false;
    private bool allowFattyMortar = false;
    private bool allowGatelingGun = false;

    public FattyCannon _fattyCannon;
    public FattyMortar _fattyMortar;
    public GatelingGun _gatelingGun;

    private Camera MainCamera;
    [SerializeField] private Camera DeadCamera;

    private Rigidbody _rigidBody;
    [SerializeField] private Animator _anim;

    private float _health_100;

    private System.DateTime _datetime = System.DateTime.Now;

    private void Awake()
    {
        DeadCamera.enabled = false;
        MainCamera = GetComponent<Camera>();
        MainCamera = Camera.main;

        _anim = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();

        _health_100 = _healthLevel;
        OutPlayerHealth(_healthLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver)
        {
            if (MainCamera.enabled)
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

                _direction.x = Input.GetAxis("Horizontal");
                _direction.z = Input.GetAxis("Vertical");

                _anim.SetBool("IsWalking", _direction != Vector3.zero);

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
        else
        {
            DeadCamera.enabled = true;
            MainCamera.enabled = false;
            _fattyCannon.enable = false;
            _fattyMortar.enable = false;
            _gatelingGun.enable = false;
        }
    }

    void FixedUpdate()
    {
        if (!_isGameOver)
        {
            if (MainCamera.enabled)
            {
                Move(Time.deltaTime);
                transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * speedRotate /** Time.fixedDeltaTime*/, 0));

                Jump();
            }
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
        if (!_isGameOver)
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isGameOver)
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

    private void OnTriggerStay(Collider other)
    {
        if (!_isGameOver)
        {
            if ((System.DateTime.Now - _datetime).Milliseconds >= 500)
            {
                if (other.CompareTag("Slug"))
                {
                    if (_healthLevel > 0)
                    {
                        _healthLevel -= _slugAI.GetDamageLevel();
                        Debug.LogFormat("PlayerHealth = {0}", _healthLevel);
                    }

                    OutPlayerHealth(_healthLevel);
                    if (_healthLevel <= 0)
                    {
                        _healthLevel = 0;
                        OutPlayerHealth(_healthLevel);
                        GameOver();
                    }

                    _datetime = System.DateTime.Now;
                }

                if (other.CompareTag("SlugBoss"))
                {
                    if (_healthLevel > 0)
                    {
                        _healthLevel -= _slugBossAI.GetDamageLevel();
                        Debug.LogFormat("PlayerHealth = {0}", _healthLevel);
                    }

                    OutPlayerHealth(_healthLevel);
                    if (_healthLevel <= 0)
                    {
                        _healthLevel = 0;
                        OutPlayerHealth(_healthLevel);
                        GameOver();
                    }

                    _datetime = System.DateTime.Now;
                }
            }
        }
    }

    private void GameOver()
    {
        if(!_isGameOver)
        {
            _isGameOver = true;

            _anim.SetBool("IsWalking", false);
            _rigidBody.isKinematic = true;

            Quaternion rotateX = Quaternion.AngleAxis(90, Vector3.left);

            transform.rotation = transform.rotation * rotateX;

            PlayerHealthDisable();
            Debug.LogFormat("ÈÃÐÀ ÎÊÎÍ×ÅÍÀ: ÂÛ ÏÐÎÈÃÐÀËÈ :(");
        }
    }

    private void OutPlayerHealth (float health)
    {
        float ps = health * 100 / _health_100;
        _textHealth.text = $"Health: {string.Format("{0:F0}", ps)}" + "%";
    }

    private void PlayerHealthDisable()
    {
        _textHealth.text = "";
    }
}
