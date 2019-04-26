namespace Tests
{
    using System.Linq;
    using NUnit.Framework;
    using Unity.Entities;

    public class ECSTestFixture
    {
        protected World _world;
        protected World _previousWorld;
        protected EntityManager _manager;
        protected EntityManager.EntityManagerDebug _managerDebug;

        [ SetUp ]
        protected virtual void SetUp()
        {
            _previousWorld = World.Active;

            _world = World.Active = new World( "ECS Test World" );

            _manager = _world.EntityManager;

            _managerDebug = new EntityManager.EntityManagerDebug( _manager );

            UnityEngine.Assertions.Assert.raiseExceptions = true;
        }

        [ TearDown ]
        protected virtual void TearDown()
        {
            if( _manager != null )
            {
                while( _world.Systems.Any() )
                {
                    _world.DestroySystem( _world.Systems.First() );
                }

                _managerDebug.CheckInternalConsistency();

                _world.Dispose();
                _world = null;

                World.Active = _previousWorld;
                _previousWorld = null;
                _manager = null;
            }
        }
    }
}