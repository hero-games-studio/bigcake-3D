using System.Collections;
using UnityEngine;

public class Shooter : MonoSingleton<Shooter>
{
    #region Shooter
    [SerializeField]
    private Vector3 _shootStartPositionDefault = new Vector3(0, -0.375f, -1.5f);//new Vector3(0, -0.3f, -3.5f);

    [SerializeField]
    private Vector3 _shootStartPosition = new Vector3(0, -0.375f, -1.5f); //new Vector3(0, -0.3f, -3.5f);

    private float _multiple = 5.0f;

    public Vector3 ShootStartPosition
    {
        get
        {
            return _shootStartPosition;
        }
        set
        {
            _shootStartPosition = value;
        }
    }

    public void ResetShootStartPosition()
    {
        _shootStartPosition = _shootStartPositionDefault;
    }

    public IEnumerator ChangePosition(Vector3 targetPosition)
    {
        var position = transform.position;
        for (float timer = 0.0f; timer < 1.0f; timer += Time.deltaTime * _multiple)
        {
            transform.position = Vector3.Lerp(position, targetPosition, timer);
            yield return null;
        }
        transform.position = targetPosition;
    }
    #endregion
}
