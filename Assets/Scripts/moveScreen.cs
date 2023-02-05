using UnityEngine;

public class moveScreen : MonoBehaviour
{
    [SerializeField] Transform handleTransform;
    float xPos;
    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(map(handleTransform.localPosition.x, -250, 250, xPos, xPos - 1080), transform.localPosition.y, 0);
    }

    float map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
