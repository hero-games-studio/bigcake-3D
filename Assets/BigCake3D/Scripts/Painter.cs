using System.Collections;
using UnityEngine;

public class Painter : MonoSingleton<Painter>
{
    #region Variables
    [Header("Painting")]

    [SerializeField]
    private Vector3 _paintingStartPosition = new Vector3(0, -0.375f, -1.85f);

    [SerializeField]
    private Vector3 _shooterDefaultPosition = new Vector3(0, -0.375f, -3.5f);

    [Header("Topping")]
    [SerializeField]
    private GameObject _toppingPrefab = null;

    public Transform ToppingTransform { get; set; }


    [HideInInspector]
    public bool MissionStage { get; set; } = false;

    [Header("Piece Material")]
    public Material PieceUnColoredMaterial = null;
    public Material PieceColoredMaterial = null;

    private bool isPainting = false;
    #endregion

    #region All Methods
    private void Awake()
    {
        MissionStage = false;
        ToppingTransform = Instantiate(_toppingPrefab).transform;
        ResetToppingPosition();
    }

    public void ResetToppingPosition()
    {
        ToppingTransform.position = new Vector3(0.0f, 75.0f, 0.0f);
        ToppingTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !MissionStage)
        {
            if (!isPainting)
            {
                isPainting = true;
                StartPainting();
            }
        }
        else if (Input.GetMouseButtonUp(0) && !MissionStage)
        {
            isPainting = false;
            StartCoroutine(TurnBack());
        }
    }


    private void StartPainting()
    {
        StartCoroutine(StartApproach());
    }

    private IEnumerator StartApproach()
    {
        StartCoroutine(Shooter.Instance.ChangePosition(_paintingStartPosition));
        yield return null;
    }

    private IEnumerator TurnBack()
    {
        StartCoroutine(Shooter.Instance.ChangePosition(_shooterDefaultPosition));
        yield return null;
    }
    #endregion
}
