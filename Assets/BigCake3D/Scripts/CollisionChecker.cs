using UnityEngine;

public class CollisionChecker : MonoBehaviour
{  
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_PIECE)
        {
            gameObject.SetActive(false);
            other.GetComponent<Piece>().SetColored();
            Painter.Instance.RotateAndCheck();
        }

        if (other.tag == Tags.T_OBSTACLE)
        {
            SceneController.RestartLevel();
        }

        if (other.tag == Tags.T_BOUNDARY)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            gameObject.SetActive(false);
        }
    }
}
