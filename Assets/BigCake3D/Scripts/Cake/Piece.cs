using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceState State = PieceState.UnColored;
    private MeshRenderer _meshRenderer = null;
    //private Collider _collider = null;
    private UiManager _uiManager = null;
    private Vector3 pieceScale;
    private float multiply = 2.0f;

    private WaitForSeconds delay;

    public MeshRenderer PieceMeshRenderer { get { return _meshRenderer; } }
    public int index = 0;

    private void Awake()
    {
        pieceScale = transform.localScale;
        _uiManager = FindObjectOfType<UiManager>();
        _meshRenderer = GetComponent<Renderer>() as MeshRenderer;
        //_collider = GetComponent<Collider>();
        delay = new WaitForSeconds(OtherData.duration * Time.deltaTime);
    }

    /*
     * METOD ADI :  SetColored
     * AÇIKLAMA  :  Piece objesini boyanması işlemlerini yapar.     
     */
    public void SetColored()
    {
        //_collider.enabled = false;
        _meshRenderer.enabled = true;

        ChangeChildrenVisibility(true);
        
        if (State == PieceState.UnColored)
        {
            State = PieceState.Colored;
            ScoreManager.Instance.AddScore();
            if (transform.parent.GetComponentInParent<Cream>() != null)
            {
                _meshRenderer.material = Painter.Instance.PieceColoredMaterialWhite;
            }
            StartCoroutine(ScaleLerp());
        }
        _uiManager.UpdateScoreText();
        StageManager.Instance.RotateAndCheckCakePart();
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
    private IEnumerator ScaleLerp()
    {
        transform.localScale = new Vector3(0.0f, transform.localScale.y, 0.0f);
        for (float time = 0; time < 1.0f; time += Time.deltaTime * multiply)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, pieceScale, time);
            yield return delay;
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
        //_collider.enabled = true;
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