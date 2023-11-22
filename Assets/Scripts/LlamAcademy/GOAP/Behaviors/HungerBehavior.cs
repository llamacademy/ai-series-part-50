using LlamAcademy.GOAP.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LlamAcademy.GOAP.Behaviors
{
    public class HungerBehavior : MonoBehaviour
    {
        [field: SerializeField] public float Hunger { get; set; }
        [SerializeField] private BioSignsSO BioSigns;
        
        private void Awake()
        {
            Hunger = Random.Range(0, BioSigns.MaxHunger);
        }

        private void Update()
        {
            Hunger += Time.deltaTime * BioSigns.HungerDepletionRate;
        }
    }
}