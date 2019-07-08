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
    private void Awake()
    {
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
        if (State == PieceState.UnColored)
        {
            ScoreManager.Instance.AddScore();
            if (Painter.Instance.isPainting)
            {
                if (transform.parent.GetComponentInParent<Cream>() != null)
                {
                    _meshRenderer.material = Painter.Instance.PieceColoredMaterialWhite;
                }
                _collider.enabled = false;
                _meshRenderer.enabled = true;
                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
                StartCoroutine(ScaleLerp());
                State = PieceState.Colored;
            }
            _uiManager.UpdateScoreText();
        }
        else
        {
            StageManager.Instance.RotateAndCheckCakePart();
        }
    }

    /*
     * METOD ADI :  ScaleLerp
     * AÇIKLAMA  :  Objenin localScale değerini 0 dan objenin başlangıç localScale
     *              değerine doğru arttırır.     
     */
    private IEnumerator ScaleLerp()
    {
        StageManager.Instance.RotateAndCheckCakePart();
        transform.localScale = new Vector3(0.0f, transform.localScale.y, 0.0f);
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime * 1.35f)
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