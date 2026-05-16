using UnityEngine;
using UnityEngine.UI;

public class FloatingTooltip : MonoBehaviour
{
    [TextArea] public string tooltipText = "Подсказка"; // текст подсказки
    public Vector2 offset = new Vector2(15f, -15f);    // смещение относительно курсора (X, Y)

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Text textComponent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textComponent = GetComponentInChildren<Text>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (textComponent != null)
            textComponent.text = tooltipText;

        gameObject.SetActive(false); // изначально скрыто
    }

    // Вызывается извне (из скрипта кнопки) для показа подсказки
    public void ShowTooltip()
    {
        gameObject.SetActive(true);
        UpdatePosition();
    }

    // Обновление позиции подсказки под курсором
    public void UpdatePosition()
    {
        if (!gameObject.activeSelf) return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition,
            parentCanvas.worldCamera,
            out mousePos
        );

        rectTransform.anchoredPosition = mousePos + offset;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}