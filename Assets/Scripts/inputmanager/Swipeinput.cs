using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(Player))]
public class SwipeInputSystem : MonoBehaviour
{
    private Player player;
    private Vector2 startPos;
    private bool swiping = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        EnhancedTouchSupport.Enable();
    }

    private void OnEnable()
    {
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(Finger finger)
    {
        if (finger.index != 0) return;
        startPos = finger.screenPosition;
        swiping = true;
    }

    private void OnFingerUp(Finger finger)
    {
        if (!swiping || finger.index != 0) return;
        swiping = false;

        Vector2 endPos = finger.screenPosition;
        Vector2 swipeDelta = endPos - startPos;

        if (Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
        {
            if (swipeDelta.y > 50f)
                player.ApplyMoveInput(Vector2.up);  // Swipe Up ? Move Forward
            else if (swipeDelta.y < -50f)
                player.ApplyMoveInput(Vector2.zero); // Swipe Down ? Stop
        }
    }
}
