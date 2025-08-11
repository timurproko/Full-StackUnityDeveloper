using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class HoverSelectionRenderPassInjector : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private UnitHover _unit;
        [SerializeField] private UnitSelectionManager _selection;
        [SerializeField] private TeamsConfig _teams;

        private HoverSelectionRenderPass _renderPass;

        private void Awake()
        {
            _renderPass = new HoverSelectionRenderPass(_material, _unit, _selection, _teams)
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