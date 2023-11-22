using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using LlamAcademy.GOAP.Config;
using UnityEngine;

namespace LlamAcademy.GOAP.Sensors
{
    public class PlayerDistanceSensor : LocalWorldSensorBase, IInjectable
    {
        private AttackConfigSO AttackConfig;
        private Collider[] Colliders = new Collider[1];
        
        public override void Created() {}
        public override void Update() {}

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            if (Physics.OverlapSphereNonAlloc(
                    agent.transform.position,
                    AttackConfig.SensorRadius,
                    Colliders,
                    AttackConfig.AttackableLayerMask
                ) > 0 && Colliders[0].TryGetComponent(out Player player))
            {
                return new SenseValue(
                    Mathf.CeilToInt(Vector3.Distance(agent.transform.position, player.transform.position))
                );
            }

            return int.MaxValue;
        }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}