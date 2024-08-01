using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityAuthStructure;
using System.Linq.Expressions;

namespace MahJongController{
    public class OnClickElement : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log(this.gameObject.name + "W ha ha");
        }
    }
}
