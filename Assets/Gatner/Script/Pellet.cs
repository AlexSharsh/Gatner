using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    private bool _isTake;
    private Vector3 _transformOrigin;
    [SerializeField] Player _player;
    [SerializeField] int Shots = 10;
    private Vector3 _offset;
    private bool _isPlayerActive;
    private Transform _originPlayer;
    private float angleX;
    //private float angleY;
    //private float angleZ;
    private float _distance;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _isTake = false;
        _transformOrigin.x = transform.position.x;
        _transformOrigin.y = transform.position.y;
        _transformOrigin.z = transform.position.z;

        _rigidbody = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>();

        _isPlayerActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isPlayerActive)
            {
                if (!_isTake)
                {
                    transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                    angleX = _player.transform.rotation.eulerAngles.y;
                    //_offset = transform.position - _player.transform.position;
                    //_rigidbody.isKinematic = true;
                    //transform.Rotate(0, 0, transform.rotation.eulerAngles.z + 180 - _player.transform.rotation.eulerAngles.y);
                    _distance = Vector3.Distance(transform.position, _player.transform.position) - 0.5f;
                    Debug.LogFormat("transform.rotation.eulerAngles.z = {0}", transform.rotation.eulerAngles.z);
                    _isTake = true;
                }
                else
                {
                    _isTake = false;
                    transform.position = new Vector3(transform.position.x, _transformOrigin.y, transform.position.z);
                    //_rigidbody.isKinematic = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isTake)
        {
            angleX = _player.transform.rotation.eulerAngles.y - angleX;
            transform.rotation *= Quaternion.Euler(0, 0, angleX);
            transform.position = transform.rotation * new Vector3(0, _distance, 1) + _player.transform.position;
            angleX = _player.transform.rotation.eulerAngles.y;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerActive = true;
        }

        if (other.CompareTag("FattyCannon"))
        {
            _isTake = false;
            Destroy(gameObject, 1);
        }  

        if (other.CompareTag("FattyMortar"))
        {
            _isTake = false;
            Destroy(gameObject, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerActive = false;
        }
    }

    public bool IsTake()
    {
        return _isTake;
    } 

    public int GetShots()
    {
        return Shots;
    }
}
