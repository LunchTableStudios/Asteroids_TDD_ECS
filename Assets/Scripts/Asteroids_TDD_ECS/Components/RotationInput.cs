namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    [ System.Serializable ]
    public struct RotationInput : IComponentData
    {
        public float Value;
    }
}