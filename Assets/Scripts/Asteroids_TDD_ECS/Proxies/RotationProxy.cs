namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class RotationProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        private RotationInput m_rotationInputData;

        public RotationSpeed SpeedData;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, m_rotationInputData );
            manager.AddComponentData( entity, SpeedData );
        }
    }
}