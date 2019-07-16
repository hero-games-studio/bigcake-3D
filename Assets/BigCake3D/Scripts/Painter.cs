using UnityEngine;

public class Painter : MonoSingleton<Painter>
{
    #region Variables
    [Header("Painting")]

    [SerializeField]
    private Vector3 _paintingStartPosition = new Vector3(0, 0, -1.85f);

    [SerializeField]
    private Vector3 _shooterDefaultPosition = new Vector3(0, 0, -3.5f);

    [Header("Paint Bound")]
    [SerializeField]
    private float boundTime = 0.05f;

    private float previousTime = -0.05f;

    [HideInInspector]
    public bool MissionStage { get; set; } = false;

    [Header("Piece Material")]
    public Material PieceUnColoredMaterial = null;
    public Material PieceColoredMaterial = null;
    public Material PieceColoredMaterialWhite = null;

    [HideInInspector] public bool isPainting = false;

    [HideInInspector] public bool nearMiss = false;
    [HideInInspector] public bool fail = false;
    #endregion

    #region All Methods
    private void Awake()
    {
        MissionStage = false;
    }

    private void Update()
    {
        GetInputs();

        if (isPainting)
        {
            if (Time.time - previousTime > boundTime)
            {
                StageManager.Instance.currentStage.GetCurrentCakePart().PaintPieces();
                previousTime = Time.time;
            }
        }
    }

    /*
     * METOD ADI :  GetInputs
     * AÇIKLAMA  :  Kullanıcıdan gelen inputları kontrol eder.
     */
    private void GetInputs()
    {
        if (MissionStage)
        {
            TurnBack();
        }

        if ((Input.GetMouseButton(0) && !MissionStage) | nearMiss)
        {
            if (!isPainting && !StageManager.Instance.fallingDown && !fail)
            {
                StartApproach();
                isPainting = true;
            }
            else if (fail)
            {
                TurnBack();
            }
        }
        else if (Input.GetMouseButtonUp(0) && !MissionStage)
        {
            fail = false;
            isPainting = false;
            TurnBack();
        }
        else
        {
            Shooter.Instance.StopSqueeze();
        }
    }

    /*
     * METOD ADI :  StartApproach
     * AÇIKLAMA  :  Shooter objesini keke yaklaştırma işlemini başlatır.
     */
    public void StartApproach()
    {
        StartCoroutine(Shooter.Instance.ChangePosition(_paintingStartPosition, true));
    }

    /*
     * METOD ADI :  TurnBack
     * AÇIKLAMA  :  Shooter objesini ilk pozisyonuna geri döndürme işlemini başlatır.
     */
    public void TurnBack()
    {
        isPainting = false;
        Shooter.Instance.StopSqueeze();
        StartCoroutine(Shooter.Instance.ChangePosition(_shooterDefaultPosition, false));
    }
    #endregion
}
