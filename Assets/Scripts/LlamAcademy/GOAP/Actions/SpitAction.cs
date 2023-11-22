using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using LlamAcademy.GOAP.Behaviors;
using LlamAcademy.GOAP.Config;
using UnityEngine;
using UnityEngine.Pool;

namespace LlamAcademy.GOAP.Actions
{
    public class SpitAction : ActionBase<SpitAction.Data>, IInjectable
    {
        private AttackConfigSO AttackConfig;
        private ObjectPool<Spit> Pool;
        
        public override void Created()
        {
            Pool = new(CreateObject);
        }

        private Spit CreateObject()
        {
            return GameObject.Instantiate(AttackConfig.SpitPrefab);
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = AttackConfig.AttackDelay;
            data.SpitBehavior.OnSpawnSpit += SpitBehaviorOnSpawnSpit;
            data.SalivaBehavior.Saliva -= 1;
        }

        private void SpitBehaviorOnSpawnSpit(Vector3 spawnLocation, Vector3 forward)
        {
            Spit instance = Pool.Get();
            instance.transform.position = spawnLocation;
            instance.transform.forward = forward;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            bool shouldAttack = data.Target != null
                                && Vector3.Distance(data.Target.Position, agent.transform.position) <=
                                AttackConfig.RangeAttackRadius;
            
            agent.transform.LookAt(data.Target.Position);
            data.Animator.SetBool(AttackData.ATTACK, shouldAttack);

            return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            data.Animator.SetBool(AttackData.ATTACK, false);
            data.SpitBehavior.OnSpawnSpit -= SpitBehaviorOnSpawnSpit;
        }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
        
        public class Data : AttackData
        {
            [GetComponent]
            public SpitBehavior SpitBehavior { get; set; }
            [GetComponent]
            public SalivaBehavior SalivaBehavior { get; set; }
        }
    }
}