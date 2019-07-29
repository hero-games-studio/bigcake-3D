using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform shooterTransform = null;
    private Vector3 distance;
    private bool finished = false;

    [SerializeField] private new Camera camera = null;
    private Camera cameraInit = null;

    private Transform transformInit = null;
    private Animator animator = null;

    private int cakePartCount = 0;
    private List<CakePart> cakeParts = null;
    private float rotateYScale = 0.0f;
    private float duration = 1.0f;
    #endregion

    #region Builtin Methods
    private void Awake()
    {
        animator = GetComponent<Animator>();
        cameraInit = camera;
        transformInit = transform;
        distance = transform.position - shooterTransform.position;
    }

    private void LateUpdate()
    {
        if (!finished)
        {
            transform.position =
                Vector3.Lerp(transform.position, shooterTransform.position + distance, Time.deltaTime);
        }
    }
    #endregion

    #region Custom Methods
    public void Look()
    {
        cakeParts = StageManager.Instance.currentStage.cakeParts;
        cakePartCount = cakeParts.Count;
        rotateYScale = 360.0f / cakePartCount;       
        StartCoroutine(LookAndRotate());
    }

    private IEnumerator LookAndRotate()
    {
        Painter.Instance.MissionStage = true;
        animator.SetTrigger(AnimatorParameters.P_LOOKCAKE);
        for (int i = 0; i < cakePartCount; i++)
        {
            Vector3 pos =
                new Vector3(transform.position.x, cakeParts[i].transform.position.y, transform.position.z);
            transform.DOMove(pos, duration);
            yield return null;
        }

        transform.DOMove(transformInit.position, duration);

        StageManager.Instance.ExecNextStage();
    }

    public void ShowNextStage() => StageManager.Instance.GetNextStage();
    #endregion
}
