namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    public struct ShootInput : IComponentData
    {
        public bool IsShooting;
    }
}