namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    [ System.Serializable ]
    public struct MovementInput : IComponentData
    {
        public float Value;
    }
}