using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet = null;

    [SerializeField]
    private int _maxSpawnCount = 35;

    [SerializeField]
    private Transform _shooter = null;

    [SerializeField]
    private GameObject _obstacle = null;

    private int _currentIndex = 0;
    private Vector3 _force = new Vector3(0, 0, 12500);
    private List<GameObject> _bullets = new List<GameObject>();
    private float _inTime = 1.0f;
    private Cake _cake = null;

    [SerializeField]
    private Cake[] _cakes = null;

    private int _currentCakeIndex = 0;
    private Vector3 _startPosIncrease = new Vector3(0.0f, 0.5f, 0.0f);

    private float _currentCakeLayerYPos = 0.0f;
    private Vector3 _startFallPos = new Vector3(0, 10, 0);
    private static readonly float _inTimeBound = 0.2f;

    private void Awake()
    {
        for (int i = 0; i < _maxSpawnCount; i++)
        {
            _bullets.Add(Spawn());
            _bullets[i].SetActive(false);
        }
        _cakes = FindObjectsOfType<Cake>();
        SortAndHideCakes();
        GetActiveCakePart();
    }

    private void Update()
    {
        _inTime += Time.deltaTime;
#if UNITY_EDITOR
        StandaloneInput();
#elif UNITY_IOS ||UNITY_ANDROID
        MobileInput();
#endif
    }

    private void StandaloneInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            CheckInTimeAndShoot();
        }
    }

    private void CheckInTimeAndShoot()
    {
        if (_inTime >= _inTimeBound)
        {
            Shoot();
            _inTime = 0.0f;
        }
    }

    private void MobileInput()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
        {
            CheckInTimeAndShoot();
        }
    }

    private GameObject Spawn() => Instantiate(_bullet);

    private void Shoot()
    {
        _shooter.position = GameManager.GetInstance().ShootStart;
        if (_currentIndex >= _bullets.Count)
        {
            _currentIndex = 0;
        }
        _bullets[_currentIndex].transform.position = GameManager.GetInstance().ShootStart;
        _bullets[_currentIndex].SetActive(true);
        _bullets[_currentIndex++].GetComponent<Rigidbody>()
            .AddForce(_force * Time.deltaTime);
    }

    private void SortAndHideCakes()
    {
        int length = _cakes.Length;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (_cakes[i].transform.position.y < _cakes[j].transform.position.y)
                {
                    var temp = _cakes[i];
                    _cakes[i] = _cakes[j];
                    _cakes[j] = temp;
                }
            }
        }

        foreach (var item in _cakes)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void GetActiveCakePart()
    {
        if (_cake != null && _cake.CheckChilds())
        {
            GameManager.GetInstance().ShootStart += _startPosIncrease;
            _currentCakeLayerYPos += _startPosIncrease.y;
        }

        if (_currentCakeIndex < _cakes.Length)
        {
            _cake = _cakes[_currentCakeIndex++];
            StartCoroutine(FallLayerDown());
            _cake.gameObject.SetActive(true);
        }
        else
        {
            _startPosIncrease = new Vector3(0, 0, 0);
        }
    }

    private IEnumerator FallLayerDown()
    {
        _cake.transform.position = _startFallPos;
        Vector3 target = new Vector3(0, _currentCakeLayerYPos, 0);
        for (float time = 0; time < 1.0f; time += Time.deltaTime)
        {
            _cake.transform.position = Vector3.Lerp(_cake.transform.position, target, time);
            _obstacle.transform.position = Vector3.Lerp(_obstacle.transform.position, target, time);
            yield return null;
        }
        _cake.transform.position = target;
        _obstacle.transform.position = target;
    }

    public void RotateAndCheckCake()
    {
        StartCoroutine(_cake.RotateMe());
        if (_cake.CheckChilds())
        {
            GetActiveCakePart();
        }
    }
}
