using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImage;
    private Image stick;
    
    public Vector3 inputDirection {set; get;}


    private void Start()
    {
        bgImage = GetComponent<Image>();
        stick = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform,
            eventData.position,
            eventData.pressEventCamera, out pos))
        {
            pos.x = pos.x / bgImage.rectTransform.sizeDelta.x;
            pos.y = pos.y / bgImage.rectTransform.sizeDelta.y;

            float x = bgImage.rectTransform.pivot.x == 1 ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = bgImage.rectTransform.pivot.y == 1 ? pos.y * 2 + 1 : pos.y * 2 - 1;
            
            inputDirection = new Vector3(x, 0, y);
            inputDirection = inputDirection.magnitude > 1 ? inputDirection.normalized : inputDirection;
                
            stick.rectTransform.anchoredPosition = new Vector3(
                inputDirection.x * (bgImage.rectTransform.sizeDelta.x / 3),
                inputDirection.z * (bgImage.rectTransform.sizeDelta.y / 3)
                );
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputDirection = Vector3.zero;
        stick.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
}
