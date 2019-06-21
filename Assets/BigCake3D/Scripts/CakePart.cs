using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;

public class CakePart : MonoBehaviour
{
    #region Variables
    protected readonly float _speed = 2.5f;
    protected Vector3 _rotateScale = new Vector3(0.0f, -20.0f, 0.0f);
    protected List<Piece> _childsPieces = new List<Piece>();
    #endregion

    #region Builtin Methods
    private void Awake()
    {
        _childsPieces = GetComponentsInChildren<Piece>().ToList();
    }
    #endregion

    #region Custom Methods
    public bool IsPartCompelete()
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

    public void ResetPart()
    {
        foreach (Piece piece in _childsPieces)
        {
            piece.SetUnColored();
        }
    }
    #endregion
}
