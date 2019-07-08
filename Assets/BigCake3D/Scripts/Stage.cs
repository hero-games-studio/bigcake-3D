using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public GameObject stage;
    public GameObject obstacle;
    public GameObject topping;
    public int currentPartIndex;
    public List<CakePart> cakeParts;

    /*
     * METOD ADI :  CheckCurrentPart
     * AÇIKLAMA  :  geçerli cakePart'ın durumunu kontrol eder.
     */
    public bool CheckCurrentPart() => cakeParts[currentPartIndex].IsPartCompelete();

    /*
     * METOD ADI :  GetCurrentCakePart
     * AÇIKLAMA  :  Geçerli cakePart'ı döndürür.
     */
    public CakePart GetCurrentCakePart() => cakeParts[currentPartIndex] as CakePart;

    /*
     * METOD ADI :  ResetStage
     * AÇIKLAMA  :  Stage'i sıfırlar.
     */
    public void ResetStage()
    {
        foreach (CakePart cakePart in cakeParts)
        {
            cakePart.ResetPart();
            cakePart.gameObject.SetActive(false);
        }
        currentPartIndex = 0;
        topping.SetActive(false);
        stage.gameObject.SetActive(false);
    }
}