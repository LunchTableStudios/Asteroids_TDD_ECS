namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    [ System.Serializable ]
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }

    public class MovementSpeedProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public MovementSpeed Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, Data );
        }
    }
}