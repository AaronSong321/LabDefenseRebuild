using System;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    [Obsolete]
    public class Destination : MonoBehaviour
    {
        public delegate void EnemyReachDestinationHandler(Enemy enemy);
        public event EnemyReachDestinationHandler EnemyReachDestinationEvent;
        public delegate void EnemyDiscardHandler(Enemy enemy, Destination dest);
        public event EnemyDiscardHandler EnemyDiscardEvent;

        private void OnTriggerEnter(Collider collision)
        {
            var category = collision.gameObject.GetComponent<Enemy>();
            if (category is Enemy)
            {
                var enemy = category as Enemy;
                EnemyReachDestinationEvent?.Invoke(enemy);
                EnemyDiscardEvent?.Invoke(enemy, this);
                Destroy(enemy.gameObject);
            }
        }
    }
}