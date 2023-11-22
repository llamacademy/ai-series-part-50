using CrashKonijn.Goap.Interfaces;

namespace LlamAcademy.GOAP.Actions
{
    public class CommonData : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}