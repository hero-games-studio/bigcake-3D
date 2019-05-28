using UnityEngine;

public enum PieceState
{
    Colored,
    UnColored
}

public class Piece : MonoBehaviour
{
    public PieceState State = PieceState.UnColored;
}