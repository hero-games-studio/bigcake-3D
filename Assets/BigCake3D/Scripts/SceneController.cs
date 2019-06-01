using UnityEngine.SceneManagement;

public class SceneController
{
    public static void RestartLevel()
    {
        Shooter.Instance.ResetShootStartPosition();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
