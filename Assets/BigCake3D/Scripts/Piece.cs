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
            if (transform.parent.GetComponentInParent<Cream>() != null)
            {
                _meshRenderer.material = Painter.Instance.PieceColoredMaterialWhite;
            }
            _meshRenderer.enabled = true;
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
            if (State == PieceState.UnColored)
            {
                ScoreManager.Instance.AddScore();
                StartCoroutine(ScaleLerp());
            }
            State = PieceState.Colored;
            StageManager.Instance.RotateAndCheckCakePart();
        }
        _uiManager.UpdateScoreText();
    }

    /*
     * METOD ADI :  ScaleLerp
     * AÇIKLAMA  :  Objenin localScale değerini 0 dan objenin başlangıç localScale
     *              değerine doğru arttırır.     
     */
    private IEnumerator ScaleLerp()
    {
        transform.localScale = new Vector3(0.0f, transform.localScale.y, 0.0f);
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime * multiply)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, pieceScale, time);
            yield return null;
        }
        transform.localScale = pieceScale;
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
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        State = PieceState.UnColored;
        _uiManager.UpdateScoreText();
        if (nearMiss)
        {
            _uiManager.UpdateNearMissSlider();
        }
    }
}