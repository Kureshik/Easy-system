using TMPro;
using UnityEngine;

public class textAnimation : MonoBehaviour
{
    [SerializeField] RectTransform handleTransform;
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float x = handleTransform.localPosition.x;
        text.color = new Color(1, 1, 1, Mathf.Abs(map(x, -250, 250, -1, 1)));
        text.text = x < 0 ? "Уравнения" : "Решение";
    }

    float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}