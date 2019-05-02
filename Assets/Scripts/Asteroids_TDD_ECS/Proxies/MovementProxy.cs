namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class MovementProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Movement MovementData;
        public MovementSpeed SpeedData;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, MovementData );
            manager.AddComponentData( entity, SpeedData );
        }
    }
}