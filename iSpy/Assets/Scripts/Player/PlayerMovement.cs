using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Joystick joystick;

        [SerializeField] private new Camera camera;
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private TouchField touch;

        [Header("Properties")] [SerializeField]
        private float speed;

        private Vector2 _mousePosition;
        private Vector2 _inputMovement;

        private void Update()
        {
            InputMovement();
        }

        private void InputMovement()
        {
            _inputMovement.x = joystick.Horizontal;
            _inputMovement.y = joystick.Vertical;

            _mousePosition.x -= touch.TouchDistance.x * 0.2f;
        }

        private void Movement()
        {
            rigidbody2D.MovePosition(rigidbody2D.position + _inputMovement * speed * Time.fixedDeltaTime);

            rigidbody2D.rotation = _mousePosition.x;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void LateUpdate()
        {
            var cameraTransform = camera.transform;
            var position = transform.position;
            cameraTransform.position = new Vector3(position.x, position.y, -10f);
        }
    }
}