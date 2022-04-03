using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PelletArsenal : MonoBehaviour
{
    [SerializeField] private GameObject _pelletPrefabs;
    [SerializeField] private Transform[] _pelletPoints;
    [SerializeField] private Player _player;

    [SerializeField] private float _timeCuldown;

    [SerializeField] private bool[] _fillPoints;
    [SerializeField] private int _counterPoint = 0;
    [SerializeField] private int _countDie;
    [SerializeField] private List<GameObject> _pellets;
    [SerializeField] private float _timer;

    private void Awake()
    {
        _pellets = new List<GameObject>();
        _fillPoints = new bool[_pelletPoints.Length];
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K) && _countDie < _bulletPoints.Length)
        //{
        //    //Destroy(_enemys[_counterPoint]);
        //    _fillPoints[_counterPoint] = false;
        //    _counterPoint++;
        //    _countDie++;
        //    if (_countDie > _bulletPoints.Length)
        //        _countDie = _bulletPoints.Length;
        //    _counterPoint %= _bulletPoints.Length;
        //    //4 / 4 = 1 | 0
        //    //5 / 4 = 1 | 1
        //    //6 / 4 = 1 | 2
        //    //7 / 4 = 1 | 3
        //    _timer = 0;
        //}

        if (_timer < _timeCuldown)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            if (_countDie > 0)
                Spawn(_countDie);
        }
    }

    private void Spawn()
    {
        Quaternion q = Quaternion.Euler(270, 0, 180);

        for (int i = 0; i < _pelletPoints.Length; i++)
        {
            //idx = enemyId.Next(0, _enemyPrefabs.Length - 1);
            _pelletPoints[i].position = new Vector3(_pelletPoints[i].position.x, /*_pelletPrefabs.GetComponent<Pellet>().GetComponent<Transform>().position.y*/0.3f, _pelletPoints[i].position.z);

            _pellets.Add(Instantiate(_pelletPrefabs, _pelletPoints[i].position, /*Quaternion.identity*/q));
            
            _fillPoints[i] = true;
        }

        _player.SetPelletArsenal(_pellets);

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
