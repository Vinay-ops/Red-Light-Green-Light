using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMoment : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Enter the build index of the win scene.")]
    public int winSceneIndex = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player crossed the win line!");
            SceneManager.LoadScene(winSceneIndex);
        }
    }
}
