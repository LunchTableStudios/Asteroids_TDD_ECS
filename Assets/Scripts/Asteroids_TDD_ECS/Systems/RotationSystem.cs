namespace Asteroids_TDD_ECS
{
    using Unity.Entities;
    using Unity.Collections;
    using Unity.Jobs;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class RotationSystem : JobComponentSystem
    {
        private struct RotationJob : IJobForEach<Rotation, RotationInput, RotationSpeed>
        {
            public float DeltaTime;

            public void Execute( ref Rotation rotation, [ ReadOnly ] ref RotationInput input, [ ReadOnly ] ref RotationSpeed speed )
            {
                rotation.Value = math.mul( math.normalize( rotation.Value ), quaternion.EulerXYZ( 0, 0, input.Value * speed.Value * DeltaTime ) );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDependencies )
        {
            return inputDependencies;
        }

        
    }
}