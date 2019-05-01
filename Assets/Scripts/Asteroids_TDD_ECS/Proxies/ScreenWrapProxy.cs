namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class ScreenWrapProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public HorizontalScreenWrap HorizontalScreenWrapData;
        public VerticalScreenWrap VerticalScreenWrapData;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, HorizontalScreenWrapData );
            manager.AddComponentData( entity, VerticalScreenWrapData );
        }
    }
}