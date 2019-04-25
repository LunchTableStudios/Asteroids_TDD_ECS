namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    [ System.Serializable ]
    public struct Weapon : IComponentData
    {
        public float ShotsPerSecond;
    }
}