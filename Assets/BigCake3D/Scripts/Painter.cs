using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet = null;

    [SerializeField]
    private int _maxSpawnCount = 35;

    private Vector3 _shootStart = new Vector3(0, -0.25f, -4.0f);

    private int _currentIndex = 0;
    private Vector3 _force = new Vector3(0, 0, 15000);
    private List<GameObject> _bullets = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < _maxSpawnCount; i++)
        {
            _bullets.Add(Spawn());
            _bullets[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)/* || Input.GetMouseButton(0)*/)
        {
            Shoot();
        }
    }

    private GameObject Spawn() => Instantiate(_bullet);

    private void Shoot()
    {
        if (_currentIndex >= _bullets.Count)
        {
            _currentIndex = 0;
        }
        _bullets[_currentIndex].transform.position = _shootStart;
        _bullets[_currentIndex].SetActive(true);
        _bullets[_currentIndex++].GetComponent<Rigidbody>()
            .AddForce(_force * Time.deltaTime);
    }
}
