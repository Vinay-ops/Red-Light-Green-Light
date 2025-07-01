using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private int mainMenuSceneIndex = 0; // Set in Inspector
    [SerializeField] private int gameplaySceneIndex = 1; // Set in Inspector

    [Header("Audio Settings")]
    [Tooltip("Assign background music AudioSource here.")]
    public AudioSource backgroundMusic;

    private void Start()
    {
        Time.timeScale = 1f; // Unpause game when entering Game Over scene

        if (backgroundMusic != null)
        {
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.loop = true;
                backgroundMusic.Play();
            }
        }
        else
        {
            Debug.LogWarning("Background music AudioSource is not assigned.");
        }
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene(gameplaySceneIndex); // Reload gameplay
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(mainMenuSceneIndex); // Go to main menu
    }
}
