using System.Collections;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class EnemySpawner : MonoBehaviour
    {
        GameManager gameManager;

        public delegate void SpawnEnemyHandler(Enemy enemy);
        public event SpawnEnemyHandler SpawnEnemyEvent;

        protected virtual void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public IEnumerator SpawnEnemy(Wave wave)
        {
            var enemies = wave.enemies;
            foreach (var enemy in enemies)
            {
                GameObject newEnemy = null;
                if (enemy.GetComponent<Enemy>().flyable == Enemy.Flyable.Ground)
                    newEnemy = Instantiate(enemy, gameManager.startOnGround.position, Quaternion.identity);
                else if (enemy.GetComponent<Enemy>().flyable == Enemy.Flyable.Air)
                    newEnemy = Instantiate(enemy, gameManager.startInAir.position, Quaternion.identity);
                SpawnEnemyEvent?.Invoke(newEnemy.GetComponent<Enemy>());
                yield return new WaitForSeconds(wave.enemyInterval);
            }
        }
    }
}
