using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    #region Varibles
    [Header("Score")]
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;

    [Header("Near Miss")]
    [SerializeField]
    private Slider _nearMissSlider = null;

    [SerializeField]
    private Animator _nearMissAnimator = null;

    [SerializeField]
    private Animator _nearMissPulseAnimator = null;

    [Header("Mission State")]
    [SerializeField]
    private GameObject _missionStatePanel = null;

    [SerializeField]
    private TextMeshProUGUI _levelNumberText = null;

    [Header("ProgressBar")]
    [SerializeField]
    private Slider progressBarSlider = null;

    [SerializeField]
    private TextMeshProUGUI currentLevelText = null;

    [SerializeField]
    private TextMeshProUGUI nextLevelText = null;

    #endregion

    #region All Methods
    private void Start()
    {
        UpdateScoreText();
        UpdateNearMissSlider();
    }

    /*
     * METOD ADI :  UpdateScoreText
     * AÇIKLAMA  :  ScoreText'i geçerli skora göre günceller
     */
    public void UpdateScoreText()
    {
        _scoreText.text = ScoreManager.Instance.GetScore().ToString();
    }

    /*
     * METOD ADI :  UpdateProgressBar
     * AÇIKLAMA  :  ProgressBar'ı günceller
     */
    public void UpdateProgressBar(float barValue, int currentLevel, int nextLevel)
    {
        progressBarSlider.value = barValue;
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = nextLevel.ToString();
    }

    /*
     * METOD ADI :  ShowMissionState
     * AÇIKLAMA  :  MissionStage paneline görünür yapar.
     */
    public void ShowMissionState(string stageNumber)
    {
        StopAllCoroutines();
        Painter.Instance.TurnBack();
        Painter.Instance.MissionStage = true;
        Painter.Instance.nearMiss = false;
        _missionStatePanel.SetActive(true);
        _levelNumberText.text = stageNumber;
    }

    /*
     * METOD ADI :  HideMissionState
     * AÇIKLAMA  :  MissionStage paneline gizler.
     */
    public void HideMissionState()
    {
        Painter.Instance.nearMiss = true;
        _missionStatePanel.SetActive(false);
        Painter.Instance.MissionStage = false;
    }

    /*
     * METOD ADI :  UpdateNearMissSlider
     * AÇIKLAMA  :  NearMiss slider'ini NearMiss değerine göre günceller.
     */
    public void UpdateNearMissSlider(bool nearMiss = false)
    {
        if (nearMiss)
        {
            _nearMissAnimator.SetTrigger(AnimatorParameters.P_NEARMISS);
            Invoke("ResetNearMissTrigger", 1.1f);
        }
        _nearMissSlider.value = ScoreManager.Instance.GetNearMiss() * 0.1f;
        _nearMissPulseAnimator.SetBool(AnimatorParameters.P_PULSE,
            ScoreManager.Instance.GetNearMiss() >= 10.0f);
    }

    /*
     * METOD ADI :  ClearCurrentStageLayer
     * AÇIKLAMA  :  NearMiss ile geçerli partın boyanma işlemini başlatır.
     */
    public void ClearCurrentStageLayer()
    {
        if (_nearMissSlider.value >= 1.0f)
        {
            ScoreManager.Instance.ResetNearMiss();
            UpdateNearMissSlider();
            Painter.Instance.nearMiss = true;
            StageManager.Instance.ClearCurrentPartWithNearMiss();
        }
    }

    /*
     * METOD ADI :  ResetNearMissTrigger
     * AÇIKLAMA  :  NearMiss animasyonunu durdurur.
     */
    private void ResetNearMissTrigger() =>
        _nearMissAnimator.ResetTrigger(AnimatorParameters.P_NEARMISS);

    #endregion
}