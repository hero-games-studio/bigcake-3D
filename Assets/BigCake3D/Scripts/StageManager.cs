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
    [SerializeField] private Vector3 cakeStartPosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 currentCakePosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 cakePositionStepSize = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 obstaclePosition = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 obstacleStartPosition = new Vector3(0.0f, 0.0f, 0.0f);
    #endregion

    #region Methods 

    private void Awake()
    {
        PrepareCurrentStage();
    }

    private IEnumerator FallDown(Transform tr, Vector3 target, bool obstacle)
    {
        fallingDown = true;
        var pos = obstacle ? tr.position : new Vector3(tr.position.x, 6.0f, tr.position.z);
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime)
        {
            tr.position = Vector3.Lerp(pos, target, time);
            yield return null;
        }
        tr.position = target;
        fallingDown = false;
    }

    private void PrepareCurrentStage()
    {
        currentStage = stages[currentStageIndex];
        currentStage.stage.SetActive(true);
        PrepareCurrentPart();
    }

    private void PrepareCurrentPart()
    {
        Shooter.Instance.GoOneStepUp();
        StartCoroutine(FallDown(currentStage.obstacle.transform, obstaclePosition, true));
        currentStage.GetCurrentCakePart().gameObject.SetActive(true);
        StartCoroutine(FallDown(currentStage.GetCurrentCakePart().transform, currentCakePosition, false));
    }

    public void RotateAndCheckCakePart()
    {
        if (currentStage.GetCurrentCakePart() != null)
        {
            StartCoroutine(currentStage.GetCurrentCakePart().RotateMe());
        }
        if (currentStage.GetCurrentCakePart().IsPartCompelete())
        {
            GetNextPart();
        }
    }

    public void ResetCurrentPart()
    {
        currentStage.GetCurrentCakePart().ResetPart();
    }

    private void GetNextPart()
    {
        currentStage.currentPartIndex++;
        if (currentStage.currentPartIndex >= currentStage.cakeParts.Count)
        {
            ResetPositions();
            GetNextStage();
        }
        else
        {
            IncreaseCakePartPosititon();
            PrepareCurrentPart();
        }
    }

    private void IncreaseCakePartPosititon()
    {
        currentCakePosition += isCake ? cakePositionStepSize : cakePositionStepSize * 0.5f;
        obstaclePosition += isCake ? cakePositionStepSize : cakePositionStepSize * 0.5f;
        isCake = !isCake;
    }

    private void GetNextStage()
    {
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
    }

    private void ResetAllStages()
    {
        ResetPositions();
        foreach (Stage stage in stages)
        {
            stage.ResetStage();
        }
    }

    private void ResetPositions()
    {
        obstaclePosition = obstacleStartPosition;
        currentCakePosition = cakeStartPosition;
    }
    #endregion
}
