using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Vector3 currentMovement;
    private Vector2 currentMovementInput;

    private Rigidbody characterController;
    private Animator playerAnimator;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private int gameOverSceneIndex = 1; // Set in Inspector

    private bool PlayerisDead = false;

    public bool IsMoving => playerAnimator.GetBool("Running") || playerAnimator.GetBool("Stopping");

    public void KillPlayer()
    {
        if (PlayerIsDead())
            return;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        playerAnimator.SetBool("Die", true);
        PlayerisDead = true;
    }

    public bool PlayerIsDead()
    {
        return playerAnimator.GetBool("Die");
    }

    private void Awake()
    {
        characterController = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource not assigned.");
        }
        else
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (PlayerisDead) return;

        if (currentMovement != Vector3.zero && !PlayerIsDead())
        {
            characterController.AddForce(transform.TransformDirection(currentMovement.normalized) * acceleration, ForceMode.Acceleration);
        }
        else
        {
            playerAnimator.SetBool("Stopping", true);
            characterController.AddForce(-characterController.velocity.normalized * acceleration, ForceMode.Acceleration);
        }

        if (characterController.velocity.magnitude > maxSpeed)
        {
            characterController.velocity = characterController.velocity.normalized * maxSpeed;
        }

        if (IsMagnitudeLowerThan())
        {
            playerAnimator.SetBool("Stopping", false);
            characterController.velocity = Vector3.zero;
        }
    }

    public void OnMove(InputValue value)
    {
        if (PlayerisDead) return;

        currentMovementInput = value.Get<Vector2>();

        if (currentMovementInput != Vector2.zero)
        {
            playerAnimator.SetBool("Running", true);
        }
        else
        {
            playerAnimator.SetBool("Running", false);
        }

        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
    }

    // ✅ Swipe input support (call from external swipe handler)
    public void ApplyMoveInput(Vector2 input)
    {
        if (PlayerisDead) return;

        currentMovementInput = input;

        if (currentMovementInput != Vector2.zero)
        {
            playerAnimator.SetBool("Running", true);
        }
        else
        {
            playerAnimator.SetBool("Running", false);
        }

        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
    }

    bool IsMagnitudeLowerThan(float minMagnitude = 0.1f)
    {
        return characterController.velocity.magnitude < minMagnitude;
    }

    public void OnDeathAnimationComplete()
    {
        Debug.Log("Death animation complete - loading Game Over scene...");
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(0.2f); // Optional delay
        SceneManager.LoadScene(gameOverSceneIndex);
    }
}
