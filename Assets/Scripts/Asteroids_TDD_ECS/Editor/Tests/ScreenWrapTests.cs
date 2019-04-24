namespace Asteroids_TDD_ECS.Editor.Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Mathematics;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class ScreenWrapTests : ECSTestFixture
    {
        [ Test ]
        [ TestCase( 101, -100, 100, -100 ) ]
        [ TestCase( -101, -100, 100, 100 ) ]
        public void MovesHorizontalPositionMovesToOppositeSide_When_HorizontalPositionOutOfBounds( float position, float min, float max, float expectedX )
        {
            float maxPosition = max;
            float minPosition = min;

            Entity entity = _manager.CreateEntity(
                typeof( Translation ),
                typeof( HorizontalScreenWrap )
            );
            _manager.SetComponentData( entity, new Translation{ Value = new float3( position, 0, 0 ) } );
            _manager.SetComponentData( entity, new HorizontalScreenWrap{ Min = minPosition, Max = maxPosition } );

            float3 expectation = new float3( expectedX, 0, 0 );

            _world.CreateSystem<HorizontalScreenWrapSystem>().Update();

            float3 result = _manager.GetComponentData<Translation>( entity ).Value;

            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        [ TestCase( 101, -100, 100, -100 ) ]
        [ TestCase( -101, -100, 100, 100 ) ]
        public void MovesVerticalPositionMovesToOppositeSide_When_VerticalPositionOutOfBounds( float position, float min, float max, float expectedY )
        {
            float maxPosition = max;
            float minPosition = min;

            Entity entity = _manager.CreateEntity(
                typeof( Translation ),
                typeof( VerticalScreenWrap )
            );
            _manager.SetComponentData( entity, new Translation{ Value = new float3( 0, position, 0 ) } );
            _manager.SetComponentData( entity, new VerticalScreenWrap{ Min = minPosition, Max = maxPosition } );

            float3 expectation = new float3( 0, expectedY, 0 );

            _world.CreateSystem<VerticalScreenWrapSystem>().Update();

            float3 result = _manager.GetComponentData<Translation>( entity ).Value;

            Assert.AreEqual( expectation, result );
        }
    }
}