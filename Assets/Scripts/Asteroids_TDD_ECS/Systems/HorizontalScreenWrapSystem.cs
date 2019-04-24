namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class HorizontalScreenWrapSystem : JobComponentSystem
    {
        [ BurstCompile ]
        public struct HorizontalScreenWrapJob : IJobForEach<Translation, HorizontalScreenWrap>
        {
            public void Execute( ref Translation translation, [ ReadOnly ] ref HorizontalScreenWrap screenWrap )
            {
                if( translation.Value.x > screenWrap.Max )
                    translation.Value = new float3( screenWrap.Min, translation.Value.y, translation.Value.z );
                else if( translation.Value.x < screenWrap.Min )
                    translation.Value = new float3( screenWrap.Max, translation.Value.y, translation.Value.z );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            HorizontalScreenWrapJob horizontalJob = new HorizontalScreenWrapJob();
            return horizontalJob.Schedule( this, inputDependencies );
        }
    }
}