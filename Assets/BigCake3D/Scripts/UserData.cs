using System;

[Serializable]
public class UserData : IUserData
{
    public int level;
    public int bestScore;

    public void Init()
    {
        level = 0;
        bestScore = 0;
    }
}