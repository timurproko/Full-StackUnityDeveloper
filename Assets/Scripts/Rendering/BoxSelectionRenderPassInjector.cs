using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SampleGame
{
    public class BoxSelectionRenderPassInjector : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private RectInput _rectInput;
        
        private BoxSelectionRenderPass _renderPass;

        private void Awake()
        {
            _renderPass = new BoxSelectionRenderPass(_material, _rectInput)
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