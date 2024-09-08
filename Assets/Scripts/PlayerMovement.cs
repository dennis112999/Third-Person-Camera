using UnityEngine;

namespace TPC
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _characterController;
        [SerializeField] private Animator _animator;

        [Header("Movement Magnitude")]
        [SerializeField] private float _walkSpeed = 1.5f;
        [SerializeField] private float _runSpeed = 3.0f;
        [SerializeField] private float _rotationSpeed = 50.0f;
        [SerializeField] private float _gravity = -30.0f;

        [Header("Only useful with Follow and Independent")]
        [SerializeField] private bool _followCameraForward = false;
        [SerializeField] private float _turnRate = 200.0f;
        [SerializeField] private Vector3 _velocity = new Vector3(0.0f, 0.0f, 0.0f);

        #region MonoBehaviour

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            Move();
        }

        #endregion MonoBehaviour

        private void Move()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float speed = GetMovementSpeed();
            RotatePlayer(h, v);
            MovePlayer(v, speed);
            ApplyGravity();

            UpdateAnimator(v, speed);
        }

        /// <summary>
        /// Player's movement speed based on input
        /// </summary>
        /// <returns> The movement speed, either walking or running depending on input.</returns>
        private float GetMovementSpeed()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return _runSpeed;
            }
            return _walkSpeed;
        }

        /// <summary>
        /// Rotates the player based on the horizontal input and camera direction
        /// </summary>
        /// <param name="h">Horizontal input value</param>
        /// <param name="v">Vertical input value</param>
        private void RotatePlayer(float h, float v)
        {
            if (_followCameraForward)
            {
                if (Mathf.Abs(v) > 0.1f || Mathf.Abs(h) > 0.1f)
                {
                    Vector3 cameraEulerAngles = Camera.main.transform.rotation.eulerAngles;
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        Quaternion.Euler(0.0f, cameraEulerAngles.y, 0.0f),
                        _turnRate * Time.deltaTime
                    );
                }
            }
            else
            {
                transform.Rotate(0.0f, h * _rotationSpeed * Time.deltaTime, 0.0f);
            }
        }

        /// <summary>
        /// Moves the player forward based on the vertical input and movement speed.
        /// </summary>
        /// <param name="v">Vertical input value, ranging from -1 to 1</param>
        /// <param name="speed">Movement speed</param>
        private void MovePlayer(float v, float speed)
        {
            Vector3 moveDirection = transform.forward * v * speed * Time.deltaTime;
            _characterController.Move(moveDirection);
        }

        /// <summary>
        /// Apply Gravity to player
        /// </summary>
        private void ApplyGravity()
        {
            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0f;
            }
        }

        /// <summary>
        /// Updates the animator with movement data.
        /// </summary>
        /// <param name="v">Input Vertical value</param>
        /// <param name="speed">Movement speed</param>
        private void UpdateAnimator(float v, float speed)
        {
            if (_animator != null)
            {
                _animator.SetFloat("PosZ", v * speed / _runSpeed);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Animator is missing !!!!");
#endif
            }
        }

    }
}
