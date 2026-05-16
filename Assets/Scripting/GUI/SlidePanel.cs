using UnityEngine;
using UnityEngine.EventSystems;

public class SlidePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform panelRect;
    public float hiddenPos = -80f;   // панель скрыта
    public float visiblePos = 80f;     // панель видна
    public float speed = 8f;          // —корость анимации
    private float targetPos;
    void Start()
    {
        Vector2 anchoredPos = panelRect.anchoredPosition;
        anchoredPos.x = hiddenPos;
        panelRect.anchoredPosition = anchoredPos;
        targetPos = hiddenPos;
    }
    void Update()
    {
        Vector2 anchoredPos = panelRect.anchoredPosition;
        anchoredPos.x = Mathf.Lerp(anchoredPos.x, targetPos, speed * Time.deltaTime);
        panelRect.anchoredPosition = anchoredPos;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
            targetPos = visiblePos;
            
    }
    public void OnPointerExit(PointerEventData eventData)
    {

            targetPos = hiddenPos;
        
    }
}
