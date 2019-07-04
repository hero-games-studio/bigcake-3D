﻿using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private bool isCollideWithPiece = false;

    [SerializeField]
    private float boundTime = 0.1f;
    private float previousTime = 0.0f;

    private Piece piece = null;

    private void Update()
    {
        if (isCollideWithPiece && Painter.Instance.isPainting)
        {
            if (Time.time - previousTime > boundTime)
            {
                piece.SetColored();
                previousTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.T_PIECE)
        {
            piece = other.GetComponent<Piece>();

            isCollideWithPiece = true;
        }

        if (other.tag == Tags.T_OBSTACLE)
        {
            isCollideWithPiece = false;
            ScoreManager.Instance.ResetNearMiss();
            StageManager.Instance.ResetCurrentPart();
        }

        if (other.tag == Tags.T_NEARMISS)
        {
            ScoreManager.Instance.AddNearMiss();
        }
    }
}
