namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class VerticalScreenWrapSystem : JobComponentSystem
    {
        private struct VerticalScreenWrapJob : IJobForEach<VerticalScreenWrap, Translation>
        {
            public void Execute( [ ReadOnly ] ref VerticalScreenWrap wrap, ref Translation translation )
            {
                if( translation.Value.y > wrap.Max )
                    translation.Value = new float3( translation.Value.x, wrap.Min, translation.Value.z );
                else if( translation.Value.y < wrap.Min )
                    translation.Value = new float3( translation.Value.x, wrap.Max, translation.Value.z );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return new VerticalScreenWrapJob().Schedule( this, inputDependencies );
        }
    }
}