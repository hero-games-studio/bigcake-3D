﻿using UnityEngine;

public class ScoreManager : MonoSingleton<ScoreManager>
{
    [SerializeField]
    private UiManager _uiManager = null;

    #region Score 
    private int _score = 0;

    public void AddScore(int point = 10)
    {
        _score += point;
    }

    public int GetScore() => _score;

    #endregion

    #region Near Miss
    private float _nearMiss = 0;

    public void AddNearMiss(float point = 1)
    {
        _uiManager.UpdateNearMissSlider(true);
        _nearMiss = _nearMiss >= 10.0f ? 10.0f : _nearMiss + point;
    }

    public float GetNearMiss() => _nearMiss;

    public void ResetNearMiss()
    {
        _nearMiss = 0.0f;
    }
    #endregion
}
