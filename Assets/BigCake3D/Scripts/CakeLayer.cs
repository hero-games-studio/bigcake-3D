[System.Serializable]
public class CakeLayer
{
    public Cake cake;
    public Cream cream;

    public void ResetLayer()
    {
        ResetCake();
        ResetCream();
    }

    public void ResetCake()
    {
        Reset(cake.GetComponentsInChildren<Piece>());
    }

    public void ResetCream()
    {
        Reset(cream.GetComponentsInChildren<Piece>());
    }

    public void ResetCurrentPart()
    {
        if (!cake.IsPartCompelete())
        {
            ResetCake();
        }
        else
        {
            if (cream != null && cream.IsPartCompelete())
            {
                ResetCream();
            }
        }
    }

    public bool CheckState()
    {
        if (!cake.IsPartCompelete())
        {
            return false;
        }
        else
        {
            if (cream != null)
            {
                return cream.IsPartCompelete();
            }
            else
            {
                return true;
            }
        }
    }

    private void Reset(Piece[] pieces)
    {
        foreach (Piece piece in pieces)
        {
            piece.SetUnColored();
        }
    }
}