using UnityEngine;

public class CameraLook : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform shooterTransform = null;
    private Vector3 distance;
    private bool finished = false;
    #endregion

    #region Builtin Methods
    private void Awake()
    {
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
    #endregion
}
