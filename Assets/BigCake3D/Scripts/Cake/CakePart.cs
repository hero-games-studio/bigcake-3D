using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CakePart : MonoBehaviour
{
    #region Variables
    protected readonly float speed = 2.0f;
    protected Vector3 rotateScale = new Vector3(0.0f, -7.5f, 0.0f);
    protected List<Piece> childPieces = new List<Piece>();

    private Transform parentTransform = null;

    private int childIndex = 0;
    #endregion

    #region Builtin Methods
    private void Awake()
    {

        parentTransform = transform.parent;
        childPieces = GetComponentsInChildren<Piece>().ToList();
    }
    #endregion

    #region Custom Methods
    /*
     * METOD ADI :  IsPartCompelete
     * AÇIKLAMA  :  Objenin bütün alt objelerinin boyanıp boyanmadığını
     *              kontrol eder.
     */
    public bool IsPartCompelete()
    {
        bool allColored = true;

        foreach (Piece piece in childPieces)
        {
            if (piece.State == PieceState.UnColored)
            {
                allColored = false;
                break;
            }
        }

        return allColored;
    }

    public void PaintPieces()
    {
        if (childIndex >= childPieces.Count)
        {
            childIndex = 0;
        }
        childPieces[childIndex++].SetColored();
    }

    /*
     * METOD ADI :  RotateMe
     * AÇIKLAMA  :  objenin yavaş bir şekilde döndürülme işlemini yapar.
     */
    public void RotateMe() =>
        parentTransform.DOLocalRotate(rotateScale, -.75f, RotateMode.LocalAxisAdd);


    /*
     * METOD ADI :  ResetPart
     * AÇIKLAMA  :  Objeyi sıfırlama işlemini yapar.
     */
    public void ResetPart()
    {
        foreach (var piece in childPieces)
        {
            piece.SetUnColored();
        }
    }

    public void ShowFirstPiece() => childPieces[0].SetColored();
    #endregion
}
