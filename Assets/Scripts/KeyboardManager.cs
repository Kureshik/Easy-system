using DG.Tweening;
using TMPro;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Core core;
    //public int cursorPosition;

    private void Start()
    {
        inputField.ActivateInputField();
    }

    public void Simbol(string v)
    {
        inputField.ActivateInputField();
        string newText = inputField.text.Insert(inputField.caretPosition, v);
        inputField.text = newText;
        inputField.caretPosition = Mathf.Min(inputField.caretPosition + 1, inputField.text.Length);
    }

    public void NewLine()
    {
        inputField.ActivateInputField();
        if (inputField.text.Split('\n').Length > 4) return;

        inputField.MoveToEndOfLine(false, false);
        string newText = inputField.text.Insert(inputField.caretPosition, "\n");
        inputField.text = newText;
        inputField.caretPosition = Mathf.Min(inputField.caretPosition + 1, inputField.text.Length);
    }

    public void Backspace()
    {
        inputField.ActivateInputField();
        if (inputField.text != "" && inputField.caretPosition != 0) inputField.text = inputField.text.Remove(inputField.caretPosition - 1, 1);
        if (inputField.text.Length != inputField.caretPosition) inputField.caretPosition = Mathf.Max(inputField.caretPosition - 1, 0);
    }

    public void debug()
    {
        GetComponent<RectTransform>().DOAnchorPosY(-450, 0.4f).SetEase(Ease.OutQuart);
    }

    public void ChangeCursorPos(bool left)
    {
        inputField.ActivateInputField();
        int length = inputField.text.Length;
        int newPos = inputField.caretPosition + (left ? -1 : 1);
        inputField.caretPosition = newPos;
    }
}
