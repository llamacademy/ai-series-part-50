using CrashKonijn.Goap.Behaviours;
using LlamAcademy.GOAP.Config;
using LlamAcademy.GOAP.Goals;
using LlamAcademy.Sensors;
using UnityEngine;

namespace LlamAcademy.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class LlamaBrain : MonoBehaviour
    {
        [SerializeField] private PlayerSensor PlayerSensor;
        [SerializeField] private HungerBehavior Hunger;
        [SerializeField] private AttackConfigSO AttackConfig;
        [SerializeField] private BioSignsSO BioSigns;
        private AgentBehaviour AgentBehavior;
        private bool PlayerIsInRange;

        private void Awake()
        {
            AgentBehavior = GetComponent<AgentBehaviour>();
        }
        
        private void Start()
        {
            AgentBehavior.SetGoal<WanderGoal>(false);

            PlayerSensor.Collider.radius = AttackConfig.SensorRadius;
        }

        private void Update()
        {
            SetGoal();
        }

        private void SetGoal()
        {
            if (Hunger.Hunger > BioSigns.MaxHunger)
            {
                AgentBehavior.SetGoal<EatGoal>(true);
            }
            else if (Hunger.Hunger < BioSigns.AcceptableHungerLimit && AgentBehavior.CurrentGoal is EatGoal && PlayerIsInRange)
            {
                AgentBehavior.SetGoal<KillPlayer>(false);
            }
            else if (Hunger.Hunger <= 0 && AgentBehavior.CurrentGoal is EatGoal && !PlayerIsInRange)
            {
                AgentBehavior.SetGoal<WanderGoal>(false);
            }
        }

        private void OnEnable()
        {
            PlayerSensor.OnPlayerEnter += PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit += PlayerSensorOnPlayerExit;
        }

        private void OnDisable()
        {
            PlayerSensor.OnPlayerEnter -= PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit -= PlayerSensorOnPlayerExit;
        }

        private void PlayerSensorOnPlayerExit(Vector3 lastKnownPosition)
        {
            PlayerIsInRange = false;
            if (Hunger.Hunger > 0 && AgentBehavior.CurrentGoal is EatGoal)
            {
                return;
            }
            
            AgentBehavior.SetGoal<WanderGoal>(true);
        }

        private void PlayerSensorOnPlayerEnter(Transform Player)
        {
            PlayerIsInRange = true;
            if (Hunger.Hunger > BioSigns.AcceptableHungerLimit && AgentBehavior.CurrentGoal is EatGoal)
            {
                return;
            }
            
            AgentBehavior.SetGoal<KillPlayer>(true);
        }
    }
}