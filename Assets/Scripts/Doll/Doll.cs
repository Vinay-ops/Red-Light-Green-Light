using System.Collections;
using UnityEngine;

public class Doll : MonoBehaviour
{
    [SerializeField] private AudioSource girlSingingAudioSource;
    [SerializeField] private AudioSource rotationAudioSource;
    [SerializeField] private AudioClip girlSinging;
    [SerializeField] private AudioClip rotateSoundClip;

    [SerializeField] private float totalTime = 70f;
    [SerializeField] private float breakTime = 4f;

    private readonly float initialSoundDuration = 5f;
    private readonly float finalSoundDuration = 2.5f;

    float elapsedTime = 0f;
    bool isPlaying = false;

    Coroutine rotationCoroutine = null;
    Player player;

    Transform head;

    bool scanning = false;

    private void Awake()
    {
        if (girlSingingAudioSource == null || girlSinging == null || rotationAudioSource == null || rotateSoundClip == null)
        {
            Debug.LogError("Audio Sources and Sound Clips not assigned!");
            return;
        }

        girlSingingAudioSource.clip = girlSinging;
        girlSingingAudioSource.loop = false;

        rotationAudioSource.clip = rotateSoundClip;
        rotationAudioSource.loop = false;

        head = transform.Find("DollHead");

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    private void Update()
    {
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            if (!isPlaying)
            {
                float currentSoundDuration = Mathf.Lerp(initialSoundDuration, finalSoundDuration, elapsedTime / totalTime);
                girlSingingAudioSource.pitch = initialSoundDuration / currentSoundDuration;
                girlSingingAudioSource.Play();
                isPlaying = true;

                Invoke(nameof(StopSound), currentSoundDuration);
            }
        }

        if (elapsedTime >= totalTime)
        {
            if (!player.PlayerIsDead())
            {
                player.KillPlayer();
            }
            return;
        }

        if (scanning)
        {
            if (player.IsMoving)
            {
                player.KillPlayer();
            }
        }
    }

    void StopSound()
    {
        girlSingingAudioSource.Stop();
        RotateHead(); // turn back to face player
        Invoke(nameof(ResumePlayback), breakTime);
    }

    void ResumePlayback()
    {
        isPlaying = false;
        scanning = false;
        RotateHead(true); // rotate back to normal position
    }

    void RotateHead(bool rotateBack = false)
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        rotationCoroutine = StartCoroutine(RotateHeadOverTime(0.2f, rotateBack));
    }

    IEnumerator RotateHeadOverTime(float seconds, bool rotateBack = false)
    {
        float elapsed = 0f;
        Quaternion startRotation = head.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, rotateBack ? 0f : 180f, 0f);

        rotationAudioSource.Play();

        while (elapsed < seconds)
        {
            head.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / seconds);
            elapsed += Time.deltaTime;
            yield return null;
        }

        head.localRotation = endRotation;
        scanning = !rotateBack;
    }
}
