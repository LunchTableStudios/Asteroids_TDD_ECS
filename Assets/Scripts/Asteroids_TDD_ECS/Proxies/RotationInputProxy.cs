namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class RotationInputProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        private RotationInput Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, Data );
        }
    }
}