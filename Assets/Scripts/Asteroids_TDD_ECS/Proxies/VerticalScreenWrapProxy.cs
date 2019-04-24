namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class VerticalScreenWrapProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public VerticalScreenWrap Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, Data );
        }
    }
}