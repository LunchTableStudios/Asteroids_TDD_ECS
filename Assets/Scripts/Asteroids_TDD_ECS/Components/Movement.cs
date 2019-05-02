namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Mathematics;

    [ System.Serializable ]
    public struct Movement : IComponentData
    {
        public float3 Value;
    }
}