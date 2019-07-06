using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceState State = PieceState.UnColored;
    private MeshRenderer _meshRenderer = null;
    private UiManager _uiManager = null;

    public MeshRenderer PieceMeshRenderer { get { return _meshRenderer; } }

    private Vector3 pieceScale;

    private void Awake()
    {
        pieceScale = transform.localScale;
        _uiManager = FindObjectOfType<UiManager>();
        _meshRenderer = GetComponent<Renderer>() as MeshRenderer;
    }

    public void SetColored()
    {
        if (State == PieceState.UnColored)
        {
            ScoreManager.Instance.AddScore();
            if (Painter.Instance.isPainting)
            {
                _meshRenderer.material = GetComponentInParent<Cream>() != null ?
                    Painter.Instance.PieceColoredMaterialWhite : Painter.Instance.PieceColoredMaterial;
                _meshRenderer.enabled = true;
                StartCoroutine(ScaleLerp());
                State = PieceState.Colored;
            }
            _uiManager.UpdateScoreText();
        }
        StageManager.Instance.RotateAndCheckCakePart();
    }

    private IEnumerator ScaleLerp()
    {
        transform.localScale = new Vector3(0.0f, transform.localScale.y, 0.0f);
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime * 1.35f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, pieceScale, time);
            yield return null;
        }
        transform.localScale = pieceScale;
    }

    public void SetUnColored(bool nearMiss = false)
    {
        _meshRenderer.material = Painter.Instance.PieceUnColoredMaterial;
        _meshRenderer.enabled = false;
        State = PieceState.UnColored;
        _uiManager.UpdateScoreText();
        if (nearMiss)
        {
            _uiManager.UpdateNearMissSlider();
        }
    }
}