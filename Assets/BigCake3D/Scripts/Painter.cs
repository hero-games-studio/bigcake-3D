using System.Collections;
using UnityEngine;

public class Painter : MonoSingleton<Painter>
{
    #region Variables
    [Header("Painting")]

    [SerializeField]
    private Vector3 _paintingStartPosition = new Vector3(0, 0, -1.85f);

    [SerializeField]
    private Vector3 _shooterDefaultPosition = new Vector3(0, 0, -3.5f);

    [Header("Topping")]
    [SerializeField]
    private GameObject _toppingPrefab = null;

    public Transform ToppingTransform { get; set; }


    [HideInInspector]
    public bool MissionStage { get; set; } = false;

    [Header("Piece Material")]
    public Material PieceUnColoredMaterial = null;
    public Material PieceColoredMaterial = null;
    public Material PieceColoredMaterialWhite = null;

    [HideInInspector] public bool isPainting = false;

    [HideInInspector] public bool nearMiss = false;
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
        if ((Input.GetMouseButton(0) && !MissionStage) | nearMiss)
        {
            if (!isPainting && !StageManager.Instance.fallingDown)
            {
                StartApproach();
                isPainting = true;
            }
        }
        else if (Input.GetMouseButtonUp(0) && !MissionStage)
        {
            isPainting = false;
            TurnBack();
        }
        else 
        {
            Shooter.Instance.StopSqueeze();
        }
    }

    public void StartApproach()
    {
        StartCoroutine(Shooter.Instance.ChangePosition(_paintingStartPosition, true));
    }

    public void TurnBack()
    {
        Shooter.Instance.StopSqueeze();
        StartCoroutine(Shooter.Instance.ChangePosition(_shooterDefaultPosition, false));
    }
    #endregion
}
