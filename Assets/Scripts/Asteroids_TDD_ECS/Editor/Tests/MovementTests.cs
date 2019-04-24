namespace Asteroids_TDD_ECS.Editor.Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Physics;
    using Unity.Transforms;
    using Unity.Mathematics;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class MovementTests : ECSTestFixture
    {
        private Entity entity;

        [ SetUp ]
        protected override void SetUp()
        {
            base.SetUp();

            entity = _manager.CreateEntity(
                typeof( MovementInput ),
                typeof( MovementSpeed ),
                typeof( DeltaTime ),
                typeof( PhysicsVelocity ),
                typeof( Rotation )
            );
            _manager.SetComponentData( entity, new MovementInput{ Value = 0 } );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = 1 } );
            _manager.SetComponentData( entity, new DeltaTime{ Value = 0.01f } );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );
        }

        [ Test ]
        public void VelocityDoesNotChange_When_MovementInput0()
        {
            float3 expectation = float3.zero;

            _world.CreateSystem<MovementSystem>().Update();

            float3 results = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, results );
        }

        [ Test ]
        [ TestCase( 1, 0.01f, 0.01f ) ]
        [ TestCase( -1, 0.01f, -0.01f ) ]
        [ TestCase( 1, 0.02f, 0.02f ) ]
        public void VelocityChangesByDelta_When_MovementInputNot0( float input, float time, float expectedY )
        {
            _manager.SetComponentData( entity, new MovementInput{ Value = input } );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = 1 } );
            _manager.SetComponentData( entity, new DeltaTime{ Value = time } );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );

            float3 expectation = new float3( 0, expectedY, 0 );

            _world.CreateSystem<MovementSystem>().Update();

            float3 results = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, results );
        }

        [ Test ]
        public void VelocityChangesTowardsForward_When_MovementInputNot0()
        {
            _manager.SetComponentData( entity, new MovementInput{ Value = 1 } );
            _manager.SetComponentData( entity, new MovementSpeed{ Value = 1 } );
            _manager.SetComponentData( entity, new DeltaTime{ Value = 0.01f } );
            _manager.SetComponentData( entity, new PhysicsVelocity{ Linear = float3.zero } );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.EulerXYZ( new float3( 0, 0, 1 ) ) } );

            float3 expectation = math.mul( math.normalize( quaternion.EulerXYZ( new float3( 0, 0, 1 ) ) ), new float3( 0, 0.01f, 0 ) );

            _world.CreateSystem<MovementSystem>().Update();

            float3 results = _manager.GetComponentData<PhysicsVelocity>( entity ).Linear;

            Assert.AreEqual( expectation, results );
        }
    }
}