namespace Asteroids_TDD_ECS
{
    using Unity.Entities;

    public struct WeaponFired : IComponentData
    {
        public float TimeOfFire;
    }
}