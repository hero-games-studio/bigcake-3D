using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UiManager : MonoBehaviour
{
    #region Varibles
    [Header("Score")]
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;

    [Header("Near Miss")]
    [SerializeField]
    private Image _nearMissBar = null;

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
        StartCoroutine(UpdateNearMissSlider());
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
    public IEnumerator UpdateProgressBar(float barValue, int currentLevel, int nextLevel)
    {
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime * 2.5f)
        {
            progressBarSlider.value = Mathf.Lerp(progressBarSlider.value, barValue, time);
            yield return null;
        }
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
        _missionStatePanel.SetActive(true);
        _levelNumberText.text = stageNumber;
    }

    /*
     * METOD ADI :  HideMissionState
     * AÇIKLAMA  :  MissionStage paneline gizler.
     */
    public void HideMissionState()
    {
        _missionStatePanel.SetActive(false);
        Painter.Instance.MissionStage = false;
    }

    /*
     * METOD ADI :  UpdateNearMissSlider
     * AÇIKLAMA  :  NearMiss slider'ini NearMiss değerine göre günceller.
     */
    public IEnumerator UpdateNearMissSlider(bool nearMiss = false)
    {
        /*   if (nearMiss)
           {
               _nearMissAnimator.SetTrigger(AnimatorParameters.P_NEARMISS);
               Invoke("ResetNearMissTrigger", 1.1f);
           }*/

        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime * 3.0f)
        {
            _nearMissBar.fillAmount =
                Mathf.Lerp(_nearMissBar.fillAmount, ScoreManager.Instance.GetNearMiss() * 0.1f, time);
            yield return null;
        }

        if (_nearMissBar.fillAmount >= 1.0f && !Painter.Instance.isCleaning)
        {
            Painter.Instance.isCleaning = true;
            ClearCurrentStageLayer();
        }
        else if(_nearMissBar.fillAmount <= 0.0f && Painter.Instance.isCleaning)
        {
            Painter.Instance.isCleaning = false;
            ClearCurrentStageLayer();
        }

        _nearMissBar.transform.parent.gameObject.SetActive(!(_nearMissBar.fillAmount <= 0.0f));

    }

    /*
     * METOD ADI :  ClearCurrentStageLayer
     * AÇIKLAMA  :  NearMiss ile geçerli partın boyanma işlemini başlatır.
     */
    public void ClearCurrentStageLayer()
    {
        StageManager.Instance.ClearCurrentPartWithNearMiss(!Painter.Instance.isCleaning);
    }

    /*
     * METOD ADI :  ResetNearMissTrigger
     * AÇIKLAMA  :  NearMiss animasyonunu durdurur.
     */
    private void ResetNearMissTrigger() =>
        _nearMissAnimator.ResetTrigger(AnimatorParameters.P_NEARMISS);
    #endregion
}