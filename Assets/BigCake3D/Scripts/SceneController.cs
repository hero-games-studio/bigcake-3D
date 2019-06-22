using UnityEngine.SceneManagement;

public class SceneController
{
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
