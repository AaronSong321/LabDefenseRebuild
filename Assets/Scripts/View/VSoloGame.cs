using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aaron.LabDefenseRebuild
{
    public sealed class VSoloGame : MonoBehaviour
    {
        GameManager gameManager;

        #region CanvasGroup Base Status Definition
        CanvasGroup CGBaseStauts;
        Image IShieldBar;
        Text XShield;
        Image IBaseHealthBar;
        Text XBaseHealth;
        Text XCash;
        readonly Color greenHealthBar = Color.green;
        readonly Color yellowHealthBar = Color.yellow;
        readonly Color redHealthBar = Color.red;
        public void RefreshBaseStatus()
        {
            if (gameManager.gameMode.currentShield == 0)
            {
                XShield.text = "No Shell";
            }
            else
            {
                XShield.text = gameManager.gameMode.currentShield.ToString();
                var shieldBlue = Color.blue;
                shieldBlue.a = gameManager.gameMode.currentShield / gameManager.gameMode.maxWaveShield;
                IShieldBar.color = shieldBlue;
            }

            var cbh = gameManager.gameMode.currentBaseHealth;
            var mbh = gameManager.gameMode.maxBaseHealth;
            XBaseHealth.text = $"{cbh}/{mbh}";
            var healthRatio = 1.0f * cbh / mbh;
            IBaseHealthBar.rectTransform.localScale = new Vector3(healthRatio, 1, 1);
            if (healthRatio >= 0 && healthRatio < 0.5)
            {
                IBaseHealthBar.color = Color.Lerp(redHealthBar, yellowHealthBar, healthRatio * 2);
            }
            else
                IBaseHealthBar.color = Color.Lerp(yellowHealthBar, greenHealthBar, healthRatio + healthRatio - 1);

            XCash.text = $"${gameManager.gameMode.currentCash.ToString()}";
        }
        #endregion

        #region CanvasGroup Wave Status Definition
        CanvasGroup CGWaveStauts;
        Text XWave;
        Text XEnemyLeft;

        public void RefreshWaveStatus()
        {
            XWave.text = $"{gameManager.gameMode.currentWave}/{gameManager.gameMode.TotalWaveCount}";
            XEnemyLeft.text = $"{gameManager.gameMode.enemyAliveCount}/{gameManager.gameMode.enemyWaveCount}";
        }
        #endregion

        #region CanvasGroup Turrets
        CanvasGroup CGTurrets;
        List<Image> ITurret;
        List<Button> BTurretChosen;
        List<Text> XTurretName;
        List<Text> XTurretCost;

        public void ReadTurretFromRepository()
        {
            var turrets = gameManager.turretRepository.turrets;

            foreach (var turret in turrets)
            {
                var xName = GameObject.Find($"CMain/CGTurrets/{turret[0].uniqueTurretName}/XName").GetComponent<Text>();
                var xCost = GameObject.Find($"CMain/CGTurrets/{turret[0].uniqueTurretName}/XCost").GetComponent<Text>();
                xName.text = turret[0].uniqueTurretName;
                xCost.text = $"${turret[0].cost}";
                XTurretCost.Add(xCost);
                XTurretName.Add(xName);
                var button = GameObject.Find($"CMain/CGTurrets/{turret[0].uniqueTurretName}/Image").GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    var index = BTurretChosen.IndexOf(button);
                    gameManager.buildManager.ChooseTurret(XTurretName[index].text);
                    foreach (var text in XTurretName)
                        text.color = Color.black;
                    XTurretName[index].color = Color.blue;
                });
                BTurretChosen.Add(button);
                var image = GameObject.Find($"CMain/CGTurrets/{turret[0].uniqueTurretName}/Image").GetComponent<Image>();
                image.sprite = Resources.Load<Sprite>($"Icons/Turrets/{turret[0].uniqueTurretName}/GameButton");
                ITurret.Add(image);
            }
        }
        #endregion

        #region CanvasGroup Upgrade definition
        CanvasGroup CGUpgrade;
        Button BUpgrade;
        Text XUpgradeDosh;
        Button BSell;
        Text XSellDosh;
        Text XChosenTurret;

        public void ShowUpgradeUI(Turret turret)
        {
            if (turret.currentLevel == turret.maxLevel)
            {
                XChosenTurret.text = $"{turret.uniqueTurretName} Level{turret.currentLevel}";
                BUpgrade.gameObject.SetActive(false);
                foreach (var turretInfo in gameManager.turretRepository.turrets)
                {
                    if (turretInfo[0].uniqueTurretName == turret.uniqueTurretName)
                    {
                        var sum = 0;
                        for (int i = 0; i < turret.currentLevel; i++)
                            sum += turretInfo[i].cost;
                        XSellDosh.text = $"+${sum * gameManager.gameMode.sellFactor}";
                    }
                }
            }
            else
            {
                BUpgrade.gameObject.SetActive(true);
                foreach (var turretInfo in gameManager.turretRepository.turrets)
                {
                    if (turretInfo[0].uniqueTurretName == turret.uniqueTurretName)
                    {
                        var sum = 0;
                        for (int i = 0; i <= turret.currentLevel; i++)
                            sum += turretInfo[i].cost;
                        XSellDosh.text = $"+${sum * gameManager.gameMode.sellFactor}";
                        XUpgradeDosh.text = $"-${turretInfo[turret.currentLevel].cost}";
                    }
                }
            }
            CGUpgrade.gameObject.SetActive(true);
        }
        #endregion

        #region CanvasGroup Stop Menu definition
        CanvasGroup CGStopMenu;
        Button BMenu;
        Button BPause;
        Button BNormalSpeed;
        Button BDoubleSpeed;
        Button BTripleSpeed;
        #endregion

        #region CanvasGroup End Game definition
        CanvasGroup CGEndGame;
        Text XGameStatus;
        Button BReturnMainMenu;
        #endregion

        CanvasGroup CGPauseGame;
        Button BResumeGame;

        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            #region CanvasGroup Base Status Processing
            CGBaseStauts = GameObject.Find("CMain/CGBaseStatus").GetComponent<CanvasGroup>();
            IShieldBar = GameObject.Find("CMain/CGBaseStatus/IShieldBackground/IShieldBar").GetComponent<Image>();
            XShield = GameObject.Find("CMain/CGBaseStatus/IShieldBackground/XShield").GetComponent<Text>();
            IBaseHealthBar = GameObject.Find("CMain/CGBaseStatus/IBaseHealthBackground/IBaseHealthBar").GetComponent<Image>();
            XBaseHealth = GameObject.Find("CMain/CGBaseStatus/IBaseHealthBackground/XBaseHealth").GetComponent<Text>();
            XCash = GameObject.Find("CMain/CGBaseStatus/XCash").GetComponent<Text>();
            #endregion

            #region CanvasGroup Wave Status Processing
            CGWaveStauts = GameObject.Find("CMain/CGWaveStatus").GetComponent<CanvasGroup>();
            XWave = GameObject.Find("CMain/CGWaveStatus/XWave").GetComponent<Text>();
            XEnemyLeft = GameObject.Find("CMain/CGWaveStatus/XEnemyLeft").GetComponent<Text>();
            #endregion

            #region CanvasGroup Upgrade Processing
            CGUpgrade = GameObject.Find("CMain/CGUpgrade").GetComponent<CanvasGroup>();
            BUpgrade = GameObject.Find("CMain/CGUpgrade/BUpgrade").GetComponent<Button>();
            XUpgradeDosh = GameObject.Find("CMain/CGUpgrade/BUpgrade/XUpgradeDosh").GetComponent<Text>();
            BSell = GameObject.Find("CMain/CGUpgrade/BSell").GetComponent<Button>();
            XSellDosh = GameObject.Find("CMain/CGUpgrade/BSell/XSellDosh").GetComponent<Text>();
            XChosenTurret = GameObject.Find("CMain/CGUpgrade/XChosenTurret").GetComponent<Text>();
            #endregion

            CGTurrets = GameObject.Find("CMain/CGTurrets").GetComponent<CanvasGroup>();
            ITurret = new List<Image>();
            BTurretChosen = new List<Button>();
            XTurretName = new List<Text>();
            XTurretCost = new List<Text>();

            CGStopMenu = GameObject.Find("CMain/CGStopMenu").GetComponent<CanvasGroup>();
            BMenu = GameObject.Find("CMain/CGStopMenu/BMenu").GetComponent<Button>();
            BPause = GameObject.Find("CMain/CGStopMenu/BPause").GetComponent<Button>();
            BNormalSpeed = GameObject.Find("CMain/CGStopMenu/BNormalSpeed").GetComponent<Button>();
            BDoubleSpeed = GameObject.Find("CMain/CGStopMenu/BDoubleSpeed").GetComponent<Button>();
            BTripleSpeed = GameObject.Find("CMain/CGStopMenu/BTripleSpeed").GetComponent<Button>();

            CGPauseGame = GameObject.Find("CMain/CGPauseGame").GetComponent<CanvasGroup>();
            BResumeGame = GameObject.Find("CMain/CGPauseGame/BResumeGame").GetComponent<Button>();

            CGEndGame = GameObject.Find("CMain/CGEndGame").GetComponent<CanvasGroup>();
            XGameStatus = GameObject.Find("CMain/CGEndGame/XGameStatus").GetComponent<Text>();
            BReturnMainMenu = GameObject.Find("CMain/CGEndGame/BReturnMainMenu").GetComponent<Button>();
        }

        private void Start()
        {
            gameManager.gameMode.GainEnemyCashEvent += (enemy, dosh) =>
            {
                RefreshBaseStatus();
            };
            gameManager.gameMode.GainWaveCashEvent += (wave, dosh) =>
            {
                RefreshBaseStatus();
            };
            gameManager.gameMode.LoseHealthEvent += (enemy, damage) =>
            {
                RefreshBaseStatus();
            };
            gameManager.gameMode.ShieldDecreaseEvent += (enemy, decrease) =>
            {
                RefreshBaseStatus();
            };

            #region CanvasGroup Turrets Processing
            gameManager.buildManager.ModifyTurretEvent += (@new, doshFluctuation) =>
            {
                if (@new != null && @new.currentLevel == 1)
                {
                    foreach (var text in XTurretName)
                        text.color = Color.black;
                }
                RefreshBaseStatus();
            };
            #endregion

            #region CanvasGroup Stop Menu processing
            BPause.onClick.AddListener(() =>
            {
                CGBaseStauts.interactable = false;
                CGWaveStauts.interactable = false;
                CGUpgrade.interactable = false;
                CGStopMenu.interactable = false;
                CGTurrets.interactable = false;
                CGPauseGame.gameObject.SetActive(true);
                gameManager.gameMode.PauseGame();
            });
            BNormalSpeed.onClick.AddListener(() =>
            {
                gameManager.gameMode.NormalSpeed();
            });
            BDoubleSpeed.onClick.AddListener(() =>
            {
                gameManager.gameMode.DoubleSpeed();
            });
            BTripleSpeed.onClick.AddListener(() =>
            {
                gameManager.gameMode.TripleSpeed();
            });
            #endregion

            BResumeGame.onClick.AddListener(() =>
            {
                CGBaseStauts.interactable = true;
                CGWaveStauts.interactable = true;
                CGUpgrade.interactable = true;
                CGStopMenu.interactable = true;
                CGTurrets.interactable = true;
                CGPauseGame.gameObject.SetActive(false);
                gameManager.gameMode.ResumeGame();
            });
            CGPauseGame.gameObject.SetActive(false);

            #region CanvasGroup Upgrade processing
            BUpgrade.onClick.AddListener(() =>
            {
                gameManager.buildManager.chosenTurretCube.UpgradeTurret();
                CGUpgrade.gameObject.SetActive(false);
            });
            BSell.onClick.AddListener(() =>
            {
                gameManager.buildManager.chosenTurretCube.SellTurret();
                CGUpgrade.gameObject.SetActive(false);
            });
            CGUpgrade.gameObject.SetActive(false);
            #endregion

            #region CanvasGroup End Game processing
            CGEndGame.gameObject.SetActive(false);
            BReturnMainMenu.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("ChoosingPerk");
            });
            gameManager.gameMode.VictoryEvent += () =>
            {
                CGBaseStauts.interactable = false;
                CGWaveStauts.interactable = false;
                CGUpgrade.interactable = false;
                CGStopMenu.interactable = false;
                CGTurrets.interactable = false;
                CGEndGame.gameObject.SetActive(true);
                XGameStatus.text = "Victory";
                XGameStatus.color = Color.cyan;
            };
            gameManager.gameMode.DefeatEvent += () =>
            {
                CGBaseStauts.interactable = false;
                CGWaveStauts.interactable = false;
                CGUpgrade.interactable = false;
                CGStopMenu.interactable = false;
                CGTurrets.interactable = false;
                CGEndGame.gameObject.SetActive(true);
                XGameStatus.text = "Defeat";
                XGameStatus.color = Color.red;
            };
            #endregion
        }
    }
}