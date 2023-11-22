using UnityEngine;

namespace LlamAcademy.GOAP.Config
{
    [CreateAssetMenu(menuName = "AI/BioSigns Config", fileName = "BioSigns Config", order = 3)]
    public class BioSignsSO : ScriptableObject
    {
        public float FoodSearchRadius = 10f;
        public LayerMask FoodLayer;
        public float HungerRestorationRate = 1f;
        public float HungerDepletionRate = 0.25f;
        public float MaxHunger = 20;
        public float AcceptableHungerLimit = 10;
        public float SalivaRestorationRate = 0.25f;
        public float SalivaDepletionRate = 0.01f;
    }
}