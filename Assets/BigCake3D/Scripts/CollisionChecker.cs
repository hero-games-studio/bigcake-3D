using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    //private Piece piece = null;
    //private bool canPaint = false;
    //private float boundTime = 0.1f;
    //private float previousTime = 0.0f;

    //private void Update()
    //{
    //    if (Painter.Instance.isPainting && canPaint)
    //    {
    //        if (Time.time - previousTime > boundTime)
    //        {
    //            piece?.SetColored();
    //            previousTime = Time.time;
    //            canPaint = false;
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == Tags.T_PIECE)
        //{
        //    canPaint = true;
        //    piece = other.GetComponent<Piece>();
        //}

        if (other.tag == Tags.T_OBSTACLE)
        {
            ScoreManager.Instance.ResetScoreAndNearMiss();
            StageManager.Instance.ResetCurrentPart();
            Painter.Instance.fail = true;
        }

        if (other.tag == Tags.T_NEARMISS)
        {
            ScoreManager.Instance.AddNearMiss();
        }
    }
}
