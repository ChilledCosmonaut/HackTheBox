using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MouseHoverText : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject descriptionText;
        public int waitTime;//Measures Time needed to wait to display description Text
        private float _timer;
        private bool _over;

        private void Start()
        {
            _timer = waitTime;
        }

        private void Update()
        {
            if (_over)
            {
                if (_timer <= 0)
                {
                    //If your mouse hovers over the GameObject with the script attached, activate the Text
                    descriptionText.gameObject.SetActive(true);
                }
            
                _timer -= Time.deltaTime;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _over = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //The mouse is no longer hovering over the GameObject so deactivate the Description Text
            descriptionText.gameObject.SetActive(false);
            _over = false;
            _timer = waitTime;
        }
    }
}
