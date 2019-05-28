using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cake : MonoBehaviour
{
    private Vector3 _roateScale = new Vector3(0.0f, -30.0f, 0.0f);
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

    public void RotateMe()
    {
        transform.Rotate(_roateScale);
    }
}
