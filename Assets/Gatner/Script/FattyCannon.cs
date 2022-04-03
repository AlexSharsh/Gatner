using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FattyCannon : MonoBehaviour
{
    public bool enable = false;
    private Vector3 _direction;
    public float speed = 2f;
    public float speedJump = 2f;
    public float speedRotate = 25f;

    [SerializeField] public Camera CannonCamera;
    [SerializeField] public Bullet _bullet;
    [SerializeField] public Pellet _pellet;
    [SerializeField] TextMesh _textHealth;
    [SerializeField] public int Shots = 10;

    private System.DateTime _datetime = System.DateTime.Now;

    private bool pressedKeyE = false;
    Quaternion originRotation;
    float angleX;
    float angleY;

    private void Awake()
    {
        //_pellet = FindObjectOfType<Pellet>();
    }

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.rotation;
        ShotsDisable();
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (!CannonCamera.enabled)
            {
                CannonCamera.enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                pressedKeyE = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }

            OutShots(Shots);
        }
        else
        {
            if (CannonCamera.enabled)
            {
                CannonCamera.enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (enable)
        {
            angleX += Input.GetAxis("Mouse X") * speedRotate;
            angleY += Input.GetAxis("Mouse Y") * speedRotate;
            if (angleY <= -5)
                angleY = -5;
            if (angleY >= 45)
                angleY = 45;

            Quaternion rotateX= Quaternion.AngleAxis(angleX, Vector3.up);
            Quaternion rotateY = Quaternion.AngleAxis(angleY * -1, Vector3.right);

            transform.rotation = originRotation * rotateX * rotateY;
        }
    }

    private void Fire()
    {
        if (Shots > 0)
        {
            var shieldObj = Instantiate(_bullet, _bullet.transform.position, _bullet.transform.rotation);
            var shield = shieldObj.GetComponent<Bullet>();
            shield.Init(/*_player.transform,*/ 1, 20f);

            Shots--;
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
        ShotsDisable();
    }

    private void OutShots(int CountShots)
    {
        _textHealth.text = $"Shots: {string.Format("{0:F0}", CountShots)}";
    }

    private void ShotsDisable()
    {
        _textHealth.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((System.DateTime.Now - _datetime).Milliseconds >= 100)
        {
            if (other.CompareTag("Pellet"))
            {
                Pellet pellet = FindObjectOfType<Pellet>();
                Shots += pellet.GetShots();

                _datetime = System.DateTime.Now;
            }
        }
    }
}
