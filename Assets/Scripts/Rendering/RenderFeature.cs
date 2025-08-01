// using UnityEngine;
// using UnityEngine.Rendering.Universal;
//
// namespace SampleGame
// {
//     public class RenderFeature : ScriptableRendererFeature
//     {
//         [SerializeField] private Material _material;
//         [SerializeField] private RectInput _rectInput;
//
//         private RenderPass _renderPass;
//
//         public override void Create()
//         {
//             _renderPass = new RenderPass(_material, _rectInput)
//             {
//                 renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
//             };
//         }
//
//         public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
//         {
//             renderer.EnqueuePass(_renderPass);
//         }
//     }
// }