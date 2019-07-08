using System.Collections;
using UnityEngine;

public class RotateDemoEffect : MonoBehaviour
{
    private Vector3 rotate = new Vector3(0, 15, 0);

    /*
     * METOD ADI :  StartShineDemo
     * AÇIKLAMA  :  Demo Star shine effectini oynatma işlemini başlatır.
     */
    public void StartShineDemo() => StartCoroutine(ActiveAndRotate());

    /*
     * METOD ADI :  ActiveAndRotate
     * AÇIKLAMA  :  Demo Star shine effectini kendi etrafında döndürür.
     */
    private IEnumerator ActiveAndRotate()
    {
        for (int i = 0; i < 24; i++)
        {
            transform.Rotate(rotate);
            yield return null;
        }
    }
}
