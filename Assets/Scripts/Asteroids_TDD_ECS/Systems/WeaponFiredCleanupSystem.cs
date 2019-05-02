namespace Asteroids_TDD_ECS
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;

    [ UpdateInGroup( typeof( SimulationSystemGroup ) ) ]
    public class WeaponFiredCleanupSystem : JobComponentSystem
    {
        private EntityQuery m_weaponCleanupQuery;
        private BeginInitializationEntityCommandBufferSystem m_weaponCleanupEntityCommandBuffer;

        private struct WeaponFiredCleanup : IJobForEachWithEntity<Weapon, WeaponFired>
        {
            [ ReadOnly ] public EntityCommandBuffer CommandBuffer;
            public float Time;

            public void Execute( Entity entity, int jobIndex, [ ReadOnly ] ref Weapon weapon, ref WeaponFired fired )
            {
                if( Time > weapon.FireRate + fired.TimeFired )
                {
                    CommandBuffer.RemoveComponent<WeaponFired>( entity );
                }
            }
        }

        protected override void OnCreate()
        {
            m_weaponCleanupQuery = GetEntityQuery(
                typeof( Weapon ),
                typeof( WeaponFired )
            );

            m_weaponCleanupEntityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return ProcessCleanupJob( m_weaponCleanupQuery, m_weaponCleanupEntityCommandBuffer, Time.time, inputDependencies );
        }

        public JobHandle ProcessCleanupJob( EntityQuery query, BeginInitializationEntityCommandBufferSystem buffer, float time, JobHandle inputDependencies = default( JobHandle ) )
        {
            WeaponFiredCleanup job = new WeaponFiredCleanup
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