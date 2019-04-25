namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;
    
    public struct MovementInput : IComponentData
    {
        public float Value;
    }
}