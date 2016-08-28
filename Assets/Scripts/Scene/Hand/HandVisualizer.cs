using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.VR.WSA.Input;

namespace HoloLensUnitySampler.Scene.HoloLens.Hand
{
    public class HandVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Material normalMat;

        [SerializeField]
        private Material pressedMat;

        Dictionary<uint, GameObject> handObjects = new Dictionary<uint, GameObject>();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // Use this for initialization
        void Start()
        {
            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            InteractionManager.SourceLost += InteractionManager_SourceLost;
            InteractionManager.SourcePressed += InteractionManager_SourcePressed;
            InteractionManager.SourceReleased += InteractionManager_SourceReleased;
            InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;

#if false  
            var handObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            handObj.transform.SetParent(this.transform);
            handObj.transform.localScale = Vector3.one * 0.03f;
            handObj.SetActive(true);
            handObj.GetComponent<Renderer>().material = normalMat;
#endif
        }

        void OnDestroy()
        {
            InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
            InteractionManager.SourceLost -= InteractionManager_SourceLost;
            InteractionManager.SourcePressed -= InteractionManager_SourcePressed;
            InteractionManager.SourceReleased -= InteractionManager_SourceReleased;
            InteractionManager.SourceUpdated -= InteractionManager_SourceUpdated;
        }

        void Update()
        {
            sb.Remove(0, sb.Length);
            sb.AppendLine("**** CurrentReading ****");
            foreach(var state in InteractionManager.GetCurrentReading())
            {
                Vector3 position, velocity;

                state.properties.location.TryGetPosition(out position);
                state.properties.location.TryGetVelocity(out velocity);

                sb.AppendFormat("[id]{0}, [kind]{1}, [position]{2}, [velocity]{3}", state.source.id, state.source.kind, position, velocity).AppendLine();
            }
            sb.AppendLine("**** DetectingHands ****");
            foreach (var pair in handObjects)
            {
                sb.AppendFormat("[id]{0}, [position]{1}", pair.Key, pair.Value.transform.position).AppendLine();
            }
            InfoWindow.Instance.InfoText = sb.ToString();
        }

        private void InteractionManager_SourceDetected(InteractionSourceState state)
        {
            if (state.source.kind != InteractionSourceKind.Hand) { return; }

            GameObject handObj = null;

            if(handObjects.ContainsKey(state.source.id))
            {
                handObj = handObjects[state.source.id];
            }
            else
            {
                handObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                handObj.transform.SetParent(this.transform);
                handObj.transform.localScale = Vector3.one * 0.03f;

                handObjects[state.source.id] = handObj;
            }

            handObj.SetActive(true);
            handObj.GetComponent<Renderer>().material = normalMat;

            SetHandPosition(handObj, state);
        }

        private void InteractionManager_SourceLost(InteractionSourceState state)
        {
            if (state.source.kind != InteractionSourceKind.Hand) { return; }

            if(handObjects.ContainsKey(state.source.id))
            {
                Destroy(handObjects[state.source.id]);
                handObjects.Remove(state.source.id);
            }
        }

        private void InteractionManager_SourcePressed(InteractionSourceState state)
        {
            if (state.source.kind != InteractionSourceKind.Hand) { return; }

            if (handObjects.ContainsKey(state.source.id))
            {
                handObjects[state.source.id].GetComponent<Renderer>().material = pressedMat;
            }
        }

        private void InteractionManager_SourceReleased(InteractionSourceState state)
        {
            if (state.source.kind != InteractionSourceKind.Hand) { return; }

            if (handObjects.ContainsKey(state.source.id))
            {
                handObjects[state.source.id].GetComponent<Renderer>().material = normalMat;
            }
        }

        private void InteractionManager_SourceUpdated(InteractionSourceState state)
        {
            if (state.source.kind != InteractionSourceKind.Hand) { return; }

            if (handObjects.ContainsKey(state.source.id))
            {
                SetHandPosition(handObjects[state.source.id], state);
            }
        }

        private void SetHandPosition(GameObject obj, InteractionSourceState state)
        {
            if (obj == null) { return; }

            Vector3 position;
            if (state.properties.location.TryGetPosition(out position))
            {
                obj.transform.position = position;
            }
        }
    }
}
