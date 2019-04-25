namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class ShootInputProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, new ShootInput{ IsShooting = false } );
        }
    }
}