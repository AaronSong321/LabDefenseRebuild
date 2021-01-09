using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class GameManager : MonoBehaviour
    {
        [HideInInspector] public GameMode gameMode;
        [HideInInspector] public WaveGenerator waveGenerator;
        [HideInInspector] public EnemySpawner enemySpawner;
        [HideInInspector] public TurretRepository turretRepository;
        [HideInInspector] public VSoloGame viewSoloGame;
        [HideInInspector] public BuildManager buildManager;
        [HideInInspector] public CameraController cameraController;
        [HideInInspector] public InputController inputController;
        [HideInInspector] public Player player;
        //[HideInInspector] public Perk perk;

        [HideInInspector] public Transform startOnGround;
        [HideInInspector] public Transform startInAir;
        [HideInInspector] public Transform endOnGround;
        [HideInInspector] public Transform endInAir;

        private void Awake()
        {
            gameMode = GameObject.Find("GameManager/GameMode").GetComponent<GameMode>();
            waveGenerator = GameObject.Find("GameManager/WaveGenerator").GetComponent<WaveGenerator>();
            enemySpawner = GameObject.Find("GameManager/EnemySpawner").GetComponent<EnemySpawner>();
            turretRepository = GameObject.Find("GameManager/TurretRepository").GetComponent<TurretRepository>();
            viewSoloGame = GameObject.Find("GameManager/ViewSoloGame").GetComponent<VSoloGame>();
            buildManager = GameObject.Find("GameManager/BuildManager").GetComponent<BuildManager>();
            cameraController = GameObject.Find("GameManager/CameraController").GetComponent<CameraController>();
            inputController = GameObject.Find("GameManager/InputController").GetComponent<InputController>();
            player = GameObject.Find("Player").GetComponent<Player>();

            startOnGround = GameObject.Find("Landmarks/StartOnGround").transform;
            startInAir = GameObject.Find("Landmarks/StartInAir").transform;
            endOnGround = GameObject.Find("Landmarks/EndOnGround").transform;
            endInAir = GameObject.Find("Landmarks/EndInAir").transform;
        }

        private void Start()
        {
            player.GetGameManager(this);
        }
    }
}