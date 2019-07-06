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
    public bool CheckCurrentPart() => cakeParts[currentPartIndex].IsPartCompelete();
    public CakePart GetCurrentCakePart() => cakeParts[currentPartIndex] as CakePart;

    public void ResetStage()
    {
        foreach (CakePart cakePart in cakeParts)
        {
            cakePart.ResetPart();
            cakePart.gameObject.SetActive(false);
        }
        currentPartIndex = 0;
        stage.gameObject.SetActive(false);
    }
}