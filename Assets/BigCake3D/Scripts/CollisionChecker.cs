using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    //[SerializeField]
    private Color32 _obstacleColor = new Color32(123, 63, 0, 255);
    private Painter _painter = null;

    private void Awake()
    {
        _painter = FindObjectOfType<Painter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_OBSTACLE)
        {
            gameObject.SetActive(false);
            other.GetComponent<MeshRenderer>().material.color = _obstacleColor;
            other.GetComponent<Piece>().State = PieceState.Colored;
            GameManager.GetInstance().AddScore();
            _painter.RotateAndCheckCake();
        }
    }
}
