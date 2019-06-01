using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoSingleton<Painter>
{
    #region Variables
    [Header("Bullet")]
    [SerializeField]
    private GameObject _bullet = null;
    [SerializeField]
    private int _maxSpawnCount = 35;
    [SerializeField]
    private Transform _bulletParent = null;

    [SerializeField]
    private StageManager _stageManager = null;

    private int _currentIndex = 0;
    private Vector3 _force = new Vector3(0, 0, 12500);
    private List<GameObject> _bullets = new List<GameObject>();
    private float _prevTime = 1.0f;
    private static readonly float _inTimeBound = 0.2f;
    #endregion

    #region All Methods
    private void Awake()
    {
        for (int i = 0; i < _maxSpawnCount; i++)
        {
            _bullets.Add(Spawn());
            _bullets[i].transform.SetParent(_bulletParent);
            _bullets[i].SetActive(false);
        }
    }

    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            CheckInTimeAndShoot();
        }
    }

    private void CheckInTimeAndShoot()
    {
        if (Time.time - _prevTime >= _inTimeBound)
        {
            Shoot();
            _prevTime = Time.time;
        }
    }

    private GameObject Spawn() => Instantiate(_bullet);

    private void Shoot()
    {
        if (_currentIndex >= _bullets.Count)
        {
            _currentIndex = 0;
        }
        _bullets[_currentIndex].transform.position = Shooter.Instance.ShootStartPosition;
        _bullets[_currentIndex].SetActive(true);
        _bullets[_currentIndex++].GetComponent<Rigidbody>()
            .AddForce(_force * Time.deltaTime);
    }

    public void RotateAndCheck()
    {
        _stageManager.RotateAndCheckCake();
    }
    #endregion
}
