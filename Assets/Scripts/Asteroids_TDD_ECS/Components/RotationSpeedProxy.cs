namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;
    
    [ System.Serializable ]
    public struct RotationSpeed : IComponentData
    {
        public float Value;
    }

    [RequiresEntityConversion]
    public class RotationSpeedProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public RotationSpeed Data;
        
        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            RotationSpeed data = new RotationSpeed { Value = Data.Value };
            manager.AddComponentData( entity, data );
        }
    }
}