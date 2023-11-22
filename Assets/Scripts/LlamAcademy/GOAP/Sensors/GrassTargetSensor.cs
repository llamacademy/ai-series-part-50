using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using LlamAcademy.GOAP.Config;
using UnityEngine;

namespace LlamAcademy.GOAP.Sensors
{
    public class GrassTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private BioSignsSO BioSigns;
        private Collider[] Colliders = new Collider[5];
        
        public override void Created() {}
        public override void Update() {}

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            Vector3 agentPosition = agent.transform.position;
            int hits = Physics.OverlapSphereNonAlloc(agentPosition, BioSigns.FoodSearchRadius, Colliders,
                BioSigns.FoodLayer);

            if (hits == 0)
            {
                return null;
            }
            
            for (int i = Colliders.Length - 1; i > hits; i--)
            {
                Colliders[i] = null;
            }

            Colliders = Colliders.OrderBy(collider =>
                collider == null
                    ? float.MaxValue
                    : (collider.transform.position - agent.transform.position).sqrMagnitude
            ).ToArray();

            return new PositionTarget(Colliders[0].transform.position);
        }

        public void Inject(DependencyInjector injector)
        {
            BioSigns = injector.BioSigns;
        }
    }
}