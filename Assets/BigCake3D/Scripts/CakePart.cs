using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CakePart : MonoBehaviour
{
    #region Variables
    protected readonly float _speed = 2.5f;
    protected Vector3 _rotateScale = new Vector3(0.0f, -12.5f, 0.0f);
    protected List<Piece> _childsPieces = new List<Piece>();

    private Transform parentTransform = null;
    #endregion

    #region Builtin Methods
    private void Awake()
    {
        _rotateScale.y = -1.0f * (360.0f / transform.childCount);
        parentTransform = transform.parent;
        _childsPieces = GetComponentsInChildren<Piece>().ToList();
    }
    #endregion

    #region Custom Methods
    public bool IsPartCompelete()
    {
        Debug.Log(_childsPieces.Count);
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
        var targetRotation = Quaternion.Euler(parentTransform.eulerAngles + _rotateScale);
        for (float time = 0; time < 0.15f; time += _speed * Time.deltaTime)
        {
            parentTransform.rotation = Quaternion.Slerp(parentTransform.rotation, targetRotation, time);
            yield return null;
        }
        parentTransform.rotation = targetRotation;
    }

    public void ResetPart()
    {
        foreach (var piece in _childsPieces)
        {
            piece.PieceMeshRenderer.enabled = false;
        }
        /*if (this as Cream)
        {
            foreach (var piece in _childsPieces)
            {
                piece.PieceMeshRenderer.enabled = false;
            }
        }

        foreach (Piece piece in _childsPieces)
        {
            piece.SetUnColored();
        }*/
    }
    #endregion
}
