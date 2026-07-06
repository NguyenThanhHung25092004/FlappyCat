using UnityEngine;
using Unity.Cinemachine;

public class CinemachineLockedY : CinemachineExtension
{
    public float lockedYPosition = 0f;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.RawPosition;
            pos.y = lockedYPosition;
            state.RawPosition = pos;
        }
    }
}
