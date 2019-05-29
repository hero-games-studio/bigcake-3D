using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static void RestartLevel()
    {
        GameManager.GetInstance().ResetShootStartPosition();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
