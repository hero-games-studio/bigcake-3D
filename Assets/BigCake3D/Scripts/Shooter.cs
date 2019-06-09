using UnityEngine;

public class Shooter : MonoSingleton<Shooter>
{
    #region Shooter
    [SerializeField]
    private Vector3 _shootStartPositionDefault = new Vector3(0, -0.25f, -3.5f);

    [SerializeField]
    private Vector3 _shootStartPosition = new Vector3(0, -0.25f, -3.5f);

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

    public void ChangePosition()
    {
        transform.position = ShootStartPosition;
    }
    #endregion
}
