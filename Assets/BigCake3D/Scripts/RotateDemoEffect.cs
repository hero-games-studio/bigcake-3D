using System.Collections;
using UnityEngine;

public class RotateDemoEffect : MonoBehaviour
{
    private Vector3 rotate = new Vector3(0, 15, 0);

    public void StartShineDemo() => StartCoroutine(ActiveAndRotate());

    private IEnumerator ActiveAndRotate()
    {
        for (int i = 0; i < 24; i++)
        {
            transform.Rotate(rotate);
            yield return null;
        }
    }
}
