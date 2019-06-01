using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;

    private void Start()
    {
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        _scoreText.text = ScoreManager.Instance.GetScore().ToString();
    }
}
