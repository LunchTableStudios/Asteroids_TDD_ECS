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
        public void ObjectWrapsToOppositeX_When_ExceedingXThreshold()
        {
            Entity entity = _manager.CreateEntity(
                typeof( HorizontalScreenWrap ),
                typeof( Translation )
            );
            _manager.SetComponentData( entity, new HorizontalScreenWrap{ Min = -1000, Max = 1000 } );
            _manager.SetComponentData( entity, new Translation{ Value = new float3( 1001, 0, 0 ) } );

            float expectation = -1000;

            _world.CreateSystem<HorizontalScreenWrapSystem>().Update();
            
            float result = _manager.GetComponentData<Translation>( entity ).Value.x;
            
            Assert.AreEqual( expectation, result );
        }

        [ Test ]
        public void ObjectWrapsToOppositeY_When_ExceedingYThreshold()
        {
            Entity entity = _manager.CreateEntity(
                typeof( VerticalScreenWrap ),
                typeof( Translation )
            );
            _manager.SetComponentData( entity, new VerticalScreenWrap{ Min = -1000, Max = 1000 } );
            _manager.SetComponentData( entity, new Translation{ Value = new float3( 0, 1001, 0 ) } );

            float expectation = -1000;

            _world.CreateSystem<VerticalScreenWrapSystem>().Update();
            
            float result = _manager.GetComponentData<Translation>( entity ).Value.y;
            
            Assert.AreEqual( expectation, result );
        }
    }

}