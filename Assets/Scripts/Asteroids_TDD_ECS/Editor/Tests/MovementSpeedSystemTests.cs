namespace Asteroids_TDD_ECS.Editor.Tests
{
    using NUnit.Framework;
    using Unity.Entities;
    using Unity.Transforms;
    using Unity.Physics;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class MovementSpeedSystemTests : ECSTestFixture
    {
        [ Test ]
        public void When_MovementSpeedEquals0_EntityVelocityDoesNotChange()
        {
            Entity entity = _manager.CreateEntity(
                typeof( MovementSpeed )
            );
        }
        
    }
}