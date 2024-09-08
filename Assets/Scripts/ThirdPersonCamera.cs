using UnityEngine;

namespace TRC
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public enum ThirdPersonCameraType
        {
            Track,
            Follow,
            FollowWithTrackRotation,
            FollowWithIndependentRotation,
            TopDown,
        }

        [Header("Third Person Camera (TPC) Type")]
        [SerializeField] private ThirdPersonCameraType _TPCType = ThirdPersonCameraType.Track;

        [Header("Target")]
        [SerializeField] private Transform _playerTrans;
        [SerializeField] private float _playerHeight = 2.0f;

        [Header("Camera Magnitude")]
        [SerializeField] private Vector3 _positionOffset = new Vector3(0.0f, 2.0f, -2.5f);
        [SerializeField] private Vector3 _angleOffset = new Vector3(0.0f, 0.0f, 0.0f);
        [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
        [SerializeField] private float _damping = 1.0f;

        [Header("Follow Independent Rotation")]
        [SerializeField] private float _minPitch = -30.0f;
        [SerializeField] private float _maxPitch = 30.0f;
        [SerializeField] private float _rotationSpeed = 5.0f;
        private float _angleX = 0.0f;

        #region MonoBehaviour

        private void Awake()
        {
            Initialize();
        }

        void LateUpdate()
        {
            HandleCameraMovement();
        }

        #endregion MonoBehaviour

        #region Initialize

        private void Initialize()
        {
            switch (_TPCType)
            {
                case ThirdPersonCameraType.Track:
                    SetCameraPositionAndRotation(CalculateTargetPosition(), Quaternion.LookRotation(_playerTrans.position - transform.position));
                    break;
                case ThirdPersonCameraType.Follow:
                    SetCameraPositionAndRotation(CalculateTargetPosition(), CalculateRotation(false));
                    break;
                case ThirdPersonCameraType.FollowWithTrackRotation:
                    SetCameraPositionAndRotation(CalculateTargetPosition(), CalculateRotation(true));
                    break;
                case ThirdPersonCameraType.FollowWithIndependentRotation:
                    SetCameraPositionAndRotation(CalculateTargetPosition(), Quaternion.Euler(_angleOffset));
                    break;
                case ThirdPersonCameraType.TopDown:
                    Vector3 topDownPosition = _playerTrans.position + new Vector3(0, _positionOffset.y, 0);
                    SetCameraPositionAndRotation(topDownPosition, Quaternion.Euler(90.0f, 0.0f, 0.0f));
                    break;
                default:
#if UNITY_EDITOR
                    Debug.LogError($"Invalid ThirdPersonCameraType: {_TPCType}");
#endif
                    break;
            }
        }

        // Utility method to set camera position and rotation instantly
        private void SetCameraPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        #endregion Initialize

        private void HandleCameraMovement()
        {
            switch (_TPCType)
            {
                case ThirdPersonCameraType.Track:
                    CameraType_Track();
                    break;
                case ThirdPersonCameraType.Follow:
                    CameraType_Follow();
                    break;
                case ThirdPersonCameraType.FollowWithTrackRotation:
                    CameraType_Follow(true);
                    break;
                case ThirdPersonCameraType.FollowWithIndependentRotation:
                    CameraType_FollowWithIndependentRotation();
                    break;
                case ThirdPersonCameraType.TopDown:
                    CameraType_TopDown();
                    break;
                default:
#if UNITY_EDITOR
                    Debug.LogError($"Invalid ThirdPersonCameraType: {_TPCType}");
#endif
                    break;
            }
        }

        #region Camera Movement Function

        private void CameraType_Track()
        {
            Vector3 targetPos = _playerTrans.transform.position;
            targetPos.y += _playerHeight;
            transform.LookAt(targetPos);
        }

        private void CameraType_Follow(bool allowRotationTracking = false)
        {
            Quaternion targetRotation = CalculateRotation(allowRotationTracking);
            Vector3 targetPos = CalculateTargetPosition();

            // Smoothly interpolate position and rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _damping);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _damping);
        }

        private void CameraType_FollowWithIndependentRotation()
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            _angleX -= my * _rotationSpeed;
            _angleX = Mathf.Clamp(_angleX, _minPitch, _maxPitch);

            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.y += mx * _rotationSpeed;

            Quaternion newRot = Quaternion.Euler(_angleX, eulerAngles.y, 0.0f) * Quaternion.Euler(_angleOffset);
            transform.rotation = newRot;

            Vector3 targetPos = CalculateTargetPosition();
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _damping);
        }

        private void CameraType_TopDown()
        {
            Vector3 targetPos = _playerTrans.position;
            targetPos.y += _positionOffset.y;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _damping);
            transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }

        #endregion Camera Movement Function

        /// <summary>
        /// Calculates the target position of the camera based on the player's position
        /// </summary>
        /// <returns>calculated target position of the camera</returns>
        private Vector3 CalculateTargetPosition()
        {
            Vector3 forward = transform.rotation * Vector3.forward;
            Vector3 right = transform.rotation * Vector3.right;
            Vector3 up = transform.rotation * Vector3.up;

            Vector3 targetPos = _playerTrans.position;
            return targetPos + forward * _positionOffset.z + right * _positionOffset.x + up * _positionOffset.y;
        }


        /// <summary>
        /// Calculates the camera's rotation
        /// </summary>
        /// <param name="allowRotationTracking">Determines if the camera should track the player's rotation.</param>
        /// <returns>
        /// The calculated rotation of the camera
        /// </returns>
        private Quaternion CalculateRotation(bool allowRotationTracking)
        {
            Quaternion initialRotation = Quaternion.Euler(_angleOffset);

            if (allowRotationTracking)
            {
                return _playerTrans.rotation * initialRotation;
            }
            return initialRotation;
        }
    }

}