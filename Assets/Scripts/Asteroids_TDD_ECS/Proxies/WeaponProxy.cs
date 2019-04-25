namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class WeaponProxy : MonoBehaviour, IConvertGameObjectToEntity
    {   
        public Weapon Data;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, Data );
        }
    }
}