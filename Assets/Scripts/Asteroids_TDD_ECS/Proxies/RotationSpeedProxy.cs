namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class RotationSpeedProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public RotationSpeed Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, Data );
        }
    }
}