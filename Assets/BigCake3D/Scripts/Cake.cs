using UnityEngine;

public class Cake : MonoBehaviour
{
    public void RotateMe()
    {
        transform.Rotate(new Vector3(0, -30, 0));
    }
}
