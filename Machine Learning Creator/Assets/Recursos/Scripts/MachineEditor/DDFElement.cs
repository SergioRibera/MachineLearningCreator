using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum TypeElement { Master, Elemento, Expand }
public class DDFElement : MonoBehaviour, IBeginDragHandler, IDragHandler,  IEndDragHandler
{
    public TypeElement typeElement;
    Transform t;

    void Start()
    {
        if (typeElement != TypeElement.Master)
        {
            t = transform.parent;
            t.SetParent(GameObject.FindGameObjectWithTag("ObjectsInScene").transform);
        }
        else
        {
            t = transform;
        }
    }
    void LateUpdate()
    {
        if(typeElement == TypeElement.Master)
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                t.position = t.position + Input.mousePosition;
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        t.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        switch (typeElement)
        {
            case TypeElement.Elemento:
                t.position = eventData.position;
                break;
            case TypeElement.Expand:
                transform.position = eventData.position;
                float x = t.position.x - transform.position.x / transform.position.x;
                float y = t.position.y - transform.position.y / transform.position.y;
                print(string.Format("x: {0}      y: {1}", x, y));
                //transform.parent.localScale = new Vector3(x, y, 1);
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        t.GetComponent<Image>().raycastTarget = true;
    }
}