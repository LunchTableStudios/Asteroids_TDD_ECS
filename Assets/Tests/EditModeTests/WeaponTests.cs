namespace Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Asteroids_TDD_ECS;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class WeaponTests : ECSTestFixture
    {
        EntityQueryDesc m_weaponQueryDesc;
        EntityQuery m_weaponQuery;
        EntityQuery m_weaponWasFiredQuery;

        BeginInitializationEntityCommandBufferSystem m_commandBuffer;

        [ SetUp ]
        protected override void SetUp()
        {
            base.SetUp();

            m_commandBuffer = _world.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            m_weaponQueryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]{ typeof( Weapon ), typeof( ShootInput ) },
                None = new ComponentType[]{ typeof( WeaponFired ) }
            };

            m_weaponQuery = _manager.CreateEntityQuery( m_weaponQueryDesc );

            m_weaponWasFiredQuery = _manager.CreateEntityQuery(
                typeof( Weapon ),
                typeof( ShootInput ),
                typeof( WeaponFired )
            );
        }

        [ Test ]
        public void WeaponFiredComponentAddedToEntity_When_ShootInputTrue()
        {
            Entity weaponEntity = _manager.CreateEntity(
                typeof( Weapon ),
                typeof( ShootInput )
            );
            _manager.SetComponentData( weaponEntity, new Weapon{ FireRate = 1 } );
            _manager.SetComponentData( weaponEntity, new ShootInput{ IsShooting = true } );

            WeaponFiringSystem firingSystem = _world.CreateSystem<WeaponFiringSystem>();
            JobHandle handle = firingSystem.ProcessWeaponFiringJob( m_weaponQuery, m_commandBuffer, 0 );

            int expectation = 1;

            m_commandBuffer.Update();
            
            int result = m_weaponWasFiredQuery.CalculateLength();

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void FiredAtSetToCurrentTime_When_WeaponFiredComponentAdded()
        {
            Entity weaponEntity = _manager.CreateEntity(
                typeof( Weapon ),
                typeof( ShootInput )
            );
            _manager.SetComponentData( weaponEntity, new Weapon{ FireRate = 1 } );
            _manager.SetComponentData( weaponEntity, new ShootInput{ IsShooting = true } );

            WeaponFiringSystem firingSystem = _world.CreateSystem<WeaponFiringSystem>();
            JobHandle handle = firingSystem.ProcessWeaponFiringJob( m_weaponQuery, m_commandBuffer, 2 );

            float expectation = 2;

            m_commandBuffer.Update();

            float result = _manager.GetComponentData<WeaponFired>( weaponEntity ).TimeFired;

            Assert.AreEqual( expectation, result );
        }
    }
}