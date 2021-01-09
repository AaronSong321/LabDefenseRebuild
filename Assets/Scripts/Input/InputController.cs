using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;

namespace Aaron.LabDefenseRebuild
{
    public class InputController : MonoBehaviour
    {
        public delegate void InputHandler();
        private Dictionary<KeyCode, InputHandler> inputDictionary;
        private readonly List<KeyCode> keyCodeList = new List<KeyCode>
        {
            A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,Tab,LeftShift,LeftControl,LeftAlt,Mouse0,Mouse1,Mouse2,Mouse3,Mouse4,Mouse5,Mouse6,KeyCode.Space,Return,Escape,
            Alpha0,Alpha1,Alpha2,Alpha3,Alpha4,Alpha5,Alpha6,Alpha7,Alpha8,Alpha9,F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,F11,F12
        };

        private InputHandler cameraMoveUp;
        private InputHandler cameraMoveDown;
        private InputHandler cameraMoveRight;
        private InputHandler cameraMoveLeft;
        private InputHandler cameraMoveIn;
        private InputHandler cameraMoveOut;

        GameManager gameManager;

        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            cameraMoveUp += () => gameManager.cameraController.CameraMoveUp();
            cameraMoveDown += () => gameManager.cameraController.CameraMoveDown();
            cameraMoveRight += () => gameManager.cameraController.CameraMoveRight();
            cameraMoveLeft += () => gameManager.cameraController.CameraMoveLeft();
            cameraMoveIn += () => gameManager.cameraController.CameraMoveIn();
            cameraMoveOut += () => gameManager.cameraController.CameraMoveOut();

            inputDictionary = new Dictionary<KeyCode, InputHandler>
            {
                { W, cameraMoveIn },
                { D, cameraMoveRight },
                { E, cameraMoveUp },
                { A, cameraMoveLeft },
                { S, cameraMoveDown },
                { Q, cameraMoveOut }
            };
        }

        private void Update()
        {
            InputHandler inputHandler;
            foreach (var keyCode in keyCodeList)
            {
                if (GetKey(keyCode))
                {
                    if (inputDictionary.TryGetValue(keyCode, out inputHandler))
                        inputHandler?.Invoke();
                }
            }
        }
    }
}