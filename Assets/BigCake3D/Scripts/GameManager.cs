using UnityEngine;

public class GameManager
{
    private static GameManager _instance = null;
    private static object _lock = new object();

    private int _score = 0;
    private Vector3 _shootStartDefault = new Vector3(0, -0.25f, -4.0f);
    private Vector3 _shootStart = new Vector3(0, -0.25f, -4.0f);

    public Vector3 ShootStart {
        get
        {
            return _shootStart;
        }
        set
        {
            _shootStart = value;
        }
    }

    private GameManager() { }

    public static GameManager GetInstance()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public void AddScore(int point = 10)
    {
        _score += point;
    }

    public void ResetShootStartPosition()
    {
        _shootStart = _shootStartDefault;
    }

    public int GetScore() => _score;
}
