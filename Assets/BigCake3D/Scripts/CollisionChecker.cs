using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_OBSTACLE)
        {
            ScoreManager.Instance.ResetScoreAndNearMiss();
            StageManager.Instance.ResetCurrentPart();
        }

        if (other.tag == Tags.T_NEARMISS)
        {
            ScoreManager.Instance.AddNearMiss();
        }
    }
}
