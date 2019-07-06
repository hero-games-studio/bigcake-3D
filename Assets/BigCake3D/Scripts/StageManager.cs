﻿using System.Collections;
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
    [SerializeField] private RotateDemoEffect rotateDemoEffect = null;

    private WaitForSeconds delayNearMiss = new WaitForSeconds(0.75f);
    #endregion

    #region Methods 

    private void Awake()
    {
        PrepareCurrentStage();
        uiManager.UpdatePregressBar(0,
            currentStageIndex + 1, currentStageIndex + 2);
    }

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

    private void PrepareCurrentStage()
    {
        currentStage = stages[currentStageIndex];
        currentStage.stage.SetActive(true);
        PrepareCurrentPart();
    }

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

    public void ResetCurrentPart()
    {
        currentStage.GetCurrentCakePart().ResetPart();
        StopAllCoroutines();
        Painter.Instance.TurnBack();
    }

    private void GetNextPart()
    {
        rotateDemoEffect.transform.position = currentStage.GetCurrentCakePart().transform.position;
        rotateDemoEffect.StartShineDemo();
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
        uiManager.UpdatePregressBar((float)currentStage.currentPartIndex / currentStage.cakeParts.Count,
            currentStageIndex + 1, currentStageIndex + 2);
    }

    private void ExecNextStage()
    {
        ResetPositions();
        GetNextStage();
    }

    private void IncreaseCakePartPosititon()
    {
        obstaclePosition.y = currentStage.GetCurrentCakePart().transform.position.y + 0.1f;
        cakePosition += isCake ? cakePositionStepSize : creamPositionStepSize;
        isCake = !isCake;
    }

    private void GetNextStage()
    {
        ParticleManager.Instance.PlayFireworks();
        Painter.Instance.MissionStage = true;
        uiManager.ShowMissionState((currentStageIndex + 1).ToString());
    }

    public IEnumerator ClearCurrentPartWithNearMiss()
    {
        currentStage.obstacle.SetActive(false);
        Shooter.Instance.StartSqueeze();
        yield return delayNearMiss;
    }

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
        uiManager.UpdatePregressBar((float)currentStage.currentPartIndex / currentStage.cakeParts.Count,
     currentStageIndex + 1, currentStageIndex + 2);
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
        Shooter.Instance.ResetShootStartPosition();
    }
    #endregion
}
