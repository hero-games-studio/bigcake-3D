using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private bool _nearMiss = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_PIECE)
        {
            other.GetComponent<Piece>().SetColored();
            Painter.Instance.RotateAndCheck();
            ResetDatas();
        }

        if (other.tag == Tags.T_OBSTACLE)
        {
            if (!_nearMiss)
            {
                ResetDatas();
                FindObjectOfType<StageManager>().ResetCurrentStage();
            }
        }

        if (other.tag == Tags.T_BOUNDARY)
        {
            ResetDatas();
        }

        if (other.tag == Tags.T_NEARMISS)
        {
            _nearMiss = true;
            ScoreManager.Instance.AddNearMiss();
        }
    }

    private void ResetDatas()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }
}
