namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class HorizontalScreenWrapSystem : JobComponentSystem
    {
        private struct HorizontalScreenWrapJob : IJobForEach<HorizontalScreenWrap, Translation>
        {
            public void Execute( [ ReadOnly ] ref HorizontalScreenWrap wrap, ref Translation translation )
            {
                if( translation.Value.x > wrap.Max )
                    translation.Value = new float3( wrap.Min, translation.Value.y, translation.Value.z );
                else if( translation.Value.x < wrap.Min )
                    translation.Value = new float3( wrap.Max, translation.Value.y, translation.Value.z );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return new HorizontalScreenWrapJob().Schedule( this, inputDependencies );
        }
    }
}