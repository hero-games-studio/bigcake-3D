using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Cake : MonoBehaviour
{
    private readonly float _speed = 2.5f;
    private Vector3 _rotateScale = new Vector3(0.0f, -30.0f, 0.0f);
    private List<Piece> _childsPieces = new List<Piece>();

    private void Awake()
    {
        _childsPieces = GetComponentsInChildren<Piece>().ToList();
    }

    public bool CheckChilds()
    {
        bool allColored = true;

        foreach (Piece piece in _childsPieces)
        {
            if (piece.State == PieceState.UnColored)
            {
                allColored = false;
                break;
            }
        }

        return allColored;
    }

    public IEnumerator RotateMe()
    {
        var targetRotation = Quaternion.Euler(transform.eulerAngles + _rotateScale);
        for (float time = 0; time < 0.15f; time += _speed * Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, time);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}
