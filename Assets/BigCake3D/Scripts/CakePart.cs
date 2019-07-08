using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CakePart : MonoBehaviour
{
    #region Variables
    protected readonly float speed = 2.0f;
    protected Vector3 rotateScale = new Vector3(0.0f, -11.5f, 0.0f);
    protected List<Piece> childsPieces = new List<Piece>();

    private Transform parentTransform = null;
    #endregion

    #region Builtin Methods
    private void Awake()
    {
        rotateScale.y = this as Cake ? -15f : -20.0f;
        parentTransform = transform.parent;
        childsPieces = GetComponentsInChildren<Piece>().ToList();
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

        foreach (Piece piece in childsPieces)
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
     * METOD ADI :  RotateMe
     * AÇIKLAMA  :  objenin yavaş bir şekilde döndürülme işlemini yapar.
     */
    public IEnumerator RotateMe()
    {
        var targetRotation = Quaternion.Euler(parentTransform.eulerAngles + rotateScale);
        for (float time = 0; time < 0.15f; time += speed * Time.deltaTime)
        {
            parentTransform.rotation = Quaternion.Slerp(parentTransform.rotation, targetRotation, time);
            yield return null;
        }
        parentTransform.rotation = targetRotation;
    }

    /*
     * METOD ADI :  ResetPart
     * AÇIKLAMA  :  Objeyi sıfırlama işlemini yapar.
     */
    public void ResetPart()
    {
        foreach (var piece in childsPieces)
        {
            piece.SetUnColored();
        }
    }
    #endregion
}
