using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _direction;
    private bool _jump = false;
    private bool _jumpUpDown = false;
    public float speed = 2f;
    public float speedJump = 10f;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _direction.z = -1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _direction.z = 1;
        }
        else
        {
            _direction.z = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _direction.x = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _direction.x = -1;
        }
        else
        {
            _direction.x = 0;
        }

        if (_jump == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _jump = true;
            }
        }
    }

    void FixedUpdate()
    {
        Move(Time.deltaTime);
        Jump();
    }
    
    private void Move(float delta)
    {
        transform.position += _direction * speed * delta;
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
}
