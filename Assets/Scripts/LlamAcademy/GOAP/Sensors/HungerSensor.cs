using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using LlamAcademy.GOAP.Behaviors;
using UnityEngine;

namespace LlamAcademy.GOAP.Sensors
{
    public class HungerSensor : LocalWorldSensorBase
    {
        public override void Created() {}

        public override void Update() {}

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return new SenseValue(Mathf.CeilToInt(references.GetCachedComponent<HungerBehavior>().Hunger));
        }
    }
}