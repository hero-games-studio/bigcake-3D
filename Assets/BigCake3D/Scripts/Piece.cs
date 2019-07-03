using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceState State = PieceState.UnColored;
    private MeshRenderer _meshRenderer = null;
    private UiManager _uiManager = null;

    public MeshRenderer PieceMeshRenderer { get { return _meshRenderer; } }

    private void Awake()
    {
        _uiManager = FindObjectOfType<UiManager>();
        _meshRenderer = GetComponent<Renderer>() as MeshRenderer;
    }

    public void SetColored()
    {
        _meshRenderer.material = Painter.Instance.PieceColoredMaterial;
        State = PieceState.Colored;
        ScoreManager.Instance.AddScore();
        _uiManager.UpdateScoreText();
    }

    public void SetUnColored(bool nearMiss = false)
    {
        _meshRenderer.material = Painter.Instance.PieceUnColoredMaterial;
        State = PieceState.UnColored;
        _uiManager.UpdateScoreText();
        if (nearMiss)
        {
            _uiManager.UpdateNearMissSlider();
        }
    }
}