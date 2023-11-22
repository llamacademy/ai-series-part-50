using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using LlamAcademy.GOAP.Actions;
using LlamAcademy.GOAP.Goals;
using LlamAcademy.GOAP.Sensors;
using LlamAcademy.GOAP.Targets;
using LlamAcademy.GOAP.WorldKeys;
using UnityEngine;

namespace LlamAcademy.GOAP.Factories
{
    [RequireComponent(typeof(DependencyInjector))]
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        private DependencyInjector Injector;
        
        public override IGoapSetConfig Create()
        {
            Injector = GetComponent<DependencyInjector>();
            GoapSetBuilder builder = new("LlamaSet");

            BuildGoals(builder);
            BuildActions(builder);
            BuildSensors(builder);

            return builder.Build();
        }

        private void BuildGoals(GoapSetBuilder builder)
        {
            builder.AddGoal<WanderGoal>()
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

            builder.AddGoal<KillPlayer>()
                .AddCondition<PlayerHealth>(Comparison.SmallerThanOrEqual, 0);

            builder.AddGoal<EatGoal>()
                .AddCondition<Hunger>(Comparison.SmallerThanOrEqual, 0);

            builder.AddGoal<EatGoal>()
                .AddCondition<Saliva>(Comparison.GreaterThanOrEqual, 5);
        }
        
        private void BuildActions(GoapSetBuilder builder)
        {
            builder.AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .AddEffect<Saliva>(EffectType.Decrease)
                .SetBaseCost(5)
                .SetInRange(10);

            builder.AddAction<MeleeAction>()
                .SetTarget<PlayerTarget>()
                .AddEffect<PlayerHealth>(EffectType.Decrease)
                .AddEffect<Saliva>(EffectType.Decrease)
                .SetBaseCost(Injector.AttackConfig.MeleeAttackCost)
                .SetInRange(Injector.AttackConfig.SensorRadius);

            builder.AddAction<SpitAction>()
                .AddCondition<Saliva>(Comparison.GreaterThanOrEqual, 1)
                .AddCondition<PlayerDistance>(Comparison.SmallerThanOrEqual, Mathf.FloorToInt(
                    Injector.AttackConfig.RangeAttackRadius))
                .AddCondition<PlayerDistance>(Comparison.GreaterThan, Mathf.FloorToInt(
                    Injector.AttackConfig.MeleeAttackRadius))
                .SetTarget<PlayerTarget>()
                .AddEffect<PlayerHealth>(EffectType.Decrease)
                .AddEffect<Saliva>(EffectType.Decrease)
                .SetBaseCost(Injector.AttackConfig.RangeAttackCost)
                .SetInRange(Injector.AttackConfig.SensorRadius);
            
            builder.AddAction<EatAction>()
                .SetTarget<GrassTarget>()
                .AddEffect<Hunger>(EffectType.Decrease)
                .AddEffect<Saliva>(EffectType.Increase)
                .SetBaseCost(8)
                .SetInRange(1);
        }
        
        private void BuildSensors(GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            builder.AddTargetSensor<PlayerTargetSensor>()
                .SetTarget<PlayerTarget>();

            builder.AddTargetSensor<GrassTargetSensor>()
                .SetTarget<GrassTarget>();

            builder.AddWorldSensor<HungerSensor>()
                .SetKey<Hunger>();

            builder.AddWorldSensor<PlayerDistanceSensor>()
                .SetKey<PlayerDistance>();

            builder.AddWorldSensor<SalivaSensor>()
                .SetKey<Saliva>();
        }
    }
}