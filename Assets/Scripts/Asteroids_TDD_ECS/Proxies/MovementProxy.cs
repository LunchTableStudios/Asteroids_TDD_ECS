namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class MovementProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        private MovementInput m_inputData;

        public MovementSpeed SpeedData;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, m_inputData );
            manager.AddComponentData( entity, SpeedData );
        }
    }
}