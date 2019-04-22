namespace Asteroids_TDD_ECS.Editor.Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Physics;
    using Unity.Mathematics;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class MovementSpeedSystemTests : ECSTestFixture
    {
        [ Test ]
        public void When_MovementSpeedEquals0_EntityVelocityDoesNotChange()
        {
            Entity entity = _manager.CreateEntity(
                typeof( MovementSpeed ),
                typeof( Rotation ),
                typeof( PhysicsVelocity )
            );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = 0 } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );

            float3 expectation = float3.zero;

            _world.CreateSystem<MovementSpeedSystem>().Update();

            float3 result = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        [ TestCase( 1, 0.1f, 0.1f ) ]
        [ TestCase( -1, 0.1f, -0.1f ) ]
        [ TestCase( 1, 0.2f, 0.2f ) ]
        public void When_MovementSpeedNotEquals0_EntityVelocityChangesByDeltaOfSpeed( float speed, float time, float expectedY )
        {
            Entity entity = _manager.CreateEntity(
                typeof( MovementSpeed ),
                typeof( Rotation ),
                typeof( PhysicsVelocity ),
                typeof( DeltaTime )
            );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = speed } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );
            _manager.SetComponentData( entity, new DeltaTime{ Value = time } );

            float3 expectation = new float3( 0, expectedY, 0 );

            _world.CreateSystem<MovementSpeedSystem>().Update();

            float3 result = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        [ TestCase( 1 ) ]
        [ TestCase( 2 ) ]
        [ TestCase( -1 ) ]
        [ TestCase( 1.5f ) ]
        public void When_EntityRotationEquals0_EntityVelocityChangesTowardsForward( float zRotation )
        {
            Entity entity = _manager.CreateEntity(
                typeof( MovementSpeed ),
                typeof( Rotation ),
                typeof( PhysicsVelocity ),
                typeof( DeltaTime )
            );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = 1 } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.EulerXYZ( 0, 0, zRotation ) } );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );
            _manager.SetComponentData( entity, new DeltaTime{ Value = 0.1f } );

            Rotation rotation = _manager.GetComponentData<Rotation>( entity );

            float3 expectation = math.mul( math.normalize( rotation.Value ), new float3( 0, 0.1f, 0 ) );

            _world.CreateSystem<MovementSpeedSystem>().Update();

            float3 result = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, result );
        }
    }
}