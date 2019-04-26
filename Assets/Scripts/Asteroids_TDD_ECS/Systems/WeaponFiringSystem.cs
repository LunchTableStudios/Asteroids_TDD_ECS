namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;

    [ UpdateInGroup( typeof( SimulationSystemGroup ) ) ]
    public class WeaponFiringSystem : JobComponentSystem
    {
        private EntityQuery m_weaponQuery;
        private BeginInitializationEntityCommandBufferSystem m_weaponEntityCommandBuffer;

        public struct WeaponFiringJob : IJobForEachWithEntity<Weapon, ShootInput>
        {
            [ ReadOnly ] public EntityCommandBuffer CommandBuffer;
            public float Time;

            public void Execute( Entity entity, int jobIndex, [ ReadOnly ] ref Weapon weapon, [ ReadOnly ] ref ShootInput input )
            {
                if( input.IsShooting )
                {
                    CommandBuffer.AddComponent( entity, new WeaponFired{ TimeFired = Time } );
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

            m_weaponQuery = GetEntityQuery( weaponQueryDesc );

            m_weaponEntityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return ProcessWeaponFiringJob( m_weaponQuery, m_weaponEntityCommandBuffer, Time.time, inputDependencies );
        }

        public JobHandle ProcessWeaponFiringJob( EntityQuery query, BeginInitializationEntityCommandBufferSystem buffer, float time, JobHandle inputDependencies = default( JobHandle ) )
        {
            WeaponFiringJob job = new WeaponFiringJob
            {
                CommandBuffer = buffer.CreateCommandBuffer(),
                Time = time
            };

            JobHandle handle = job.Schedule( query, inputDependencies );

            buffer.AddJobHandleForProducer( handle );

            return handle;
        }
    }
}