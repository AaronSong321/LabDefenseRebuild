using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class TurretCube : MonoBehaviour
    {
        GameManager gameManager;

        [HideInInspector] public Turret turret;
        private Renderer mapRenderer;

        protected virtual void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            mapRenderer = GetComponent<Renderer>();
        }

        protected virtual void Start()
        {
            mapRenderer.material.color = gameManager.buildManager.normalColor;
        }

        public virtual Turret BuildTurret(string uniqueTurretName)
        {
            var turrets = gameManager.turretRepository.turrets;
            foreach (var turret in turrets)
                if (turret[0].uniqueTurretName == uniqueTurretName)
                {
                    if (gameManager.gameMode.currentCash <= turret[0].cost)
                        return null;
                    var newTurret = Instantiate(turret[0].turretPrefab, gameObject.transform.position, Quaternion.identity);
                    this.turret = newTurret.GetComponent<Turret>();
                    this.turret.currentLevel = 1;
                    gameManager.buildManager.ModifyTurret(this.turret, -turret[0].cost);
                    return this.turret;
                }
            throw new UnityException($"Cannot find turret {uniqueTurretName} in GameManager.TurretRepository.");
        }

        public virtual Turret UpgradeTurret()
        {
            var turrets = gameManager.turretRepository.turrets;
            foreach (var turret in turrets) 
                if (turret[0].uniqueTurretName == this.turret.uniqueTurretName)
                {
                    if (gameManager.gameMode.currentCash <= turret[this.turret.currentLevel].cost)
                        return null;
                    var newTurret = Instantiate(turret[this.turret.currentLevel].turretPrefab
                        , gameObject.transform.position, Quaternion.identity);
                    newTurret.GetComponent<Turret>().currentLevel = this.turret.currentLevel + 1;
                    Destroy(this.turret.gameObject);
                    this.turret = newTurret.GetComponent<Turret>();
                    gameManager.buildManager.ModifyTurret(this.turret, -turret[this.turret.currentLevel - 1].cost);
                    return this.turret;
                }
            throw new UnityException($"Cannot find turret {turret.uniqueTurretName} in GameManager.TurretRepository.");
        }

        public virtual void SellTurret()
        {
            var turrets = gameManager.turretRepository.turrets;
            foreach (var turret in turrets)
                if (turret[0].uniqueTurretName == this.turret.uniqueTurretName)
                {
                    var sum = 0;
                    for (int i = 0; i < this.turret.currentLevel; i++)
                        sum += turret[i].cost;
                    Destroy(this.turret.gameObject);
                    gameManager.buildManager.ModifyTurret(null, sum);
                    return;
                }
            throw new UnityException($"Cannot find turret {turret.uniqueTurretName} in GameManager.TurretRepository.");
        }
    }
}