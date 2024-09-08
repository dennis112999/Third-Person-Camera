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
        }

        [Header("Third Person Camera (TPC) Type")]
        [SerializeField] private ThirdPersonCameraType _TPCType = ThirdPersonCameraType.Track;

        [Header("Target")]
        [SerializeField] private Transform _playerTrans;
        [SerializeField] private float mPlayerHeight = 2.0f;

        [Header("Camera Magnitude")]
        [SerializeField] private Vector3 _positionOffset = new Vector3(0.0f, 2.0f, -2.5f);
        [SerializeField] private Vector3 _angleOffset = new Vector3(0.0f, 0.0f, 0.0f);
        [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
        [SerializeField] private float _damping = 1.0f;

        [Header("Follow Independent Rotation")]
        [SerializeField] private float mMinPitch = -30.0f;
        [SerializeField] private float mMaxPitch = 30.0f;
        [SerializeField] private float mRotationSpeed = 5.0f;
        private float angleX = 0.0f;

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
            }
        }

        #region Camera Movement Function

        private void CameraType_Track()
        {
            Vector3 targetPos = _playerTrans.transform.position;
            targetPos.y += mPlayerHeight;
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

            angleX -= my * mRotationSpeed;
            angleX = Mathf.Clamp(angleX, mMinPitch, mMaxPitch);

            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.y += mx * mRotationSpeed;

            Quaternion newRot = Quaternion.Euler(angleX, eulerAngles.y, 0.0f) * Quaternion.Euler(_angleOffset);
            transform.rotation = newRot;

            Vector3 targetPos = CalculateTargetPosition();
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _damping);
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