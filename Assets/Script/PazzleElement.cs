using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PazzleElement : MonoBehaviour,IPointerDownHandler
{
    public int Id;
    public int X;
    public int Y;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.CanMove)
        {
            Debug.Log(Id);
            GameManager.Instance.PazzleMoving(X,Y, this);
        }
    }
    
}
	
