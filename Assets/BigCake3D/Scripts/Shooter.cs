using UnityEngine;

public class Shooter : MonoSingleton<Shooter>
{
    #region Shooter
    private Vector3 _shootStartPositionDefault = new Vector3(0, -0.25f, -4.0f);
    private Vector3 _shootStartPosition = new Vector3(0, -0.25f, -4.0f);

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
