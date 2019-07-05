using System.Collections;
using UnityEngine;

public class Shooter : MonoSingleton<Shooter>
{
    #region Shooter
    [SerializeField]
    private GameObject creamSqueezeModel = null;

    [SerializeField]
    private Vector3 _shootStartPositionDefault = new Vector3(0, -0.375f, -1.5f);

    [SerializeField]
    private Vector3 _shootStartPosition = new Vector3(0, -0.375f, -1.5f);

    private float _multiple = 5.0f;

    [SerializeField]
    private Animator creamSqueezeAnimator = null;

    public void ResetShootStartPosition()
    {
        _shootStartPosition = _shootStartPositionDefault;
    }

    public IEnumerator ChangePosition(Vector3 targetPosition, bool squeeze)
    {
        if (!squeeze)
        {
            StopSqueeze();
        }

        var position = transform.position;
        targetPosition.y = position.y;
        for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * _multiple)
        {
            transform.position = Vector3.Lerp(position, targetPosition, timer);
            yield return null;
        }
        transform.position = targetPosition;

        if (squeeze)
        {
            StartSqueeze();
        }
    }

    public void StartSqueeze()
    {
        creamSqueezeModel.SetActive(true);
        creamSqueezeAnimator.SetBool(AnimatorParameters.P_ISSQUEEZE, true);
        Invoke("StartPainting", 0.3f);
    }

    private void StartPainting()
    {
        Painter.Instance.isPainting = true;
    }

    public void StopSqueeze()
    {
        Painter.Instance.isPainting = false;
        creamSqueezeModel.SetActive(false);
        creamSqueezeAnimator.SetBool(AnimatorParameters.P_ISSQUEEZE, false);
    }

    public void GoOneStepUp()
    {
        _shootStartPosition = new Vector3(_shootStartPosition.x,
            StageManager.Instance.currentStage.GetCurrentCakePart().transform.position.y + 0.05f,
            _shootStartPosition.z);

        transform.position = _shootStartPosition;
    }
    #endregion
}
