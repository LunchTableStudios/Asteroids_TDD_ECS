namespace Asteroids_TDD_ECS.Editor.Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Mathematics;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class RotationSpeedSystemTests : ECSTestFixture
    {
        Entity entity;

        [ SetUp ]
        protected override void SetUp()
        {
            base.SetUp();

            entity = _manager.CreateEntity(
                typeof( Rotation ),
                typeof( RotationSpeed ),
                typeof( DeltaTime )
            );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );
        }

        [ Test ]
        public void When_RotationSpeedEquals0_RotationDoesNotChange()
        {
            _manager.SetComponentData( entity, new RotationSpeed{ Value = 0 } );
            _manager.SetComponentData( entity, new DeltaTime{ Value = 0.1f } );

            quaternion expectation = quaternion.identity;

            _world.CreateSystem<RotationSpeedSystem>().Update();

            quaternion result = _manager.GetComponentData<Rotation>( entity ).Value;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        [ TestCase( 1, 0.1f, 0.1f ) ]
        [ TestCase( -1, 0.1f, -0.1f ) ]
        [ TestCase( 1, 0.2f, 0.2f ) ]
        public void When_RotationSpeedNotEquals0_RotationChangesByDeltaOfSpeed( float speed, float deltaTime, float expectedZ )
        {
            _manager.SetComponentData( entity, new RotationSpeed{ Value = speed } );
            _manager.SetComponentData( entity, new DeltaTime{ Value = deltaTime } );

            quaternion expectation = quaternion.EulerXYZ( new float3( 0, 0, expectedZ ) );

            _world.CreateSystem<RotationSpeedSystem>().Update();

            quaternion result = _manager.GetComponentData<Rotation>( entity ).Value;

            Assert.AreEqual( expectation, result );
        }
    }
}