using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvasUI : MonoBehaviour
{
    [SerializeField] private int mainMenuSceneIndex = 0; // Set this in Inspector

    [Header("Audio Settings")]
    [Tooltip("Assign background music AudioSource here.")]
    public AudioSource backgroundMusic;

    void Start()
    {
        Time.timeScale = 1f; // Unpause game just in case

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

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex); // Load Main Menu scene
    }
}
