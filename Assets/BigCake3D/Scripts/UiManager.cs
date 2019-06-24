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
    private Button _nearMissButton = null;

    [SerializeField]
    private Animator _nearMissAnimator = null;

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

    public void UpdateScoreText()
    {
        _scoreText.text = ScoreManager.Instance.GetScore().ToString();
    }

    public void UpdatePregressBar(float barValue, int currentLevel, int nextLevel)
    {
        progressBarSlider.value = barValue;
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = nextLevel.ToString();
    }

    public void ShowMissionState(string stageNumber)
    {
        StopAllCoroutines();
        Painter.Instance.TurnBack();
        _missionStatePanel.SetActive(true);
        _levelNumberText.text = stageNumber;
    }

    public void HideMissionState()
    {
        Painter.Instance.MissionStage = false;
        _missionStatePanel.SetActive(false);
    }

    public void UpdateNearMissSlider(bool nearMiss = false)
    {
        if (nearMiss)
        {
            _nearMissAnimator.SetTrigger(AnimatorParameters.P_NEARMISS);
            Invoke("ResetNearMissTrigger", 1.1f);
        }
        _nearMissButton.interactable = ScoreManager.Instance.GetNearMiss() >= 10.0f;
        _nearMissSlider.value = ScoreManager.Instance.GetNearMiss() * 0.1f;
    }

    public void ClearCurrentStageLayer()
    {
        Painter.Instance.MissionStage = true;
        _nearMissButton.interactable = false;
        ScoreManager.Instance.ResetNearMiss();
        UpdateNearMissSlider();
        StartCoroutine(StageManager.Instance.ClearCurrentPartWithNearMiss());
    }

    private void ResetNearMissTrigger() =>
        _nearMissAnimator.ResetTrigger(AnimatorParameters.P_NEARMISS);

    #endregion
}