using UnityEngine;

public class PaintCollisionChecker : MonoBehaviour
{
    #region Variables
    private Piece piece = null;

    private float boundTime = 0.05f;
    private float previousTime = -0.05f;
    #endregion

    #region Builtin Methods

    private void Update()
    {
        if (Painter.Instance.isPainting)
        {
            if (Time.time - previousTime > boundTime)
            {
                previousTime = Time.time;
                piece?.SetColored();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_PIECE)
        {
            piece = other.GetComponent<Piece>();
        }
    }

    #endregion
}
