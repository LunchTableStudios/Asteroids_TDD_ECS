namespace Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Mathematics;
    using Asteroids_TDD_ECS;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class MovementTests : ECSTestFixture
    {
        [ Test ]
        public void RotationChangesByDeltaOfSpeed_When_RotationInputNotEquals0()
        {
            float input = 1;
            float speed = 1;
            float mockDeltaTime = 0.1f;

            Entity entity = _manager.CreateEntity(
                typeof( Rotation ),
                typeof( RotationInput ),
                typeof( RotationSpeed )
            );
            _manager.SetComponentData( entity, new Rotation{ Value = quaternion.identity } );
            _manager.SetComponentData( entity, new RotationInput{ Value = input } );
            _manager.SetComponentData( entity, new RotationSpeed{ Value = speed } );

            quaternion expectation = quaternion.EulerXYZ( 0, 0, input * speed * mockDeltaTime );

            _world.CreateSystem<RotationSystem>().Update();

            quaternion result = _manager.GetComponentData<Rotation>( entity ).Value;

            Assert.AreEqual( expectation, result );
        }
    }
}