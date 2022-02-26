using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Vector2 TouchDistance { get; set; }
        public Vector2 PointerOld { get; set; }
        private int PointerId { get; set; }
        public bool Pressed { get; set; }

        void Update()
        {
            if (Pressed)
            {
                if (PointerId >= 0 && PointerId < Input.touches.Length)
                {
                    TouchDistance = Input.touches[PointerId].position - PointerOld;
                    PointerOld = Input.touches[PointerId].position;
                }
                else
                {
                    TouchDistance = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
            }
            else
            {
                TouchDistance = new Vector2();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            PointerId = eventData.pointerId;
            PointerOld = eventData.position;
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
        }
    }
}