using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float horizontalSpeed;
        [SerializeField] private float verticalSpeed;
        [SerializeField] private float arrowSpeed;
        //private Vector3 oldCameraPosition;
        private Transform cameraTransform;
        //float defaultMoveThisAxis;
        //float defaultMoveOtherAxis;
        //float defaultMoveSum;

        public void Awake()
        {
            cameraTransform = Camera.main.gameObject.transform;
            //oldCameraPosition = cameraTransform.position;
            //defaultMoveOtherAxis = 4;
            //defaultMoveThisAxis = 7;
            //defaultMoveSum = Mathf.Sqrt(Mathf.Pow(defaultMoveThisAxis, 2) + Mathf.Pow(defaultMoveOtherAxis, 2));
        }

        public void CameraMoveUp()
        {
            cameraTransform.Translate(new Vector3(0, verticalSpeed * Time.deltaTime, 0));
            //Debug.Log($"CameraMoveUp, camera should move {0},{verticalSpeed * Time.deltaTime},{0}");
            //Debug.Log($"After CameraMoveUp, camera moved {cameraTransform.position - oldCameraPosition}");
            //oldCameraPosition = cameraTransform.position;
        }

        public void CameraMoveDown()
        {
            cameraTransform.Translate(new Vector3(0, -verticalSpeed * Time.deltaTime, 0));
            //Debug.Log($"CameraMoveDown, camera should move {0},{-verticalSpeed * Time.deltaTime},{0}");
            //Debug.Log($"After CameraMoveDown, camera moved {cameraTransform.position - oldCameraPosition}");
            //oldCameraPosition = cameraTransform.position;
        }

        public void CameraMoveRight()
        {
            cameraTransform.Translate(new Vector3(horizontalSpeed * Time.deltaTime, 0, 0));
        }

        public void CameraMoveLeft()
        {
            cameraTransform.Translate(new Vector3(-horizontalSpeed * Time.deltaTime, 0, 0));
        }

        public void CameraMoveIn()
        {
            cameraTransform.Translate(new Vector3(0, 0, arrowSpeed * Time.deltaTime));
            //Debug.Log($"CameraMoveIn, camera should move {0}, {0}, {arrowSpeed*Time.deltaTime}");
            //Debug.Log($"After CameraMoveIn, camera moved {cameraTransform.position - oldCameraPosition}");
            //oldCameraPosition = cameraTransform.position;
        }

        public void CameraMoveOut()
        {
            cameraTransform.Translate(new Vector3(0, 0, -arrowSpeed * Time.deltaTime));
            //Debug.Log($"CameraMoveOut, camera should move {0}, {0}, {-arrowSpeed * Time.deltaTime}");
            //Debug.Log($"After CameraMoveOut, camera moved {cameraTransform.position - oldCameraPosition}");
            //oldCameraPosition = cameraTransform.position;
        }
    }
}