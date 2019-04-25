namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public struct DeltaTime : IComponentData
    {
        public float Value;
    }
}