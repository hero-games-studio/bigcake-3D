using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    #region Variables
    public List<Stage> stages;

    [SerializeField]
    private Vector3 fallDownTargetPosition = new Vector3(0.0f, -0.4f, 0.0f);

    [SerializeField]
    private Vector3 fallDownDefaultTargetPosition = new Vector3(0.0f, -0.4f, 0.0f);

    [SerializeField]
    private Vector3 fallDownIncreaseTargetPosition = new Vector3(0.0f, 0.2f, 0.0f);

    private Stage currentStage = null;
    private int currentStageIndex = 0;
    #endregion

    #region Methods 
    private void Awake()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        currentStageIndex = 0;
        currentStage = stages[currentStageIndex];
        currentStage.stage.gameObject.SetActive(true);
        currentStage.GetCurrentCakeLayer().cake.gameObject.SetActive(true);
    }

    public void CheckCurrentLayer()
    {
        if (currentStage.IsLayerComplete())
        {
            GetNextLayer();
        }
        else
        {
            if (currentStage.GetCurrentCakeLayer().cake.IsPartCompelete())
            {
                if (currentStage.GetCurrentCakeLayer().cream != null)
                {
                    currentStage.GetCurrentCakeLayer().cream.gameObject.SetActive(true);
                }
            }
            else
            {
                currentStage.GetCurrentCakeLayer().cake.gameObject.SetActive(true);
            }
        }
    }

    private void GetNextLayer()
    {
        currentStage.currentLayerIndex++;
        if (currentStage.currentLayerIndex >= currentStage.layers.Count)
        {
            GetNextStage();
        }
        else
        {
            PrepareLayer();
        }
    }

    private void PrepareLayer()
    {
        currentStage.GetCurrentCakePart().gameObject.SetActive(true);
    }

    private void GetNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex >= stages.Count)
        {
            currentStageIndex = 0;
        }
        currentStage = stages[currentStageIndex];
        PrepareLayer();
    }

    private IEnumerator FallDown(Transform tr, Vector3 target)
    {
        var pos = tr.position;
        for (float time = 0.0f; time < 1.0f; time += Time.deltaTime)
        {
            tr.position = Vector3.Lerp(pos, target, time);
            yield return null;
        }
        tr.position = target;
    }

    public void RotateCakePart()
    {
        if (currentStage.GetCurrentCakePart() != null)
        {
            StartCoroutine(currentStage.GetCurrentCakePart().RotateMe());
        }
        CheckLayerState();
    }

    public void ResetCurrentPart()
    {
        currentStage.GetCurrentCakeLayer().ResetCurrentPart();
    }

    public bool CheckLayerState() => currentStage.GetCurrentCakeLayer().CheckState();
    #endregion
}
