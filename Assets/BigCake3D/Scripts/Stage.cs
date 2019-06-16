using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public GameObject stage;

    public int currentLayerIndex;
    public List<CakeLayer> layers;

    public CakeLayer GetCurrentCakeLayer() => layers[currentLayerIndex];

    public CakePart GetCurrentCakePart()
    {
        return (GetCurrentCakeLayer().cream != null && GetCurrentCakeLayer().cake.IsPartCompelete()) ?
            GetCurrentCakeLayer().cream : GetCurrentCakeLayer().cake as CakePart;
    }

    public bool IsLayerComplete()
    {
        if (!GetCurrentCakeLayer().cake.IsPartCompelete())
        {
            return false;
        }
        else
        {
            if (GetCurrentCakeLayer().cream != null)
            {
                return GetCurrentCakeLayer().cream.IsPartCompelete();
            }
            else
            {
                return true;
            }
        }
    }

    public void ResetStage()
    {
        foreach (var item in layers)
        {
            item.ResetLayer();
        }
        currentLayerIndex = 0;
        stage.SetActive(false);
    }
}