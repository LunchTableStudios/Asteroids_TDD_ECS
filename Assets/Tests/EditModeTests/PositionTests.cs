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
    public class PositionTests : ECSTestFixture
    {
        [ Test ]
        [ TestCase( 1001, -1000 ) ]
        [ TestCase( -1001, 1000 ) ]
        public void ObjectWrapsToOppositeX_When_ExceedingXThreshold( float position, float expectation )
        {
            float mockMin = -1000;
            float mockMax = 1000;

            Entity entity = _manager.CreateEntity(
                typeof( HorizontalScreenWrap ),
                typeof( Translation )
            );
            _manager.SetComponentData( entity, new HorizontalScreenWrap{ Min = mockMin, Max = mockMax } );
            _manager.SetComponentData( entity, new Translation{ Value = new float3( position, 0, 0 ) } );

            _world.CreateSystem<HorizontalScreenWrapSystem>().Update();
            
            float result = _manager.GetComponentData<Translation>( entity ).Value.x;
            
            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        [ TestCase( 1001, -1000 ) ]
        [ TestCase( -1001, 1000 ) ]
        public void ObjectWrapsToOppositeY_When_ExceedingYThreshold( float position, float expectation )
        {
            float mockMin = -1000;
            float mockMax = 1000;

            Entity entity = _manager.CreateEntity(
                typeof( VerticalScreenWrap ),
                typeof( Translation )
            );
            _manager.SetComponentData( entity, new VerticalScreenWrap{ Min = mockMin, Max = mockMax } );
            _manager.SetComponentData( entity, new Translation{ Value = new float3( 0, position, 0 ) } );

            _world.CreateSystem<VerticalScreenWrapSystem>().Update();
            
            float result = _manager.GetComponentData<Translation>( entity ).Value.y;
            
            Assert.AreEqual( expectation, result );
        }
    }

}