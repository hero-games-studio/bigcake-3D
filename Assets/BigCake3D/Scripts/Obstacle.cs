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
    /*
     * METOD ADI :  RotateObstacles
     * AÇIKLAMA  :  Engelleri pasta etrafında döndürür.
     */
    private void RotateObstacles()
    {
        transform.Rotate(Vector3.up, (_increaseRotationScale * Time.deltaTime) +
            ( (StageManager.Instance.currentStageIndex+1) *
            StageManager.Instance.currentStage.currentPartIndex+1 >= 5 ?
            5 : StageManager.Instance.currentStage.currentPartIndex + 1) * Time.deltaTime);
    }
}
