using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject shooterObj = null;
    private Vector3 offset;

    private Transform transformInit = null;
    private Animator animator = null;

    private int cakePartCount = 0;
    private List<CakePart> cakeParts = null;
    private float rotateYScale = 0.0f;
    private float duration = 1.0f;
    private float time = 0.125f;
    #endregion

    #region Builtin Methods
    private void Start()
    {
        animator = GetComponent<Animator>();
        transformInit = transform;
        offset = transform.position - shooterObj.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = shooterObj.transform.position + offset;
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
        transform.position = new Vector3(transform.position.x, cakeParts[0].transform.position.y, transform.position.z); 
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
