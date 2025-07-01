using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Set the index of the game scene to load.")]
    public int sceneIndexToLoad = 1;

    [Header("Audio Settings")]
    [Tooltip("Assign background music AudioSource here.")]
    public AudioSource backgroundMusic;

    private void Start()
    {
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

    // Called when Start button is clicked
    public void StartGame()
    {
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    // Called when Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
