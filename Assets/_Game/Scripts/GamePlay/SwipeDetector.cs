using System;
using System.Collections.Generic;
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
    private bool ignoreThisSwipe = false;

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
#if UNITY_EDITOR
        // In simulator/editor, allow real touch input if present (Device Simulator)
        if (Input.touchCount > 0)
            HandleTouchSwipe();
        else
            HandleMouseSwipe();
#elif UNITY_STANDALONE
        HandleMouseSwipe();
#else
        HandleTouchSwipe();
#endif
    }

    void HandleMouseSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isSwiping = true;
            ignoreThisSwipe = IsUI_Mouse(startPos) || IsInIgnoreArea(startPos);
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            if (!ignoreThisSwipe)
                DetectSwipe(Input.mousePosition);

            isSwiping = false;
            ignoreThisSwipe = false;
        }
    }

    void HandleTouchSwipe()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startPos = touch.position;
            isSwiping = true;
            ignoreThisSwipe = IsUI_Touch(touch.fingerId) || IsInIgnoreArea(startPos);
        }
        else if (touch.phase == TouchPhase.Ended && isSwiping)
        {
            if (!ignoreThisSwipe)
                DetectSwipe(touch.position);

            isSwiping = false;
            ignoreThisSwipe = false;
        }
    }

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

    private bool IsInIgnoreArea(Vector2 screenPos)
    {
        if (ignoreArea == null) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(ignoreArea, screenPos);
    }

    private bool IsUI_Touch(int fingerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
    }

    // More reliable for mouse in editor/standalone than IsPointerOverGameObject() without id
    private bool IsUI_Mouse(Vector2 screenPos)
    {
        if (EventSystem.current == null) return false;

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results != null && results.Count > 0;
    }
}