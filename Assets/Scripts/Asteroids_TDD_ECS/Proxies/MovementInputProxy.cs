namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    [RequiresEntityConversion]
    public class MovementInputProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        private MovementInput Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, Data );
        }
    }
}