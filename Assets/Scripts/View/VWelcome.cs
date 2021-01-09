using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aaron.LabDefenseRebuild
{
    public sealed class VWelcome : MonoBehaviour
    {
        CanvasGroup CGMain;
        Button BStart;
        Button BDescription;
        Button BQuit;

        CanvasGroup CGLogin;
        InputField IFName;
        InputField IFPassword;
        Text XMessage;
        Button BLogin;
        Button BRegister;

        LoginManager loginManager;

        private void Awake()
        {
            loginManager = GameObject.Find("LoginManager").GetComponent<LoginManager>();

            #region CanvasGroup Main
            CGMain = GameObject.Find("CMain/CGMain").GetComponent<CanvasGroup>();
            BStart = GameObject.Find("CMain/CGMain/BStart").GetComponent<Button>();
            BDescription = GameObject.Find("CMain/CGMain/BDescription").GetComponent<Button>();
            BQuit = GameObject.Find("CMain/CGMain/BQuit").GetComponent<Button>();

            BStart.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("ChoosingPerk");
            });
            BDescription.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Description");
            });
            BQuit.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
            CGMain.interactable = false;
            #endregion

            #region CanvasGroup Login
            CGLogin = GameObject.Find("CMain/CGLogin").GetComponent<CanvasGroup>();
            IFName = GameObject.Find("CMain/CGLogin/IFName").GetComponent<InputField>();
            IFPassword = GameObject.Find("CMain/CGLogin/IFPassword").GetComponent<InputField>();
            XMessage = GameObject.Find("CMain/CGLogin/XMessage").GetComponent<Text>();
            BLogin = GameObject.Find("CMain/CGLogin/BLogin").GetComponent<Button>();
            BRegister = GameObject.Find("CMain/CGLogin/BRegister").GetComponent<Button>();

            IFName.ActivateInputField();
            IFName.characterLimit = 15;
            IFName.lineType = InputField.LineType.SingleLine;
            IFPassword.characterLimit = 15;
            IFPassword.lineType = InputField.LineType.SingleLine;
            IFPassword.contentType = InputField.ContentType.Password;
            CGMain.interactable = false;
            BRegister.onClick.AddListener(() =>
            {
                if (IFName.text.Length == 0)
                {
                    XMessage.text = "<color=yellow>User name cannot be null.</color>";
                    return;
                }
                if (IFPassword.text.Length < 6)
                {
                    XMessage.text = "<color=yellow>Password cannot be shorter than 6 characters.</color>";
                    return;
                }
                LoginManager.RegisterState state = loginManager.Register(IFName.text, IFPassword.text);
                switch (state)
                {
                    case LoginManager.RegisterState.AlreadyExists:
                        XMessage.text = "<color=red>User name has already existed.</color>";
                        break;
                    case LoginManager.RegisterState.Success:
                        XMessage.text = "<color=blue>Register success, please login.</color>";
                        break;
                }
            });
            BLogin.onClick.AddListener(() =>
            {
                var state = loginManager.Login(IFName.text, IFPassword.text);
                if (IFName.text.Equals("") && IFPassword.text.Equals(""))
                    state = loginManager.Login("1", "123456");
                switch (state)
                {
                    case LoginManager.LoginState.NoSuchUserName:
                        XMessage.text = "<color=red>User name does not exist.</color>";
                        break;
                    case LoginManager.LoginState.WrongPassword:
                        XMessage.text = "<color=red>Wrong password.</color>";
                        break;
                    case LoginManager.LoginState.Success:
                        CGLogin.gameObject.SetActive(false);
                        CGMain.interactable = true;
                        break;
                }
            });
            #endregion
        }

        private void Update()
        {
            if (IFName.IsActive() && Input.GetKeyDown(KeyCode.Tab))
                IFPassword.ActivateInputField();
        }
    }
}