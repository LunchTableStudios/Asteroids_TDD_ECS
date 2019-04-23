namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class MovementSpeedProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public MovementSpeed Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, Data );
        }
    }
}