using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update

    enum DoorState
    {
        DOOR_STATE_HOME_POSITION = 0,
        DOOR_STATE_MOVE_OPEN,
        DOOR_STATE_SLIDE_OPEN,
        DOOR_STATE_OPENED,
        DOOR_STATE_OPEN_IDLE,
        DOOR_STATE_SLIDE_CLOSE,
        DOOR_STATE_MOVE_CLOSE,
        DOOR_STATE_CLOSE_IDLE,
    }

    [SerializeField] float _Speed = 2;
    private Transform _door;
    private Transform _doorHomePosition;
    private DoorState _doorState;
    private Vector3 _direction;
    private bool _activateDoor;
    private bool _isPlayerNearDoor = false;
    

    private void Awake()
    {
        _doorState = DoorState.DOOR_STATE_HOME_POSITION;
        _activateDoor = false;
        _isPlayerNearDoor = false;
        _door = GetComponent<Transform>();
        _doorHomePosition = _door;
        _direction = new Vector3(0, 0, 0);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isPlayerNearDoor)
            {
                _activateDoor = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (_activateDoor)
        {
            activateDoor();
        }
    }

    private void activateDoor()
    {
        switch(_doorState)
        {
            case DoorState.DOOR_STATE_HOME_POSITION:
                _doorState = DoorState.DOOR_STATE_MOVE_OPEN;
                break;

            case DoorState.DOOR_STATE_MOVE_OPEN:
                _direction.z = 1;
                _direction.x = 0;
                break;

            case DoorState.DOOR_STATE_SLIDE_OPEN:
                _direction.z = 0;
                _direction.x = 1;
                break;

            case DoorState.DOOR_STATE_OPENED:
                _direction.z = 0;
                _direction.x = 0;
                _doorState = DoorState.DOOR_STATE_OPEN_IDLE;
                _activateDoor = false;
                break;

            case DoorState.DOOR_STATE_OPEN_IDLE:
                _doorState = DoorState.DOOR_STATE_SLIDE_CLOSE;
                break;

            case DoorState.DOOR_STATE_SLIDE_CLOSE:
                _direction.z = 0;
                _direction.x = -1;
                break;

            case DoorState.DOOR_STATE_MOVE_CLOSE:
                _direction.z = -1;
                _direction.x = 0;
                break;

            case DoorState.DOOR_STATE_CLOSE_IDLE:
                _direction.z = 0;
                _direction.x = 0;
                _doorState = DoorState.DOOR_STATE_HOME_POSITION;
                _activateDoor = false;
                break;
        }

        if (_doorState == DoorState.DOOR_STATE_HOME_POSITION)
        {
            transform.position = new Vector3(_doorHomePosition.position.x, 0, _doorHomePosition.position.z);
        }
        else
        {
            transform.position += _direction * Time.fixedDeltaTime * _Speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DoorMoveHomePosition"))
        {
            if (_doorState == DoorState.DOOR_STATE_MOVE_CLOSE)
            {
                _doorState = DoorState.DOOR_STATE_CLOSE_IDLE;
            }
        }

        if (other.CompareTag("DoorMoveNotHomePosition"))
        {
            if(_doorState == DoorState.DOOR_STATE_MOVE_OPEN)
            {
                _doorState = DoorState.DOOR_STATE_SLIDE_OPEN;
            }

            if (_doorState == DoorState.DOOR_STATE_SLIDE_CLOSE)
            {

            }
        }

        if (other.CompareTag("DoorSlideStartPosition"))
        {
            if (_doorState == DoorState.DOOR_STATE_SLIDE_CLOSE)
            {
                _doorState = DoorState.DOOR_STATE_MOVE_CLOSE;
            }
        }

        if (other.CompareTag("DoorSlideStopPosition"))
        {
            _doorState = DoorState.DOOR_STATE_OPENED;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isPlayerNearDoor)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerNearDoor = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearDoor = false;
        }
    }
}
