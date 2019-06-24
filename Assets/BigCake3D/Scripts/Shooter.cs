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

    [SerializeField]
    private Vector3 creamSqueezeStartPosition = new Vector3(0, -0.375f, -1.5f);

    [SerializeField]
    private Vector3 creamSqueezePositionStepSize = new Vector3(0, -0.083f, 0);

    private float _multiple = 5.0f;
    private Animator creamSqueezeAnimator = null;

    private void Awake()
    {
        creamSqueezeModel.SetActive(false);
        creamSqueezeAnimator = creamSqueezeModel.GetComponentInChildren<Animator>();
    }

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
        creamSqueezeModel.transform.position = creamSqueezeStartPosition;
        creamSqueezeAnimator.SetBool(AnimatorParameters.P_ISSQUEEZE, true);
    }

    public void StopSqueeze()
    {
        creamSqueezeModel.SetActive(false);
        creamSqueezeAnimator.SetBool(AnimatorParameters.P_ISSQUEEZE, false);
    }

    public void GoOneStepUp()
    {
        _shootStartPosition = new Vector3(_shootStartPosition.x,
            StageManager.Instance.currentStage.GetCurrentCakePart().transform.position.y,
            _shootStartPosition.z);

        transform.position = _shootStartPosition;
    }

    public void IncreaseSqueezePosition() =>
        creamSqueezeStartPosition.y += creamSqueezePositionStepSize.y;

    public void ResetSqueezePosition() =>
        creamSqueezeStartPosition = new Vector3(0, 0, 0);
    #endregion
}
