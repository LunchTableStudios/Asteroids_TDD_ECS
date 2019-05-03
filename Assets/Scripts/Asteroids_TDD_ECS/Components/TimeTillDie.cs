namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    [System.Serializable ]
    public struct TimeTillDie : IComponentData
    {
        public float Value;
    }
}