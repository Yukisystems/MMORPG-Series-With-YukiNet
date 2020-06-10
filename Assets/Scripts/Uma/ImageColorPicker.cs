using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageColorPicker : MonoBehaviour, IPointerClickHandler
{
    public Color selectedColor;

    [Serializable]
#pragma warning disable CA1034 // Nested types should not be visible
    public class ColorEvent : UnityEvent<Color> { }

#pragma warning restore CA1034 // Nested types should not be visible

    public ColorEvent OnColorPicked = new ColorEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedColor = GetColor(GetPointerUVPosition());
        OnColorPicked.Invoke(selectedColor);
    }

    private Color GetColor(Vector2 pos)
    {
        Texture2D texture = GetComponent<Image>().sprite.texture;
        Color selected = texture.GetPixelBilinear(pos.x, pos.y);
        selected.a = 1; // force full alpha
        return selected;
    }

    private Vector2 GetPointerUVPosition()
    {
        Vector3[] imageCorners = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(imageCorners);
        float texWidth = imageCorners[2].x - imageCorners[0].x;
        float texHeight = imageCorners[2].y - imageCorners[0].y;
        float uvX = (Input.mousePosition.x - imageCorners[0].x) / texWidth;
        float uvY = (Input.mousePosition.y - imageCorners[0].y) / texHeight;
        return new Vector2(uvX, uvY);
    }
}