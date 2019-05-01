namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    [ System.Serializable ]
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
}