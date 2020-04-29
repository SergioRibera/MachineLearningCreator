using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public enum Conector { Normal, Condicion }
public class DDFConector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Conector typeElement;

    Transform container;
    public RectTransform fromTarget;
    public RectTransform toTarget;
    public static bool drag = false;

    void Start()
    {
        container = GameObject.FindGameObjectWithTag("ObjectsInScene").transform;
        fromTarget = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        toTarget = null;
        drag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
        if (toTarget != null)
            Destroy(toTarget.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!drag)
        {
            switch (typeElement)
            {
                case Conector.Normal:
                    GetComponent<Image>().color = Color.red;
                    break;
                case Conector.Condicion:
                    GetComponent<Image>().color = Color.red;
                    break;
            }
        }
        else
        {
            switch (typeElement)
            {
                case Conector.Normal:
                    GetComponent<Image>().color = Color.green;
                    break;
                case Conector.Condicion:
                    GetComponent<Image>().color = Color.green
;
                    break;
            }
            toTarget = eventData.pointerEnter.GetComponent<RectTransform>();
            if (fromTarget != null && toTarget != null)
            {
                print("Create Nodo");
                ConnectionManager.CreateConnection(fromTarget, toTarget);
            }
            drag = false;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        switch (typeElement)
        {
            case Conector.Normal:
                GetComponent<Image>().color = Color.white;
                break;
            case Conector.Condicion:
                GetComponent<Image>().color = Color.white;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Invoke("DisabledMenu", 5f);
    }
}