using UnityEngine;

public class CollisionChecker : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_PIECE)
        {
            Piece piece = other.GetComponentInParent<Piece>();
            if (piece.State == PieceState.UnColored)
            {
                if (piece.GetComponentInParent<Cream>() != null)
                {
                    other.GetComponent<Renderer>().enabled = true;
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
