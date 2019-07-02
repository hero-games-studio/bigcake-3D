using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float _increaseRotationScale = 60.0f;

    private void Update()
    {
        if (!Painter.Instance.MissionStage)
        {
            RotateObstacles();
        }
    }

    private void RotateObstacles()
    {
        transform.Rotate(Vector3.up, _increaseRotationScale * Time.deltaTime);
    }
}
