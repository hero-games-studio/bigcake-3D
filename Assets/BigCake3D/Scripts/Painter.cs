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

    [HideInInspector] public bool fail = false;
    [HideInInspector] public bool goingUp = false;
    [HideInInspector] public bool isCleaning = false;
    #endregion

    #region All Methods
    private void Awake()
    {
        MissionStage = false;
    }

    private void Update()
    {
        if (isPainting)
        {
            if (Time.time - previousTime > boundTime)
            {
                StageManager.Instance.currentStage.GetCurrentCakePart().PaintPieces();
                previousTime = Time.time;
            }

            if (StageManager.Instance.currentStage.obstacle.activeInHierarchy)
            {
                ScoreManager.Instance.AddNearMiss(Time.deltaTime * 2.5f);
            }
            else
            {
                ScoreManager.Instance.AddNearMiss(-Time.deltaTime * 2.5f);
            }
        }
        else
        {
            if (!MissionStage)
            {
                ScoreManager.Instance.AddNearMiss(-Time.deltaTime * 1.25f);
            }
        }

        GetInputs();
    }

    /*
     * METOD ADI :  GetInputs
     * AÇIKLAMA  :  Kullanıcıdan gelen inputları kontrol eder.
     */
    private void GetInputs()
    {
        if (!goingUp && (Input.GetMouseButton(0) && !MissionStage))
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
        Shooter.Instance.StartSqueeze();
    }

    /*
     * METOD ADI :  TurnBack
     * AÇIKLAMA  :  Shooter objesini ilk pozisyonuna geri döndürme işlemini başlatır.
     */
    public void TurnBack()
    {
        isPainting = false;
        Shooter.Instance.StopSqueeze();
        MissionStage = false;
    }
    #endregion
}
