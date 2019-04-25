namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Jobs;

    [ UpdateInGroup( typeof( SimulationSystemGroup ) ) ]
    public class WeaponFiringSystem : JobComponentSystem
    {
        private EntityQuery m_weaponEntityQuery;
        private BeginInitializationEntityCommandBufferSystem m_weaponBufferSystem;

        private struct WeaponFiringJob : IJobForEachWithEntity<Weapon, ShootInput>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            public float Time;

            public void Execute( Entity entity, int index, ref Weapon weapon, ref ShootInput input )
            {
                if( input.IsShooting )
                {
                    WeaponFired fired = new WeaponFired{ TimeOfFire = Time };
                    CommandBuffer.AddComponent( index, entity, fired );
                }
            }
        }

        protected override void OnCreate()
        {
            EntityQueryDesc weaponQueryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]{ typeof( Weapon ), typeof( ShootInput ) },
                None = new ComponentType[]{ typeof( WeaponFired ) }
            };

            m_weaponEntityQuery = GetEntityQuery( weaponQueryDesc );

            m_weaponBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            JobHandle job = new WeaponFiringJob
            {
                CommandBuffer = m_weaponBufferSystem.CreateCommandBuffer().ToConcurrent(),
                Time = UnityEngine.Time.time
            }.ScheduleSingle( m_weaponEntityQuery, inputDependencies );

            m_weaponBufferSystem.AddJobHandleForProducer( job );

            return job;
        }
    }
}