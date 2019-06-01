using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float _increaseRotationScale = 60.0f;

    private void Update()
    {
        RotateObstacles();
    }

    private void RotateObstacles()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, _increaseRotationScale * Time.deltaTime);
    }
}
