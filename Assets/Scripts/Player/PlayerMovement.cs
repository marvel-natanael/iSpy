using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField] private Joystick joystick;

        //[SerializeField] private new Camera camera;
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private TouchField touch;

        [Header("Properties")]
        [SerializeField]
        public Button btnShoot;
        [SerializeField] private float speed;

        private Vector2 _mousePosition;
        private Vector2 _inputMovement;

        public Vector2 GetInputMovement() => _inputMovement;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {

            if (Input.GetKey(KeyCode.A))
            {
                if (btnShoot == null) return;

                btnShoot.onClick.Invoke();
                Debug.Log("Test");
                _inputMovement.x = 1;

                rigidbody2D.MovePosition(rigidbody2D.position + _inputMovement * speed * Time.fixedDeltaTime);

            }
            InputMovement();
        }

        private void InputMovement()
        {
            if (joystick == null && touch == null) return;

            _inputMovement.x = joystick.Horizontal;
            _inputMovement.y = joystick.Vertical;

            _mousePosition.x -= touch.TouchDistance.x * 0.2f;
        }

        private void Movement()
        {
            //if (!hasAuthority) return;

            //rigidbody2D.MovePosition(rigidbody2D.position + _inputMovement * speed * Time.fixedDeltaTime);
            var input = new Vector3(_inputMovement.x, _inputMovement.y, 0);
            transform.Translate(input * speed * Time.fixedDeltaTime, Space.Self);
            rigidbody2D.rotation = _mousePosition.x; ;

        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void LateUpdate()
        {
            //var cameraTransform = camera.transform;
            //var position = transform.position;
            //cameraTransform.position = new Vector3(position.x, position.y, -10f);
            //cameraTransform.rotation = Quaternion.identity;
        }

        private void Initialize()
        {
            if (!hasAuthority && !isLocalPlayer) return;

            joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
            touch = GameObject.Find("TouchField").GetComponent<TouchField>();
            btnShoot = GameObject.Find("BtnShoot").GetComponent<Button>();
        }
    }
}