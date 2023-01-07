using UnityEngine;

public class MovableCube : MonoBehaviour
{
    public GameObject textObject;

    void OnMouseDown()
    {
        TextMesh textMesh = textObject.GetComponent<TextMesh>();
        textObject.SetActive(!textObject.activeSelf);
    }
}