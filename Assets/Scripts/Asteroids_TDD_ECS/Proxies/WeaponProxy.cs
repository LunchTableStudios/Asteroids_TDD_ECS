namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;

    public class WeaponProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        private ShootInput m_shootInputData;

        public Weapon WeaponData;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem convertionSystem )
        {
            manager.AddComponentData( entity, m_shootInputData );
            manager.AddComponentData( entity, WeaponData );
        }
    }
}