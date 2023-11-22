using System.Data;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using LlamAcademy.GOAP.Behaviors;
using LlamAcademy.GOAP.Config;
using UnityEngine;

namespace LlamAcademy.GOAP.Actions
{
    public class EatAction : ActionBase<EatAction.Data>, IInjectable
    {
        private BioSignsSO BioSigns;
        private static readonly int IS_EATING = Animator.StringToHash("IsEating");
        
        public override void Created() {}

        public override void Start(IMonoAgent agent, Data data)
        {
            data.Hunger.enabled = false;
            data.Timer = 1f;
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;
            data.Hunger.Hunger -= context.DeltaTime * BioSigns.HungerRestorationRate;
            data.Saliva.Saliva += context.DeltaTime * BioSigns.SalivaRestorationRate;
            data.Animator.SetBool(IS_EATING, true);
            if (data.Target == null || data.Hunger.Hunger <= 0)
            {
                return ActionRunState.Stop;
            }

            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            data.Animator.SetBool(IS_EATING, false);
            data.Hunger.enabled = true;
        }
        
        public class Data : CommonData
        {
            [GetComponent]
            public Animator Animator { get; set; }
            [GetComponent]
            public HungerBehavior Hunger { get; set; }
            [GetComponent]
            public SalivaBehavior Saliva { get; set; }
        }

        public void Inject(DependencyInjector injector)
        {
            BioSigns = injector.BioSigns;
        }
    }
}