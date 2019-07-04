using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private bool isCollideWithPiece = false;
    private float boundTime = 0.2f;
    private float previousTime = 0.0f;

    private Piece piece;
    private void Update()
    {
        if (isCollideWithPiece && Painter.Instance.isPainting)
        {
            if (Time.time - previousTime > boundTime)
            {
                previousTime = Time.time;
                piece.PieceMeshRenderer.enabled = true;
                piece.SetColored();
                StageManager.Instance.RotateAndCheckCakePart();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_PIECE)
        {
            piece = other.GetComponent<Piece>();

            isCollideWithPiece = true;
            /*if (piece.State == PieceState.UnColored)
            {
                if (other.GetComponentInParent<Cream>() != null)
                {*/
                                         /* }*/

            //}
        }

        if (other.tag == Tags.T_OBSTACLE)
        {
            isCollideWithPiece = false;
            ScoreManager.Instance.ResetNearMiss();
            StageManager.Instance.ResetCurrentPart();
        }

        if (other.tag == Tags.T_NEARMISS)
        {
            ScoreManager.Instance.AddNearMiss();
        }
    }
}
