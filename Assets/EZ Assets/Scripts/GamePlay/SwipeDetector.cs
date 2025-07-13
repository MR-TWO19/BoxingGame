using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour
{
    public static SwipeDetector Ins;

    [Header("Swipe Settings")]
    public float minSwipeDistance = 50f; // pixel
    private Vector2 startPos;
    private bool isSwiping = false;

    [Header("Ignore Area")]
    public RectTransform ignoreArea;

    [Header("Swipe Events")]
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;
    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnSwipeClick;

    private void Awake()
    {
        if (Ins == null)
            Ins = this;
    }

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
        int idx = IsUI() ? 1 : 0;

        if (Input.GetMouseButtonDown(idx))
        {
            startPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(idx) && isSwiping)
        {
            DetectSwipe(Input.mousePosition);
            isSwiping = false;
        }
    }
#else
    void HandleTouchSwipe()
    {
        if (Input.touchCount == 0) return;

        int idx = IsUI() ? 1 : 0;
        Touch touch = Input.GetTouch(idx);
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
            Debug.Log("Swipe: CLICK");
            OnSwipeClick?.Invoke();
            return;
        }

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x < 0)
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

    private bool IsUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
#else
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return true;
        }
#endif
        return false;
    }
}