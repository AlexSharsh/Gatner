using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlugBossAI : MonoBehaviour
{
    enum AgentMode
    {
        STATIC_AGENT = 0,
        FOLLOW_AGENT,
        PATROL_AGENT
    }
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Bullet1 _bullet1;
    [SerializeField] private float _healthLevel = 100;
    [SerializeField] private float _damageLevel = 30;
    [SerializeField] private Player _player;
    [SerializeField] private AgentMode _agentMode;
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] TextMesh _textHealth;
    private NavMeshAgent _agent;
    private int PatrolPointsIndex;
    private bool _isPatrol;
    private float _health_100;
    private System.DateTime _datetime = System.DateTime.Now;

    private void Awake()
    {
        _player = FindObjectOfType<Player>(); 
        _agent = GetComponent<NavMeshAgent>();

        _health_100 = _healthLevel;
        OutSlugHealth(_healthLevel);

        _isPatrol = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (_agentMode)
        {
            case AgentMode.STATIC_AGENT:

                break;

            case AgentMode.FOLLOW_AGENT:
                _agent.SetDestination(_player.transform.position);
                break;

            case AgentMode.PATROL_AGENT:
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    PatrolPointsIndex = (PatrolPointsIndex + 1) % PatrolPoints.Length;
                    _agent.SetDestination(PatrolPoints[PatrolPointsIndex].position);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
     
        switch (_agentMode)
        {
            case AgentMode.STATIC_AGENT:
                if ((Vector3.Distance(_agent.transform.position, _player.transform.position)) < 4)
                {
                    _agentMode = AgentMode.FOLLOW_AGENT;
                    _agent.speed = 1.0f;
                }
                break;

            case AgentMode.FOLLOW_AGENT:
                _agent.SetDestination(_player.transform.position);
                break;

            case AgentMode.PATROL_AGENT:
                if (_isPatrol)
                {
                    if (_agent.remainingDistance <= _agent.stoppingDistance)
                    {
                        PatrolPointsIndex = (PatrolPointsIndex + 1) % PatrolPoints.Length;
                        _agent.SetDestination(PatrolPoints[PatrolPointsIndex].position);
                    }

                    if ((Vector3.Distance(_agent.transform.position, _player.transform.position)) < 4)
                    {
                        _agent.SetDestination(_player.transform.position);
                        _agent.speed = 1.0f;
                        _isPatrol = false;
                    }
                }
                else
                {
                    if ((Vector3.Distance(_agent.transform.position, _player.transform.position)) < 4)
                    {
                        _agent.SetDestination(_player.transform.position);
                    }
                    else
                    {
                        _agent.SetDestination(PatrolPoints[PatrolPointsIndex].position);
                        _agent.speed = 0.5f;
                        _isPatrol = true;
                    }
                }
                break;
        }
    }

    void FixedUpdate()
    {
        
    }

    public void SetPatrolPoints(Transform[] points)
    {
        PatrolPoints = points;
    }

    public float GetDamageLevel()
    {
        return _damageLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((System.DateTime.Now - _datetime).Milliseconds >= 100)
        {
            if (other.CompareTag("Bullet"))
            {
                _agentMode = AgentMode.FOLLOW_AGENT;
                _agent.speed = 2.0f;

                if (_healthLevel > 0)
                {
                    _healthLevel -= _bullet.GetBulletDamage();
                    Debug.LogFormat("_healthLevel = {0}", _healthLevel);
                }

                OutSlugHealth(_healthLevel);
                if (_healthLevel <= 0)
                {
                    SlugHealthDisable();
                    Destroy(gameObject);
                }

                _datetime = System.DateTime.Now;
            }

            if (other.CompareTag("Bullet1"))
            {
                _agentMode = AgentMode.FOLLOW_AGENT;
                _agent.speed = 2.0f;

                if (_healthLevel > 0)
                {
                    _healthLevel -= _bullet1.GetBulletDamage();
                }

                OutSlugHealth(_healthLevel);
                if (_healthLevel <= 0)
                {
                    SlugHealthDisable();
                    Destroy(gameObject);
                }

                _datetime = System.DateTime.Now;
            }
        }
    }

    private void OutSlugHealth(float health)
    {
        float ps = health * 100 / _health_100;

        _textHealth.text = $"{string.Format("{0:F0}", ps)}" + "%";
    }

    private void SlugHealthDisable()
    {
        _textHealth.text = "";
    }
}
