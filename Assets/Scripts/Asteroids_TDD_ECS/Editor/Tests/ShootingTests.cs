namespace Asteroids_TDD_ECS.Editor.Tests
{
    using NUnit.Framework;
    using Unity.Entities;

    [ TestFixture ]
    [ Category( "Asteroids Tests" ) ]
    public class ShootingTests : ECSTestFixture
    {
        [ Test ]
        public void BulletSpawns_When_ShootInputTrue()
        {
            Entity entity = _manager.CreateEntity(
                typeof( ShootInput )
            );
            _manager.SetComponentData( entity, new ShootInput{ IsShooting = true } );

            int expected = 1;

            EntityQuery bulletQuery = _manager.CreateEntityQuery(
                typeof( Bullet )
            );

            _world.CreateSystem<ShootSystem>().Update();

            int result = bulletQuery.CalculateLength();

            Assert.AreEqual( expected, result );
        }
    }
}