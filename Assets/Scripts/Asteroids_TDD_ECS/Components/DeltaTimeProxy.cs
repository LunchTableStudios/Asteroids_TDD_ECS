namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    [ System.Serializable ]
    public struct DeltaTime : IComponentData
    {
        public float Value;
    }

    [RequiresEntityConversion]
    public class DeltaTimeProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public DeltaTime Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            DeltaTime data = new DeltaTime{ Value = Data.Value };
            manager.AddComponentData( entity, data );
        }
    }
}