using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class UnitSelectionRenderPassInjector : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private UnitSelectionManager _selection;
        [SerializeField] private TeamsConfig _teams;
        
        private UnitSelectionRenderPass _renderPass;

        private void Awake()
        {
            _renderPass = new UnitSelectionRenderPass(_material, _selection, _teams)
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
            };
        }

        private void InjectRenderPass(ScriptableRenderContext context, Camera renderCamera)
        {
            renderCamera.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(_renderPass);
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += InjectRenderPass;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= InjectRenderPass;
        }
    }
}