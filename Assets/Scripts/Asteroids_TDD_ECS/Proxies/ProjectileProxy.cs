namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Physics;

    public class ProjectileProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        private Projectile m_projectileData;

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem )
        {
            manager.AddComponentData( entity, m_projectileData );
        }
    }
}