using System;
using LlamAcademy.GOAP.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LlamAcademy.GOAP.Behaviors
{
    public class SalivaBehavior : MonoBehaviour
    {
        [field: SerializeField] public float Saliva { get; set; }
        [SerializeField] private BioSignsSO BioSigns;

        private void Awake()
        {
            Saliva = Random.Range(0, 5);
        }

        private void Update()
        {
            Saliva = Mathf.Clamp(Saliva - Time.deltaTime * BioSigns.SalivaDepletionRate, 0, float.MaxValue);
        }
    }
}