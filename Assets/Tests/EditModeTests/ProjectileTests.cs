namespace Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Transforms;
    using Unity.Mathematics;
    using Asteroids_TDD_ECS;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class ProjectileTests : ECSTestFixture
    {
        private EntityQuery m_weaponFiredQuery;
        private Entity m_weaponEntity;
        private Entity m_projectileEntity;

        [ SetUp ]
        protected override void SetUp()
        {
            base.SetUp();

            m_projectileEntity = _manager.CreateEntity(
                typeof( Projectile ),
                typeof( Rotation ),
                typeof( Translation )
            );

            m_weaponEntity = _manager.CreateEntity(
                typeof( Weapon ),
                typeof( WeaponFired ),
                typeof( Rotation ),
                typeof( Translation )
            );
            _manager.SetComponentData( m_weaponEntity, new Weapon{ FireRate = 1, ProjectilePrefab = m_projectileEntity } );
            _manager.SetComponentData( m_weaponEntity, new WeaponFired{ TimeFired = 0 } );
            _manager.SetComponentData( m_weaponEntity, new Rotation{ Value = quaternion.identity } );
            _manager.SetComponentData( m_weaponEntity, new Translation{ Value = float3.zero } );

            m_weaponFiredQuery = _manager.CreateEntityQuery(
                typeof( Weapon ),
                typeof( WeaponFired ),
                typeof( Rotation ),
                typeof( Translation )
            );
        }

        [ Test ]
        public void ProjectileSpawned_When_WeaponFired()
        {

            BeginInitializationEntityCommandBufferSystem projectileEntityCommandBuffer = _world.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            SpawnProjectileSystem spawnSystem = _world.CreateSystem<SpawnProjectileSystem>();
            JobHandle handle = spawnSystem.ProcessSpawnProjectileJob( m_weaponFiredQuery, projectileEntityCommandBuffer );

            projectileEntityCommandBuffer.Update();

            _manager.DestroyEntity( m_projectileEntity ); // Destroy prefab entity so that it doesn't count towards the final result

            EntityQuery projectileQuery = _manager.CreateEntityQuery( typeof( Projectile ) );

            int expectation = 1;
            int result = projectileQuery.CalculateLength();

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void SpawnedProjectile_SpawnedAtPositionOfWeapon()
        {
            _manager.SetComponentData( m_weaponEntity, new Translation{ Value = new float3( 1, 0, 0 ) } );

            BeginInitializationEntityCommandBufferSystem projectileEntityCommandBuffer = _world.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            SpawnProjectileSystem spawnSystem = _world.CreateSystem<SpawnProjectileSystem>();
            JobHandle handle = spawnSystem.ProcessSpawnProjectileJob( m_weaponFiredQuery, projectileEntityCommandBuffer );

            projectileEntityCommandBuffer.Update();

            _manager.DestroyEntity( m_projectileEntity ); // Destroy prefab entity so that it doesn't count towards the final result

            EntityQuery projectileQuery = _manager.CreateEntityQuery( typeof( Projectile ), typeof( Translation ) );

            float3 expectation = new float3( 1, 0, 0 );

            Entity spawnedProjectile = projectileQuery.GetSingletonEntity();
            float3 result = _manager.GetComponentData<Translation>( spawnedProjectile ).Value;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void SpawnedProjectile_FacingDirectionOfWeapon()
        {
            _manager.SetComponentData( m_weaponEntity, new Rotation{ Value = quaternion.EulerXYZ( new float3( 0, 0, 1 ) ) } );

            BeginInitializationEntityCommandBufferSystem projectileEntityCommandBuffer = _world.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            SpawnProjectileSystem spawnSystem = _world.CreateSystem<SpawnProjectileSystem>();
            JobHandle handle = spawnSystem.ProcessSpawnProjectileJob( m_weaponFiredQuery, projectileEntityCommandBuffer );

            projectileEntityCommandBuffer.Update();

            _manager.DestroyEntity( m_projectileEntity ); // Destroy prefab entity so that it doesn't count towards the final result

            EntityQuery projectileQuery = _manager.CreateEntityQuery( typeof( Projectile ), typeof( Rotation ) );

            quaternion expectation = quaternion.EulerXYZ( new float3( 0, 0, 1 ) );

            Entity spawnedProjectile = projectileQuery.GetSingletonEntity();
            quaternion result = _manager.GetComponentData<Rotation>( spawnedProjectile ).Value;

            Assert.AreEqual( expectation, result );
        }
    }
}