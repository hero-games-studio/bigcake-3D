using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Vector3 _increaseRotationScale = new Vector3(0.0f, 2.0f, 0.0f);

    private void LateUpdate()
    {
        StartCoroutine(RotateTheObstacles());
    }

    private IEnumerator RotateTheObstacles()
    {
        for (float time = 0; time < 1.0f; time += Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.eulerAngles + _increaseRotationScale), time * 0.1f);
            yield return null;
        }
    }
}
