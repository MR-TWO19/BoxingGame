using UnityEngine;
using UnityEngine.Events;

public class SwipeDetector : MonoBehaviour
{
    [Header("Swipe Settings")]
    public float minSwipeDistance = 50f; // pixel
    private Vector2 startPos;
    private bool isSwiping = false;

    [Header("Swipe Events")]
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;
    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseSwipe();
#else
        HandleTouchSwipe();
#endif
    }

    
#if UNITY_EDITOR || UNITY_STANDALONE
    void HandleMouseSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            DetectSwipe(Input.mousePosition);
            isSwiping = false;
        }
    }
#else
    void HandleTouchSwipe()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            startPos = touch.position;
            isSwiping = true;
        }
        else if (touch.phase == TouchPhase.Ended && isSwiping)
        {
            DetectSwipe(touch.position);
            isSwiping = false;
        }
    }
#endif

    void DetectSwipe(Vector2 endPos)
    {
        Vector2 delta = endPos - startPos;
        if (delta.magnitude < minSwipeDistance)
        {
            Debug.Log("Swipe quá ngắn, bỏ qua");
            return;
        }

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                Debug.Log("Swipe: RIGHT →");
                OnSwipeRight?.Invoke();
            }
            else
            {
                Debug.Log("Swipe: LEFT ←");
                OnSwipeLeft?.Invoke();
            }
        }
        else
        {
            if (delta.y > 0)
            {
                Debug.Log("Swipe: UP ↑");
                OnSwipeUp?.Invoke();
            }
            else
            {
                Debug.Log("Swipe: DOWN ↓");
                OnSwipeDown?.Invoke();
            }
        }
    }
}