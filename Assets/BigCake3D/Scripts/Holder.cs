﻿using UnityEngine;

public class Tags
{
    public static readonly string T_PIECE = "Piece";
    public static readonly string T_OBSTACLE = "Obstacle";
    public static readonly string T_BOUNDARY = "Boundary";
    public static readonly string T_NEARMISS = "NearMiss";
}
public  class AnimatorParameters
{
    public static readonly int P_NEARMISS = Animator.StringToHash("NearMiss");
    public static readonly int P_ISSQUEEZE = Animator.StringToHash("IsSqueeze");
    public static readonly int P_TOPPINGANIM = Animator.StringToHash("ToppingAnim");
    public static readonly int P_PULSE = Animator.StringToHash("Pulse");
    public static readonly int P_LOOKCAKE = Animator.StringToHash("Look");
}
public class OtherData
{
    public static readonly string USERDATA_PATH = Application.persistentDataPath + "/userData.bc3d";
    public static readonly float duration = 0.15f;
}