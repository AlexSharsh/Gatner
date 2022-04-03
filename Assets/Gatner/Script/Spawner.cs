using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefabs;
    [SerializeField] private GameObject _enemyBossPrefabs;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private SlugAI _slugAI;
    [SerializeField] private SlugBossAI _slugBossAI;

    [SerializeField] private float _timeCuldown;

    [SerializeField] private bool[] _fillPoints;
    [SerializeField] private int _counterPoint = 0;
    [SerializeField] private int _countDie;
    [SerializeField] private List<GameObject> _enemys;
    [SerializeField] private float _timer;

    private bool _isWin = false;

    private void Awake()
    {
        _isWin = false;
        _enemys = new List<GameObject>();
        _fillPoints = new bool[_spawnPoints.Length];
        _slugAI.SetPatrolPoints(_patrolPoints);
        _slugBossAI.SetPatrolPoints(_patrolPoints);
    }

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && _countDie < _spawnPoints.Length)
        {
            //Destroy(_enemys[_counterPoint]);
            _fillPoints[_counterPoint] = false;
            _counterPoint++;
            _countDie++;
            if (_countDie > _spawnPoints.Length)
                _countDie = _spawnPoints.Length;
            _counterPoint %= _spawnPoints.Length;
            //4 / 4 = 1 | 0
            //5 / 4 = 1 | 1
            //6 / 4 = 1 | 2
            //7 / 4 = 1 | 3
            _timer = 0;
        }

        if (_timer < _timeCuldown)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            if (_countDie > 0)
                Spawn(_countDie);
        }

        CheckWin();
    }

    private void CheckWin()
    {
        for(int i = 0; i < _enemys.Count - 1; i++)
        {
            if (_enemys[i] != null)
            {
                return;
            }
        }

        _isWin = true;
        Debug.LogFormat("ÈÃÐÀ ÎÊÎÍ×ÅÍÀ: ÂÛ ÏÎÁÅÄÈËÈ!!!");
    }

    public bool IsWin()
    {
        return _isWin;
    }

    private void Spawn()
    {
        Random enemyId = new Random();
        int idx;

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            //idx = enemyId.Next(0, _enemyPrefabs.Length - 1);
            if (i == 0)
            {
                _enemys.Add(Instantiate(_enemyBossPrefabs, _spawnPoints[i].position, Quaternion.identity));
            }
            else
            {
                _enemys.Add(Instantiate(_enemyPrefabs, _spawnPoints[i].position, Quaternion.identity));
            }
            
            _fillPoints[i] = true;
        }

        _countDie = 0;
    }
    
    private void Spawn(int count)
    {
        //for (int i = 0; i < count; i++)
        //{
        //    //var freePoint = _fillPoints.FirstOrDefault(point => !point);
        //    for (var j = 0; j < _fillPoints.Length; j++)
        //    {
        //        if (!_fillPoints[j])
        //        {
        //            _enemys[j] = Instantiate(_enemyPrefab, _spawnPoints[j].position, Quaternion.identity);
        //            _fillPoints[j] = true;
        //            break;
        //        }
        //    }
        //}
        //_countDie = 0;
    }
}
