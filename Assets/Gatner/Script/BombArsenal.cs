using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BombArsenal : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefabs;
    [SerializeField] private Transform[] _bombPoints;
    [SerializeField] private Player _player;
    [SerializeField] private float _timeCuldown;

    [SerializeField] private bool[] _fillPoints;
    [SerializeField] private int _counterPoint = 0;
    [SerializeField] private int _countDie;
    [SerializeField] private List<GameObject> _bomb;
    [SerializeField] private float _timer;

    private void Awake()
    {
        _bomb = new List<GameObject>();
        _fillPoints = new bool[_bombPoints.Length];
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
        Quaternion q = Quaternion.Euler(270, 180, 90);

        for (int i = 0; i < _bombPoints.Length; i++)
        {
            //idx = enemyId.Next(0, _enemyPrefabs.Length - 1);
            _bombPoints[i].position = new Vector3(_bombPoints[i].position.x, 0.2f, _bombPoints[i].position.z);
            _bomb.Add(Instantiate(_bombPrefabs, _bombPoints[i].position, /*Quaternion.identity*/q));

            _fillPoints[i] = true;
        }

        _player.SetBombArsenal(_bomb);

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
