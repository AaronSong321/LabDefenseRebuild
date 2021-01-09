using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class GameMode : MonoBehaviour
    {
        [HideInInspector] public int maxBaseHealth;
        [HideInInspector] public int currentBaseHealth;
        [HideInInspector] public int maxWaveShield;
        [HideInInspector] public int currentShield;
        [HideInInspector] public int currentCash;
        [HideInInspector] public int currentWave;
        [HideInInspector] public List<Wave> waves;
        public int TotalWaveCount => waves.Count;

        public static float normalSpeed = 1;
        public static float doubleSpeed = 2;
        public static float tripleSpeed = 3;
        [HideInInspector] public bool bPause;
        [HideInInspector] public float gameSpeed;
        public float GameSpeed => bPause ? 0 : gameSpeed;

        [HideInInspector] public int enemyAliveCount;
        [HideInInspector] public int enemyWaveCount;

        [HideInInspector] public float sellFactor;
        [HideInInspector] public GameSettings.Difficulty difficulty;
        protected Coroutine GameCycleCoroutine;

        GameManager gameManager;

        public delegate void RefreshShieldHandler();
        public event RefreshShieldHandler RefreshShieldEvent;
        public delegate void VictoryHandler();
        public event VictoryHandler VictoryEvent;
        public delegate void DefeatHandler();
        public event DefeatHandler DefeatEvent;
        public delegate void GameSpeedChangeHandler();
        public event GameSpeedChangeHandler GameSpeedChangeEvent;

        public delegate void ShieldDecreaseHandler(Enemy enemy, float decrease);
        public event ShieldDecreaseHandler ShieldDecreaseEvent;
        public delegate void LoseHealthHandler(Enemy enemy, float damage);
        public event LoseHealthHandler LoseHealthEvent;

        public delegate void GainWaveCashHandler(Wave wave, int dosh);
        public event GainWaveCashHandler GainWaveCashEvent;
        public delegate void GainEnemyCashHandler(Enemy e, float dosh);
        public event GainEnemyCashHandler GainEnemyCashEvent;

        protected virtual void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            bPause = false;
            gameSpeed = 1;
            currentCash = 0;
            maxWaveShield = 0;
            currentShield = 0;
            currentWave = 0;
            maxBaseHealth = 100;
            currentBaseHealth = maxBaseHealth;
            enemyAliveCount = 0;
            enemyWaveCount = 0;
            sellFactor = 0.5f;

            difficulty = GameSettings.Difficulty.Hypothetical;
        }

        protected virtual void Start()
        {
            gameManager.player.currentPerk.IsActive = true;
            GameCycleCoroutine = StartCoroutine(StartGameCycle());
            gameManager.enemySpawner.SpawnEnemyEvent += (enemy) =>
            {
                enemy.EnemyKilledEvent += (e) =>
                {
                    enemyAliveCount--;
                    currentCash += e.rewardcash;
                    GainEnemyCashEvent?.Invoke(e, e.rewardcash);
                    gameManager.viewSoloGame.RefreshWaveStatus();
                };
                enemy.EnemyDiscardEvent += (e) =>
                {
                    enemyAliveCount--;
                    gameManager.viewSoloGame.RefreshWaveStatus();
                };
                enemy.ReachDestinationEvent += (e) =>
                {
                    var dmg = e.currentDamage;
                    if (currentShield > 0)
                    {
                        dmg = dmg > currentShield ? currentShield : dmg;
                        currentShield -= dmg;
                        ShieldDecreaseEvent?.Invoke(e, dmg);
                    }
                    else
                    {
                        dmg = dmg > currentBaseHealth ? currentBaseHealth : dmg;
                        currentBaseHealth -= dmg;
                        LoseHealthEvent?.Invoke(e, dmg);
                        if (currentBaseHealth == 0)
                        {
                            gameSpeed = 0;
                            if (GameCycleCoroutine != null)
                                StopCoroutine(GameCycleCoroutine);
                            Destroy(gameManager.endInAir.gameObject);
                            Destroy(gameManager.endOnGround.gameObject);
                            DefeatEvent?.Invoke();
                        }
                    }
                };
            };
            gameManager.buildManager.ModifyTurretEvent += (@new, doshFluctuation) =>
            {
                currentCash += doshFluctuation;
                gameManager.viewSoloGame.RefreshBaseStatus();
            };
            DefeatEvent += () =>
            {
                if (GameCycleCoroutine != null)
                    StopCoroutine(GameCycleCoroutine);
            };
        }

        protected virtual IEnumerator StartGameCycle()
        {
            waves = gameManager.waveGenerator.GenerateAllWaves();
            foreach (var wave in waves)
            {
                gameManager.viewSoloGame.RefreshWaveStatus();
                currentCash += wave.initCash;
                GainWaveCashEvent?.Invoke(wave, wave.initCash);
                currentShield = 0;
                RefreshShieldEvent?.Invoke();
                maxWaveShield = currentShield;
                enemyWaveCount = 0;
                gameManager.viewSoloGame.RefreshWaveStatus();
                yield return new WaitForSeconds(wave.waveInterval);

                //begin spawning enemies
                enemyWaveCount = wave.enemies.Count;
                enemyAliveCount = enemyWaveCount;
                currentWave++;
                gameManager.viewSoloGame.RefreshWaveStatus();
                yield return StartCoroutine(gameManager.enemySpawner.SpawnEnemy(wave));
                while (enemyAliveCount > 0)
                    yield return 0;
                gameManager.viewSoloGame.RefreshWaveStatus();
            }
            VictoryEvent?.Invoke();
        }

        public void PauseGame()
        {
            if (bPause == false)
            {
                bPause = true;
                GameSpeedChangeEvent?.Invoke();
            }
        }

        public void ResumeGame()
        {
            if (bPause == true)
            {
                bPause = false;
                GameSpeedChangeEvent?.Invoke();
            }
        }

        public void NormalSpeed()
        {
            if (Mathf.Approximately(gameSpeed, normalSpeed))
            {
                gameSpeed = normalSpeed;
                GameSpeedChangeEvent?.Invoke();
            }
        }
        public void DoubleSpeed()
        {
            if (Mathf.Approximately(gameSpeed, doubleSpeed))
            {
                gameSpeed = doubleSpeed;
                GameSpeedChangeEvent?.Invoke();
            }
        }
        public void TripleSpeed()
        {
            if (Mathf.Approximately(gameSpeed, tripleSpeed))
            {
                gameSpeed = tripleSpeed;
                GameSpeedChangeEvent?.Invoke();
            }
        }
    }
}