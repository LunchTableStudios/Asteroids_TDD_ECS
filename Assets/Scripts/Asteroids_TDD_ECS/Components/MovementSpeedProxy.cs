namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }

    public class MovementSpeedProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            
        }
    }
}