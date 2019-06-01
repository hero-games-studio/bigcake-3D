using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Variables
    private Vector3 _startPosIncrease = new Vector3(0.0f, 0.5f, 0.0f);

    private float _currentCakeLayerYPos = 0.0f;
    private Vector3 _startFallPos = new Vector3(0, 10, 0);
    private Cake _cake;

    public List<Stage> _stages;
    public int _currentStageIndex = 0;

    #endregion

    [System.Serializable]
    public class Stage
    {
        public GameObject stage;
        public List<Cake> cakes;
        public GameObject obstacle;
        public int currentIndex = 0;

        public Cake GetCurrentCake() => cakes[currentIndex];
    }

    #region All Methods
    private void Start()
    {
        _stages[_currentStageIndex].stage.SetActive(true);
        _cake = _stages[_currentStageIndex].GetCurrentCake();
        _cake.gameObject.SetActive(true);
    }

    private void ResetPositionVariables()
    {
        Shooter.Instance.ResetShootStartPosition();
        _currentCakeLayerYPos = 0;
    }

    public void GetActiveCakePart()
    {
        if (_cake != null && _cake.CheckChilds())
        {
            if (_currentStageIndex >= _stages.Count)
            {
                _currentStageIndex = 0;
            }

            if (_stages[_currentStageIndex].currentIndex >= _stages[_currentStageIndex].cakes.Count - 1)
            {
                ResetPositionVariables();
                _stages[_currentStageIndex++].stage.gameObject.SetActive(false);
                _stages[_currentStageIndex].stage.gameObject.SetActive(true);
            }
            else
            {
                Shooter.Instance.ShootStartPosition += _startPosIncrease;
                _stages[_currentStageIndex].currentIndex++;
                _currentCakeLayerYPos += _startPosIncrease.y;
            }
            _cake = _stages[_currentStageIndex].GetCurrentCake();
            _cake.gameObject.SetActive(true);
            StartCoroutine(FallLayerDown());
        }
    }

    private IEnumerator FallLayerDown()
    {
        Shooter.Instance.ChangePosition();
        _cake.transform.position = _startFallPos;
        Vector3 target = new Vector3(0, _currentCakeLayerYPos, 0);
        for (float time = 0; time < 1.0f; time += Time.deltaTime)
        {
            _cake.transform.position = Vector3.Lerp(_cake.transform.position, target, time);
            _stages[_currentStageIndex].obstacle.transform.position =
                Vector3.Lerp(_stages[_currentStageIndex].obstacle.transform.position, target, time);
            yield return null;
        }
        _cake.transform.position = target;
        _stages[_currentStageIndex].obstacle.transform.position = target;
    }

    public void RotateAndCheckCake()
    {
        StartCoroutine(_cake.RotateMe());
        if (_cake.CheckChilds())
        {
            GetActiveCakePart();
        }
    }
    #endregion
}
