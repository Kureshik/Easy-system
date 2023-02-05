using TMPro;
using UnityEngine;

public class debug : MonoBehaviour
{
    public TMP_InputField text;
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void PasteText()
    {
        text.text = "5y + y + 5x + 4z = 3\n3y + 7x = -6z\n4z + 6y + 6x = 2 - 2y";
    }
}
