using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;

    [Header("Mission State")]
    [SerializeField]
    private GameObject _missionStatePanel = null;

    [SerializeField]
    private TextMeshProUGUI _levelNumberText = null;

    private void Start()
    {
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        _scoreText.text = ScoreManager.Instance.GetScore().ToString();
    }

    public void ShowMissionState(string stageNumber)
    {
        _missionStatePanel.SetActive(true);
        _levelNumberText.text = stageNumber;
    }

    public void HideMissionState()
    {
        _missionStatePanel.SetActive(false);
    }
}
