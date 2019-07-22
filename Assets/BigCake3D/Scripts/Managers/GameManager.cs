public class GameManager
{
    #region Singleton
    private static GameManager _instance = null;
    private static object _lock = new object();

    private GameManager() { }

    public static GameManager GetInstance()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    #endregion
}
