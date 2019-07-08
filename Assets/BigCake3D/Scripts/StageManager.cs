using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    #region Variables
    public List<Stage> stages;
    private int currentStageIndex = 0;

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
    #endregion

    #region Methods 

    private void Awake()
    {
        PrepareCurrentStage();
        uiManager.UpdateProgressBar(0,
            currentStageIndex + 1, currentStageIndex + 2);
    }

    /*
     * METOD ADI :  FallDown
     * AÇIKLAMA  :  Parametre olarak gönderilen objenin pozisyonunu
     *              parametre olarak target değerine eşitler.
     */
    private IEnumerator FallDown(Transform tr, Vector3 target, bool obstacle, bool topping = false)
    {
        fallingDown = true;

        var pos = obstacle ? tr.position : topping ?
            new Vector3(tr.position.x, 50.0f, tr.position.z) : new Vector3(tr.position.x, 6.0f, tr.position.z);
        tr.gameObject.SetActive(true);
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime)
        {
            tr.position = Vector3.Lerp(pos, target, time);
            yield return null;
        }
        tr.position = target;

        if (!obstacle)
        {
            Shooter.Instance.GoOneStepUp();
        }
        fallingDown = false;
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
        if (Painter.Instance.nearMiss)
        {
            Painter.Instance.nearMiss = false;
            currentStage.obstacle.SetActive(true);
        }
        StartCoroutine(FallDown(currentStage.obstacle.transform, obstaclePosition, true));
        currentStage.GetCurrentCakePart().gameObject.SetActive(true);
        if (currentStage.GetCurrentCakePart() as Cream)
        {
            foreach (Piece piece in
                currentStage.GetCurrentCakePart().GetComponentsInChildren<Piece>())
            {
                piece.PieceMeshRenderer.enabled = false;
            }
        }
        StartCoroutine(FallDown(currentStage.GetCurrentCakePart().transform,
            currentStage.GetCurrentCakePart().transform.position, false));
    }

    /*
     * METOD ADI :  RotateAndCheckCakePart
     * AÇIKLAMA  :  Objeyi döndürüp, tamamlama durumunu kotrol eder.
     */
    public void RotateAndCheckCakePart()
    {
        if (currentStage.GetCurrentCakePart() != null)
        {
            StartCoroutine(currentStage.GetCurrentCakePart().RotateMe());
        }

        if (currentStage.GetCurrentCakePart().IsPartCompelete())
        {
            Painter.Instance.TurnBack();
            Painter.Instance.MissionStage = false;

            GetNextPart();
        }
    }

    /*
     * METOD ADI :  ResetCurrentPart
     * AÇIKLAMA  :  Geçerli partı sıfırlar.
     */
    public void ResetCurrentPart()
    {
        currentStage.GetCurrentCakePart().ResetPart();
        StopAllCoroutines();
        Painter.Instance.TurnBack();
    }

    /*
     * METOD ADI :  GetNextPart
     * AÇIKLAMA  :  Geçerli part tamamlandıktan sonra, bir sonraki partı boyamaya hazırlar.
     *              Stage tamamlanmış ise bir sonraki Stage'ı başlatır.
     */
    private void GetNextPart()
    {
        ParticleManager.Instance.PlayStarRing(currentStage.GetCurrentCakePart().transform.position);
        currentStage.currentPartIndex++;
        if (currentStage.currentPartIndex >= currentStage.cakeParts.Count)
        {
            currentStage.topping.SetActive(true);
            FallDown(currentStage.topping.transform, currentStage.topping.transform.position, false, true);
            Invoke("ExecNextStage", 1.5f);
        }
        else
        {
            IncreaseCakePartPosititon();
            PrepareCurrentPart();
        }
        uiManager.UpdateProgressBar((float)currentStage.currentPartIndex / currentStage.cakeParts.Count,
            currentStageIndex + 1, currentStageIndex + 2);
    }

    /*
     * METOD ADI :  ExecNextStage
     * AÇIKLAMA  :  Bir sonraki Stage'i başlatmadan önce pozisyonları sıfırlar ve Stage'i başlatır.
     */
    private void ExecNextStage()
    {
        ResetPositions();
        GetNextStage();
    }

    /*
     * METOD ADI :  IncreaseCakePartPosititon
     * AÇIKLAMA  :  kek katmanının ve engel objelerin oluşacağı pozisyonu ayarlar
     */
    private void IncreaseCakePartPosititon()
    {
        obstaclePosition.y = currentStage.GetCurrentCakePart().transform.position.y + 0.1f;
        cakePosition += isCake ? cakePositionStepSize : creamPositionStepSize;
        isCake = !isCake;
    }

    /*
     * METOD ADI :  GetNextStage
     * AÇIKLAMA  :  Bir sonraki Stage'i başlatır.
     */
    private void GetNextStage()
    {
        ParticleManager.Instance.PlayFireworks();
        Painter.Instance.MissionStage = true;
        uiManager.ShowMissionState((currentStageIndex + 1).ToString());
    }

    /*
     * METOD ADI :  ClearCurrentPartWithNearMiss
     * AÇIKLAMA  :  Nearmiss clear butonuna basıldığında geçerli olan partı tamamen boyar.
     */
    public IEnumerator ClearCurrentPartWithNearMiss()
    {
        currentStage.obstacle.SetActive(false);
        Shooter.Instance.StartSqueeze();
        yield return delayNearMiss;
    }

    /*
     * METOD ADI :  PrepareNextStage
     * AÇIKLAMA  :  Bir sonraki Stage'i oynamaya hazırlar.
     */
    public void PrepareNextStage()
    {
        ParticleManager.Instance.StopFireworks();
        ResetPositions();
        currentStage.stage.SetActive(false);
        currentStage.ResetStage();
        currentStageIndex++;
        if (currentStageIndex >= stages.Count)
        {
            currentStageIndex = 0;
            ResetAllStages();
        }
        PrepareCurrentStage();
        uiManager.HideMissionState();
        uiManager.UpdateProgressBar((float)currentStage.currentPartIndex / currentStage.cakeParts.Count,
     currentStageIndex + 1, currentStageIndex + 2);
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
