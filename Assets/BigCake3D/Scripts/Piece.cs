using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceState State = PieceState.UnColored;
    private MeshRenderer _meshRenderer = null;
    private Collider _collider = null;
    private UiManager _uiManager = null;

    public MeshRenderer PieceMeshRenderer { get { return _meshRenderer; } }

    private Vector3 pieceScale;
    private float multiply = 1.35f;
    private void Awake()
    {
        multiply = GetComponentInParent<Cream>() != null ? 0.65f : 1.35f;
        pieceScale = transform.localScale;
        _uiManager = FindObjectOfType<UiManager>();
        _meshRenderer = GetComponent<Renderer>() as MeshRenderer;
        _collider = GetComponent<Collider>();
    }

    /*
     * METOD ADI :  SetColored
     * AÇIKLAMA  :  Piece objesini boyanması işlemlerini yapar.     
     */
    public void SetColored()
    {
        _collider.enabled = false;
        if (Painter.Instance.isPainting)
        {
            _meshRenderer.enabled = true;

            ChangeChildrenVisibility(true);

            if (State == PieceState.UnColored)
            {
                ScoreManager.Instance.AddScore();
                if (transform.parent.GetComponentInParent<Cream>() != null)
                {
                    _meshRenderer.material = Painter.Instance.PieceColoredMaterialWhite;
                    ScaleLerp(Random.Range(0.2f, 0.45f));
                }
                else
                {
                    ScaleLerp(Random.Range(0.25f, 0.35f));
                }
            }
            State = PieceState.Colored;
            StageManager.Instance.RotateAndCheckCakePart();
        }
        _uiManager.UpdateScoreText();
    }

    private void ChangeChildrenVisibility(bool visiblity)
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = visiblity;
        }
    }

    /*
     * METOD ADI :  ScaleLerp
     * AÇIKLAMA  :  Objenin localScale değerini 0 dan objenin başlangıç localScale
     *              değerine doğru arttırır.     
     */
    private void ScaleLerp(float time)
    {
        transform.localScale = new Vector3(0.0f, transform.localScale.y, 0.0f);
        transform.DOScale(pieceScale, time);
    }

    /*
     * METOD ADI :  SetUnColored
     * AÇIKLAMA  :  Piece objesini boyanmamış haline döndürür.     
     */
    public void SetUnColored(bool nearMiss = false)
    {
        if (transform.parent.GetComponentInParent<Cream>() != null)
        {
            _meshRenderer.material = Painter.Instance.PieceUnColoredMaterial;
        }
        _collider.enabled = true;
        _meshRenderer.enabled = false;

        ChangeChildrenVisibility(false);

        State = PieceState.UnColored;
        _uiManager.UpdateScoreText();
        if (nearMiss)
        {
            _uiManager.UpdateNearMissSlider();
        }
    }
}