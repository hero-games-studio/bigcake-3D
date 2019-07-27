using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class StageManager : MonoSingleton<StageManager>
{
    #region Variables
    public List<Stage> stages;

    [HideInInspector]
    public int currentStageIndex = 0;

    [HideInInspector]
    public bool fallingDown = false;
    [HideInInspector]
    public Stage currentStage;

    private bool isCake = true;

    [Header("Positions")]
    [SerializeField] private Vector3 obstaclePosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 obstacleStartPosition = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField] private Vector3 cakePosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 cakeStartPosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 cakePositionStepSize = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 creamPositionStepSize = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("Scripts")]
    [SerializeField] private UiManager uiManager = null;

    private WaitForSeconds delayNearMiss = new WaitForSeconds(0.75f);

    DataManager dataManager;
    #endregion

    #region Methods 

    private void Awake()
    {
        GameAnalytics.Initialize();

        dataManager = DataManager.Instance;
        dataManager.Load();

        currentStageIndex = dataManager.data.level;

        for (int i = 0; i < currentStageIndex; i++)
        {
            stages[i].stage.SetActive(false);
        }

        PrepareCurrentStage();

        StartCoroutine(uiManager.UpdateProgressBar(0,
            currentStageIndex + 1, currentStageIndex + 2));
    }

    /*
     * METOD ADI :  FallDown
     * AÇIKLAMA  :  Parametre olarak gönderilen objenin pozisyonunu
     *              parametre olarak target değerine eşitler.
     */
    private IEnumerator FallDown(Transform tr, Vector3 target, bool obstacle, bool topping = false)
    {
        fallingDown = true;

        Painter.Instance.goingUp = true;
        var pos = obstacle ? tr.position :
            topping ? new Vector3(tr.position.x, 50.0f, tr.position.z) : new Vector3(tr.position.x, 6.0f, tr.position.z);
        if (!obstacle)
        {
            tr.gameObject.SetActive(true);
        }
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime)
        {
            tr.position = Vector3.Lerp(pos, target, time);
            yield return null;
        }
        tr.position = target;

        if (!obstacle)
        {
            Shooter.Instance.GoOneStepUp(target.y);
        }
    }

    /*
     * METOD ADI :  PrepareCurrentStage
     * AÇIKLAMA  :  Geçerli Stage'i ayarlayıp, stage objesini aktif eder. 
     */
    private void PrepareCurrentStage()
    {
        currentStage = stages[currentStageIndex];
        currentStage.stage.SetActive(true);
        PrepareCurrentPart();
    }

    /*
     * METOD ADI :  PrepareCurrentPart
     * AÇIKLAMA  :  Geçerli partı hazırlar.
     */
    private void PrepareCurrentPart()
    {
        StartCoroutine(FallDown(currentStage.obstacle.transform, obstaclePosition, true));
        currentStage.GetCurrentCakePart().gameObject.SetActive(true);
        currentStage.GetCurrentCakePart().ResetRotation();
        Shooter.Instance.SetCreamMaterial(currentStage.GetCurrentCakePart().GetPieceColor());
        if (currentStage.GetCurrentCakePart() as Cream)
        {
            foreach (Piece piece in
                currentStage.GetCurrentCakePart().GetComponentsInChildren<Piece>())
            {
                piece.PieceMeshRenderer.enabled = false;
            }
            //currentStage.GetCurrentCakePart().ShowFirstPiece();
        }
        StartCoroutine(FallDown(currentStage.GetCurrentCakePart().transform,
            currentStage.GetCurrentCakePart().transform.position, false));

        if (!Painter.Instance.isCleaning)
        {
            currentStage.obstacle.SetActive(true);
        }

        Painter.Instance.MissionStage = false;
    }

    /*
     * METOD ADI :  RotateAndCheckCakePart
     * AÇIKLAMA  :  Objeyi döndürüp, tamamlama durumunu kotrol eder.
     */
    public void RotateAndCheckCakePart()
    {
        if (currentStage.GetCurrentCakePart().IsPartCompelete())
        {
            Painter.Instance.TurnBack();
            Shooter.Instance.StopSqueeze();
            GetNextPart();
        }
        else
        {
            currentStage.GetCurrentCakePart().RotateMe();
        }
    }

    /*
     * METOD ADI :  ResetCurrentPart
     * AÇIKLAMA  :  Geçerli partı sıfırlar.
     */
    public void ResetCurrentPart()
    {
        Painter.Instance.TurnBack();
        currentStage.GetCurrentCakePart().ResetPart();
    }

    /*
     * METOD ADI :  GetNextPart
     * AÇIKLAMA  :  Geçerli part tamamlandıktan sonra, bir sonraki partı boyamaya hazırlar.
     *              Stage tamamlanmış ise bir sonraki Stage'ı başlatır.
     */
    private void GetNextPart()
    {
        Painter.Instance.TurnBack();

        ParticleManager.Instance.PlayStarRing(currentStage.GetCurrentCakePart().transform.position);
        currentStage.currentPartIndex++;
        if (currentStage.currentPartIndex >= currentStage.cakeParts.Count)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Application.version,
                currentStageIndex.ToString("00000"), ScoreManager.Instance.GetScore());

            Painter.Instance.MissionStage = true;
            currentStage.topping.SetActive(true);
            currentStage.topping.GetComponent<Animator>().Play(AnimatorParameters.P_TOPPINGANIM);
            Invoke("ExecNextStage", 2.0f);
        }
        else
        {
            IncreaseCakePartPosititon();
            PrepareCurrentPart();
        }
        StartCoroutine(uiManager.UpdateProgressBar((float)currentStage.currentPartIndex / currentStage.cakeParts.Count,
            currentStageIndex + 1, currentStageIndex + 2));
    }

    /*
     * METOD ADI :  ExecNextStage
     * AÇIKLAMA  :  Bir sonraki Stage'i başlatmadan önce pozisyonları sıfırlar ve Stage'i başlatır.
     */
    private void ExecNextStage()
    {
        Painter.Instance.TurnBack();
        ParticleManager.Instance.PlayFireworks();
        ResetPositions();
        GetNextStage();
    }

    /*
     * METOD ADI :  IncreaseCakePartPosititon
     * AÇIKLAMA  :  kek katmanının ve engel objelerin oluşacağı pozisyonu ayarlar
     */
    private void IncreaseCakePartPosititon()
    {
        obstaclePosition.y = currentStage.GetCurrentCakePart().transform.position.y - 0.1f;
        cakePosition += isCake ? cakePositionStepSize : creamPositionStepSize;
        isCake = !isCake;
    }

    /*
     * METOD ADI :  GetNextStage
     * AÇIKLAMA  :  Bir sonraki Stage'i başlatır.
     */
    private void GetNextStage()
    {
        uiManager.ShowMissionState((currentStageIndex + 1).ToString());
    }

    /*
     * METOD ADI :  ClearCurrentPartWithNearMiss
     * AÇIKLAMA  :  Nearmiss clear butonuna basıldığında geçerli olan partı tamamen boyar.
     */
    public void ClearCurrentPartWithNearMiss(bool ac)
    {
        currentStage.obstacle.SetActive(ac);
    }

    /*
     * METOD ADI :  PrepareNextStage
     * AÇIKLAMA  :  Bir sonraki Stage'i oynamaya hazırlar.
     */
    public void PrepareNextStage()
    {
        Painter.Instance.MissionStage = true;
        ParticleManager.Instance.StopFireworks();
        ResetPositions();
        currentStage.stage.SetActive(false);
        currentStage.ResetStage();

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,
            Application.version, currentStageIndex.ToString("0000"));

        currentStageIndex++;

        dataManager.data.bestScore =
            dataManager.data.bestScore < ScoreManager.Instance.GetScore() ?
            ScoreManager.Instance.GetScore() : dataManager.data.bestScore;

        dataManager.data.level = currentStageIndex;

        dataManager.Save();

        if (currentStageIndex >= stages.Count)
        {
            currentStageIndex = 0;
            ResetAllStages();
        }
        PrepareCurrentStage();
        uiManager.HideMissionState();
        StartCoroutine(uiManager.UpdateProgressBar((float)currentStage.currentPartIndex / currentStage.cakeParts.Count,
        currentStageIndex + 1, currentStageIndex + 2));
    }

    /*
     * METOD ADI :  ResetAllStages
     * AÇIKLAMA  :  Bütün Stageler tamamlandığında ilk Stage'e geri dönmeden önce bütün Stage'leri sıfırlar.
     */
    private void ResetAllStages()
    {
        ResetPositions();
        foreach (Stage stage in stages)
        {
            stage.ResetStage();
        }
    }

    /*
     * METOD ADI :  ResetPositions
     * AÇIKLAMA  :  Pozisyonları sıfırlar.
     */
    private void ResetPositions()
    {
        obstaclePosition = obstacleStartPosition;
        Shooter.Instance.ResetShootStartPosition();
    }
    #endregion
}
