namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Burst;

    public class ShootSystem : JobComponentSystem
    {
        private BeginInitializationEntityCommandBufferSystem m_entityCommandBuffer;

        [ BurstCompile ]
        private struct ShootJob : IJobForEach<ShootInput>
        {
            public EntityCommandBuffer CommandBuffer;

            public void Execute( [ ReadOnly ] ref ShootInput input )
            {

            }
        }
    
        protected override void OnCreate()
        {
            m_entityCommandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies ) 
        {
            JobHandle job = new ShootJob
            {
                CommandBuffer = m_entityCommandBuffer.CreateCommandBuffer()
            }.ScheduleSingle( this, inputDependencies );

            m_entityCommandBuffer.AddJobHandleForProducer( job );

            return job;
        }
    }
}