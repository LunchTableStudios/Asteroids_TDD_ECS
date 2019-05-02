namespace Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Transforms;
    using Unity.Physics;
    using Unity.Mathematics;
    using Asteroids_TDD_ECS;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class MovementTests : ECSTestFixture
    {
        [ Test ]
        [ TestCase( true, false, 1 ) ]
        [ TestCase( false, true, -1 ) ]
        [ TestCase( true, true, 0 ) ]
        public void RotationInputCorrect_When_ProperButtonPressed( bool rotateLeft, bool rotateRight, int expectation )
        {
            Entity entity = _manager.CreateEntity(
                typeof( RotationInput )
            );
            _manager.SetComponentData( entity, new RotationInput{ Value = 0 } );

            RotationInputSystem inputSystem = _world.CreateSystem<RotationInputSystem>();
            JobHandle handle = inputSystem.ProcessRotationInputJob( rotateLeft, rotateRight );

            handle.Complete();

            int result = _manager.GetComponentData<RotationInput>( entity ).Value;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void RotationChangesByDeltaOfSpeed_When_RotationInputNotEquals0()
        {
            int input = 1;
            float speed = 1;
            float mockDeltaTime = 0.01f;

            Entity entity = _manager.CreateEntity(
                typeof( Rotation ),
                typeof( RotationInput ),
                typeof( RotationSpeed )
            );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );
            _manager.SetComponentData( entity, new RotationInput{ Value = input } );
            _manager.SetComponentData( entity, new RotationSpeed{ Value = speed } );


            RotationSystem rotationSystem = _world.CreateSystem<RotationSystem>();
            JobHandle handle = rotationSystem.ProcessRotationJob( mockDeltaTime );

            handle.Complete();

            quaternion expectation = quaternion.EulerXYZ( 0, 0, input * speed * mockDeltaTime );
            quaternion result = _manager.GetComponentData<Rotation>( entity ).Value;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void VelocityChangesByDeltaOfSpeed_When_MovementInputNotEquals0()
        {
            float mockDeltaTime = 0.01f;

            Entity entity = _manager.CreateEntity(
                typeof( Movement ),
                typeof( MovementSpeed ),
                typeof( PhysicsVelocity ),
                typeof( Rotation )
            );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );
            _manager.SetComponentData( entity, new Movement{ Value = new float3( 0, 1, 0 ) } );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = 1 } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );

            MovementSystem movementSystem = _world.CreateSystem<MovementSystem>();
            JobHandle handle = movementSystem.ProcessMovementJob( mockDeltaTime );

            handle.Complete();

            float3 expectation = new float3( 0, 0.01f, 0 );
            float3 result = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void VelocityChangesTowardsRotation()
        {
            float mockDeltaTime = 0.01f;

            Entity entity = _manager.CreateEntity(
                typeof( Movement ),
                typeof( MovementSpeed ),
                typeof( PhysicsVelocity ),
                typeof( Rotation )
            );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );
            _manager.SetComponentData( entity, new Movement{ Value = new float3( 0, 1, 0 ) } );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = 1 } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.EulerXYZ( 0, 0, 1 ) } );

            MovementSystem movementSystem = _world.CreateSystem<MovementSystem>();
            JobHandle handle = movementSystem.ProcessMovementJob( mockDeltaTime );

            handle.Complete();

            float3 expectation = math.mul( math.normalize( quaternion.EulerXYZ( 0, 0, 1 ) ), new float3( 0, 0.01f, 0 ) );
            float3 result = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, result );
        }
    }
}