namespace Asteroids_TDD_ECS.Editor.Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Mathematics;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class RotationTests : ECSTestFixture
    {
        private Entity m_entity;

        [ SetUp ]
        protected override void SetUp()
        {
            base.SetUp();

            m_entity = _manager.CreateEntity(
                typeof( Rotation ),
                typeof( RotationInput ),
                typeof( RotationSpeed ),
                typeof( DeltaTime )
            );
            _manager.SetComponentData( m_entity, new Rotation{ Value = quaternion.identity } );
            _manager.SetComponentData( m_entity, new RotationInput{ Value = 0 } );
            _manager.SetComponentData( m_entity, new RotationSpeed{ Value = 1 } );
            _manager.SetComponentData( m_entity, new DeltaTime{ Value = 0.01f } );
        }

        [ Test ]
        public void RotationDoesNotChange_When_RotationInput0()
        {
            quaternion expectation = quaternion.identity;

            _world.CreateSystem<RotationSystem>().Update();

            quaternion result = _manager.GetComponentData<Rotation>( m_entity ).Value;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void RotationChangesByDelta_When_RotationInputNot0()
        {
            _manager.SetComponentData( m_entity, new RotationInput{ Value = 1 } );

            quaternion expectation = quaternion.EulerXYZ( 0, 0, 0.01f );

            _world.CreateSystem<RotationSystem>().Update();

            quaternion result = _manager.GetComponentData<Rotation>( m_entity ).Value;

            Assert.AreEqual( expectation, result );
        }
    }
}