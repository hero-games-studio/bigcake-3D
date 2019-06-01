public class ScoreManager : MonoSingleton<ScoreManager>
{
    #region Score 

    private int _score = 0;

    public void AddScore(int point = 10)
    {
        _score += point;
    }

    public int GetScore() => _score;

    #endregion
}
