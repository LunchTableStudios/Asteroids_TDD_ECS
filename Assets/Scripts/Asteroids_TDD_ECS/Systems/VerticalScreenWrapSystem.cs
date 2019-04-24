namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class VerticalScreenWrapSystem : JobComponentSystem
    {
        [ BurstCompile ]
        public struct VerticalScreenWrapJob : IJobForEach<Translation, VerticalScreenWrap>
        {
            public void Execute( ref Translation translation, [ ReadOnly ] ref VerticalScreenWrap screenWrap )
            {
                if( translation.Value.y > screenWrap.Max )
                    translation.Value = new float3( translation.Value.x, screenWrap.Min, translation.Value.z );
                else if( translation.Value.y < screenWrap.Min )
                    translation.Value = new float3( translation.Value.x, screenWrap.Max, translation.Value.z );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            VerticalScreenWrapJob verticalJob = new VerticalScreenWrapJob();
            return verticalJob.Schedule( this, inputDependencies );
        }
    }
}