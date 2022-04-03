using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _healthLevel = 100f;
    [SerializeField] SlugAI _slugAI;
    [SerializeField] SlugBossAI _slugBossAI;
    [SerializeField] Spawner _spawner;
    [SerializeField] TextMesh _textHealth;
    [SerializeField] TextMesh _textPlayOff;
    [SerializeField] TextMesh _textGameOver;

    private Vector3 _direction;
    private bool _jump = false;
    private bool _jumpUpDown = false;
    public float speed = 2f;
    public float speedJump = 2f;
    public float speedRotate = 15f;
    private bool _isGameOver = false;

    private bool _isMainView;
    private bool allowFattyCannon = false;
    private bool allowFattyMortar = false;
    private bool allowGatelingGun = false;

    [SerializeField] private List<GameObject> _bombArsenal;
    [SerializeField] private List<GameObject> _pelletArsenal;

    public FattyCannon _fattyCannon;
    public FattyMortar _fattyMortar;
    public GatelingGun _gatelingGun;
    //public Bomb _bomb;

    private Camera MainCamera;
    [SerializeField] private Camera DeadCamera;
    [SerializeField] private Camera PersonCamera;

    private Rigidbody _rigidBody;
    [SerializeField] private Animator _anim;

    private float _health_100;

    private System.DateTime _datetime = System.DateTime.Now;

    private void Awake()
    {
        DeadCamera.enabled = false;
        PersonCamera.enabled = false;

        _isMainView = true;

        MainCamera = GetComponent<Camera>();
        MainCamera = Camera.main;

        _anim = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _spawner = FindObjectOfType<Spawner>();

        _health_100 = _healthLevel;
        OutPlayerHealth(_healthLevel);
        GameOffTextDisable();
        GameOverTextDisable();

        //_bomb = GetComponent<Bomb>();
        //_bomb = GetComponent<Bomb>();
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
            if (_isMainView)
            {
                if (IsBombTake() || IsPelletTake())
                {
                    MainCamera.enabled = false;
                    PersonCamera.enabled = true;
                }
                else
                {
                    if (PersonCamera.enabled)
                    {
                        MainCamera.enabled = true;
                        PersonCamera.enabled = false;
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (allowFattyCannon)
                        {
                            MainCamera.enabled = false;
                            _fattyCannon.enable = true;
                            _isMainView = false;
                        }

                        if (allowFattyMortar)
                        {
                            MainCamera.enabled = false;
                            _fattyMortar.enable = true;
                            _isMainView = false;
                        }

                        if (allowGatelingGun)
                        {
                            MainCamera.enabled = false;
                            _gatelingGun.enable = true;
                            _isMainView = false;
                        }
                    }
                }
                

                //if (Input.GetKeyDown(KeyCode.V))
                //{
                //    if (!allowFattyCannon && !allowFattyMortar && !allowGatelingGun)
                //    {
                //        if (!PersonCamera.enabled)
                //        {
                //            MainCamera.enabled = false;
                //            PersonCamera.enabled = true;
                //        }
                //        else
                //        {
                //            MainCamera.enabled = true;
                //            PersonCamera.enabled = false;
                //        }
                //    }
                //}

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
                    MainCamera.enabled = true;
                    _isMainView = true;
                }

                if (_fattyMortar.IsNeedChangeView())
                {
                    _fattyMortar.ResetState();
                    MainCamera.enabled = true;
                    _isMainView = true;
                }

                if (_gatelingGun.IsNeedChangeView())
                {
                    _gatelingGun.ResetState();
                    MainCamera.enabled = true;
                    _isMainView = true;
                }
            }

            if (_spawner.IsWin())
            {
                GameWinText();
            }
        }
        else
        {
            _isMainView = false;
            DeadCamera.enabled = true;
            MainCamera.enabled = false;
            PersonCamera.enabled = false;
            _fattyCannon.enable = false;
            _fattyMortar.enable = false;
            _gatelingGun.enable = false;
        }
    }

    void FixedUpdate()
    {
        if (!_isGameOver)
        {
            if (_isMainView)
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

    public void SetBombArsenal(List<GameObject> _bombList)
    {
        _bombArsenal = _bombList;
    }

    public void SetPelletArsenal(List<GameObject> _pelletList)
    {
        _pelletArsenal = _pelletList;
    }

    private bool IsBombTake()
    {
        for(int i = 0; i < _bombArsenal.Count; i++)
        {
            if (_bombArsenal[i])
            {
                if (_bombArsenal[i].GetComponent<Bomb>().IsTake())
                    return true;
            }
        }

        return false;
    }

    private bool IsPelletTake()
    {
        for (int i = 0; i < _pelletArsenal.Count; i++)
        {
            if (_pelletArsenal[i])
            {
                if (_pelletArsenal[i].GetComponent<Pellet>().IsTake())
                    return true;
            }
        }

        return false;
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
            GameOverText();
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

    private void GameWinText()
    {
        _textPlayOff.text = $"ÂÛ ÏÎÁÅÄÈËÈ!";
    }

    private void GameOverText()
    {
        _textGameOver.text = $"ÂÛ ÏÐÎÈÃÐÀËÈ!";
    }

    private void GameOffTextDisable()
    {
        _textPlayOff.text = "";
    }

    private void GameOverTextDisable()
    {
        _textGameOver.text = "";
    }
}
