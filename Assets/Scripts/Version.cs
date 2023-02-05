using UnityEngine;
using TMPro;

public class Version : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = $"Версия: {Application.version}";
    }
}
