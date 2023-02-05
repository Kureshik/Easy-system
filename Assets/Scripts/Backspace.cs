using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Backspace : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] KeyboardManager keyboardManager;
    public float speed = 0.25f;
    public float holdTime = 1f;


    float currentHoldTime = 1f;
    bool isHolding;

    IEnumerator ExecuteAfterTime()
    {
        while (isHolding)
        {
            if (currentHoldTime > 0) currentHoldTime -= speed;
            else keyboardManager.Backspace();

            yield return new WaitForSeconds(speed);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        keyboardManager.Backspace();
        StartCoroutine(ExecuteAfterTime());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        StopCoroutine(ExecuteAfterTime());
        currentHoldTime = holdTime;
    }
}
