using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceState State = PieceState.UnColored;
    private MeshRenderer _meshRenderer = null;
    private Color32 _obstacleColor = new Color32(123, 63, 0, 255);
    private UiManager _uiManager = null;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UiManager>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColored()
    {
        _meshRenderer.material.color = _obstacleColor;
        State = PieceState.Colored;
        ScoreManager.Instance.AddScore();
        _uiManager.UpdateScoreText();
    }
}