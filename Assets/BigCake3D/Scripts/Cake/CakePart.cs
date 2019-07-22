using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CakePart : MonoBehaviour
{
    #region Variables
    protected Vector3 rotateScale = new Vector3(0.0f, -12.0f, 0.0f);
    protected List<Piece> childPieces = new List<Piece>();

    private Transform parentTransform = null;

    private int childIndex = 0;

    private bool canRotate = true;

    #endregion

    #region Builtin Methods
    private void Awake()
    {
        parentTransform = transform.parent;
        childPieces = GetComponentsInChildren<Piece>().ToList();
        rotateScale.y = -360.0f / childPieces.Count;
        for (int i = 0; i < childPieces.Count; i++)
        {
            childPieces[i].index = i;
        }
    }
    #endregion

    #region Custom Methods
    public Material GetPieceColor() =>
        GetComponentsInChildren<Piece>()[0].GetComponent<Renderer>().material;

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

    /*
     * METOD ADI :  PaintPieces
     * AÇIKLAMA  :  Geçerli indexteki piece'nin boyanma işlemini başlatır.
     */
    public void PaintPieces()
    {
        if (childIndex >= childPieces.Count)
        {
            childIndex = 0;
        }

        if (canRotate)
        {
            canRotate = false;
            childPieces[childIndex++].SetColored();
        }
    }

    /*
     * METOD ADI :  RotateMe
     * AÇIKLAMA  :  objenin yavaş bir şekilde döndürülme işlemini yapar.
     */
    public void RotateMe()
    {
        parentTransform.DORotate(rotateScale, OtherData.duration, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            canRotate = true;
        });
    }

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

    public void ResetRotation()
    {
        int count = GetComponentsInChildren<Piece>().Length;
        transform.parent.localRotation = Quaternion.Euler(
            0.0f,
                count >= 32 ? (360.0f / count) * 0.85f :
                count == 24 ? (360.0f / count) * 1.15f :
                (360.0f / count) * 1.5f,
            0.0f);
    }
    #endregion
}
