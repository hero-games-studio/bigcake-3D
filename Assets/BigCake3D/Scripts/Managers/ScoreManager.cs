using UnityEngine;

public class ScoreManager : MonoSingleton<ScoreManager>
{
    [SerializeField]
    private UiManager _uiManager = null;

    #region Score 
    private int _score = 0;

    /*
     * METOD ADI :  AddScore
     * AÇIKLAMA  :  Score değerine parametre olarak döndürülen değeri ekler.
     */
    public void AddScore(int point = 10)
    {
        _score += point;
        _uiManager.UpdateScoreText();
    }

    /*
     * METOD ADI :  GetScore
     * AÇIKLAMA  :  Score değerini döndürür.
     */
    public int GetScore() => _score;

    #endregion

    #region Near Miss
    private float _nearMiss = 0.0f;

    /*
     * METOD ADI :  AddNearMiss
     * AÇIKLAMA  :  Near miss değerine parametre olarak döndürülen değeri ekler.
     */
    public void AddNearMiss(float point = 1)
    {
        _nearMiss = _nearMiss >= 10.0f ? 10.0f : _nearMiss + point;
        StartCoroutine(_uiManager.UpdateNearMissSlider(true));
    }

    /*
     * METOD ADI :  ResetScoreAndNearMiss
     * AÇIKLAMA  :  Near miss ve Score değerlerini sıfırlar.
     */
    public void ResetScoreAndNearMiss()
    {
        ResetNearMiss();
        _score = 0;
        _uiManager.UpdateScoreText();
    }

    /*
     * METOD ADI :  GetNearMiss
     * AÇIKLAMA  :  Near miss değerini döndürür.
     */
    public float GetNearMiss() => _nearMiss;

    /*
     * METOD ADI :  ResetNearMiss
     * AÇIKLAMA  :  Near miss değerini sıfırlar.
     */
    public void ResetNearMiss()
    {
        _nearMiss = 0.0f;
        _uiManager.UpdateNearMissSlider();
    }
    #endregion
    /*
    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 250, 125), "Tıkla"))
        {
            _nearMiss = 10.0f;
            _uiManager.UpdateNearMissSlider();
        }
    }
    */
}
