using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [SerializeField] GameObject PauseOverlay;
    
    void Start()
    {
        
    }

    public void Game()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        PauseOverlay.SetActive(true);
        Time.timeScale = 0;
    }

    public void Return()
    {
        PauseOverlay.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
