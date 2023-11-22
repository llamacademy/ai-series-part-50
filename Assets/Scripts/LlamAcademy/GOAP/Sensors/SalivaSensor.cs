using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using LlamAcademy.GOAP.Behaviors;
using UnityEngine;

namespace LlamAcademy.GOAP.Sensors
{
    public class SalivaSensor : LocalWorldSensorBase
    {
        public override void Created() {}

        public override void Update() {}

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return new SenseValue(Mathf.FloorToInt(references.GetCachedComponent<SalivaBehavior>().Saliva));
        }
    }
}