using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_PIECE)
        {
            Piece piece = other.GetComponent<Piece>();
            if (piece.State == PieceState.UnColored)
            {
                if (other.GetComponentInParent<Cream>() != null)
                {
                    piece.PieceMeshRenderer.enabled = true;
                }
                piece.SetColored();
                StageManager.Instance.RotateAndCheckCakePart();
            }
        }

        if (other.tag == Tags.T_OBSTACLE)
        {
            ScoreManager.Instance.ResetNearMiss();
            StageManager.Instance.ResetCurrentPart();
        }

        if (other.tag == Tags.T_NEARMISS)
        {
            ScoreManager.Instance.AddNearMiss();
        }
    }
}
