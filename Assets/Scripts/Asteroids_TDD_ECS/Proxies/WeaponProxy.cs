namespace Asteroids_TDD_ECS
{
    using System.Collections.Generic;
    using UnityEngine;
    using Unity.Entities;

    public class WeaponProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        private ShootInput m_shootInputData;

        public GameObject ProjectilePrefab;
        public Weapon WeaponData;

        public void DeclareReferencedPrefabs( List<GameObject> gameObjects )
        {
            gameObjects.Add( ProjectilePrefab );
        }

        public void Convert( Entity entity, EntityManager manager, GameObjectConversionSystem convertionSystem )
        {
            manager.AddComponentData( entity, m_shootInputData );

            WeaponData.ProjectilePrefab = convertionSystem.GetPrimaryEntity( ProjectilePrefab );
            manager.AddComponentData( entity, WeaponData );
        }
    }
}