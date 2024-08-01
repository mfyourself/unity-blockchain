using MahjongLobbyController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Elements
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleTextColorChanger : MonoBehaviour
    {
        public Color onColor = Color.black;
        public Color offColor = Color.white;
        private Toggle toggle;
        private Text text;

        private void Start()
        {
            toggle = GetComponent<Toggle>();
            text = GetComponentInChildren<Text>(true);
            OnValueChanged(toggle.isOn);
        }

        public void OnValueChanged(bool isOn)
        {

            if (text == null) return;
            if (isOn) text.color = onColor;
            else text.color = offColor;
        }
        public void OnPointerUp()
        {
            if( this.gameObject.name == "Dong") 
                LobbyController.roomType = 1;
            else if (this.gameObject.name == "Ban")
                LobbyController.roomType = 2;
            else LobbyController.roomType = 0;
            Debug.Log(this.gameObject.name + "[][]" + LobbyController.roomType);
        }
    }
}
