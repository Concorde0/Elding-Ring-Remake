using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectHightLight : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject highLight;
    public void OnPointerEnter(PointerEventData eventData)
    {
        highLight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highLight.SetActive(false);
    }
}
