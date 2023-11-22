using UnityEngine;

namespace LlamAcademy.GOAP.Behaviors
{
    public class SpitBehavior : MonoBehaviour
    {
        [field: SerializeField] private Transform SpawnLocation;

        public delegate void SpawnSpitEvent(Vector3 spawnLocation, Vector3 forward);
        public event SpawnSpitEvent OnSpawnSpit;
        
        public void BeginSpit(int _)
        {
            OnSpawnSpit?.Invoke(SpawnLocation.position, SpawnLocation.forward);
        }
    }
}