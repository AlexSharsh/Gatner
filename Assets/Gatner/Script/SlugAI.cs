using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlugAI : MonoBehaviour
{
    enum AgentMode
    {
        STATIC_AGENT = 0,
        FOLLOW_AGENT,
        PATROL_AGENT
    }

    [SerializeField] private Player _player;
    [SerializeField] private AgentMode _agentMode;
    [SerializeField] private Transform[] PatrolPoints;
    private NavMeshAgent _agent;
    private int PatrolPointsIndex;
    private bool _isPatrol;

    private void Awake()
    {
        _player = FindObjectOfType<Player>(); 
        _agent = GetComponent<NavMeshAgent>();

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
}
