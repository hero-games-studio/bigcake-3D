using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    private Color _obstacleColor = Color.black;

    private Cake _cake = null;

    private void Awake()
    {
        _cake = FindObjectOfType<Cake>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_OBSTACLE)
        {
            gameObject.SetActive(false);
            var material = other.GetComponent<MeshRenderer>().material;
            material.color = _obstacleColor;
            GameManager.GetInstance().AddScore();
            _cake.RotateMe();
        }
    }
}
