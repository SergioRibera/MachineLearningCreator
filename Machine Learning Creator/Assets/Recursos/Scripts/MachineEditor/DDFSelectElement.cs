using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DDFSelectElement : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject prefabElement;
    public GameObject go;

    public void OnDrag(PointerEventData eventData)
    {
        go.transform.position = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        go = Instantiate(prefabElement);
        go.transform.SetParent(transform);
        go.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        go.GetComponent<Image>().color = GetComponent<Image>().color;
        go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = transform.parent.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        int r = Random.Range(0, 9999);
        foreach (var item in GetChilds(go.transform))
        {
            item.gameObject.name += " " + r;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        go = null;
    }
    List<Transform> GetChilds(Transform t)
    {
        return t.GetComponentsInChildren<Transform>().ToList();
    }
}