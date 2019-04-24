namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    [ System.Serializable ]
    public struct RotationSpeed : IComponentData
    {
        public float Value;
    }
}