using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Painter.Instance.isPainting)
        {
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
}
