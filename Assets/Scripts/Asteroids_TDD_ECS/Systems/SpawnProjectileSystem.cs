namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Transforms;

    // [ UpdateInGroup( typeof( SimulationSystemGroup ) ) ]
    public class SpawnProjectileSystem : JobComponentSystem
    {
        private BeginInitializationEntityCommandBufferSystem m_projectileEntityCommandBuffer;

        private struct SpawnProjectileJob : IJobForEach<Weapon, WeaponFired, Rotation, Translation>
        {
            [ ReadOnly ] public EntityCommandBuffer CommandBuffer;

            public void Execute( [ ReadOnly ] ref Weapon weapon, [ ReadOnly ] ref WeaponFired fired, [ ReadOnly ] ref Rotation rotation, [ ReadOnly ] ref Translation translation )
            {
                Entity projectileEntity = CommandBuffer.Instantiate( weapon.ProjectilePrefab );
                CommandBuffer.SetComponent( projectileEntity, new Rotation{ Value = rotation.Value } );
                CommandBuffer.SetComponent( projectileEntity, new Translation{ Value = translation.Value } );
            }
        }

        protected override void OnCreate()
        {
            m_projectileEntityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return ProcessSpawnProjectileJob( m_projectileEntityCommandBuffer, inputDependencies );
        }

        public JobHandle ProcessSpawnProjectileJob( BeginInitializationEntityCommandBufferSystem buffer, JobHandle inputDependencies = default( JobHandle ) )
        {
            SpawnProjectileJob job = new SpawnProjectileJob
            {
                CommandBuffer = buffer.CreateCommandBuffer()
            };

            JobHandle handle = job.Schedule( this, inputDependencies );

            buffer.AddJobHandleForProducer( handle );

            return handle;
        }
    }
}