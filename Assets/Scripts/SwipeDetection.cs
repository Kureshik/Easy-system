using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetection : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static event OnSwipe SwipeEvent;
    public delegate void OnSwipe();

    Vector2 startPosition;
    Vector2 swipeDelta;
    float deadZone = 80;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        swipeDelta = eventData.position - startPosition;

        if (swipeDelta.magnitude > deadZone && SwipeEvent != null)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && swipeDelta.x > 0)
                SwipeEvent();

            ResetSwipe();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetSwipe();
    }

    private void ResetSwipe()
    {
        startPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
