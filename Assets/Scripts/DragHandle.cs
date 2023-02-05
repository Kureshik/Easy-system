using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandle : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] RectTransform handlePos;
    [SerializeField] Canvas canvas;
    [SerializeField] float minX;
    [SerializeField] float maxX;

    [SerializeField] TMP_InputField inputFieldEq;
    [SerializeField] RectTransform Keyboard;

    public bool isFirstScreen;

    public void OnDrag(PointerEventData eventData)
    {
        // чтобы не перетаскивалось по пустой области
        if (Mathf.Abs(handlePos.localPosition.x - (eventData.position.x - Screen.width / 2)) > 260) return;

        Vector3 offset = new Vector3(eventData.delta.x, 0, 0) / canvas.scaleFactor;
        handlePos.localPosition += offset;
        handlePos.localPosition = new Vector3(Mathf.Clamp(handlePos.localPosition.x, minX, maxX), handlePos.localPosition.y, 0);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("ПОДНЯЛ");
        float xSelf = handlePos.localPosition.x;
        float xPointer = eventData.position.x - Screen.width / 2;

        //(магнит влево) (для клика вправо)            (клик слева)                   (доведение до края)
        if ((xSelf < 0 && xSelf != -250) || (xPointer < 0 && xSelf == 250) || (!isFirstScreen && xSelf == -250))
        {
            handlePos.DOLocalMoveX(-250, 0.4f).SetEase(Ease.OutQuart);
            inputFieldEq.ActivateInputField();
            Keyboard.DOAnchorPosY(450, 0.4f).SetEase(Ease.OutQuart);
        }
        else if ((xSelf > 0 && xSelf != 250) || (xPointer > 0 && xSelf == -250) || (isFirstScreen && xSelf == 250))
        {
            handlePos.DOLocalMoveX(250, 0.4f).SetEase(Ease.OutQuart);
            Keyboard.DOAnchorPosY(-450, 0.4f).SetEase(Ease.OutQuart);
        }
    }

    public void moveToSolution()
    {
        handlePos.DOLocalMoveX(250, 0.4f).SetEase(Ease.OutQuart);
        Keyboard.DOAnchorPosY(-450, 0.4f).SetEase(Ease.OutQuart);
        isFirstScreen = handlePos.localPosition.x < 0;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isFirstScreen = handlePos.localPosition.x < 0;
        Debug.Log("НАЖАЛ");
    }
}
