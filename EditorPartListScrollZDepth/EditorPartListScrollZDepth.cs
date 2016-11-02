using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using KSP;

namespace EditorPartListScrollZDepth
{
    [KSPAddon(KSPAddon.Startup.EditorAny,false)]
    public class EditorPartListScrollZDepth : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("[EditorPartListScrollZDepth]: Searching for EditorPartList");
            if(KSP.UI.Screens.EditorPartList.Instance != null) {
                Debug.Log("[EditorPartListScrollZDepth]: Found It. Now get the scrollbar handle");
                Transform editorPartListScrollHandle = KSP.UI.Screens.EditorPartList.Instance.gameObject.transform.Find("Mode Transition/PartList Area/PartList and sorting/ListAndScrollbar/Scrollbar/Sliding Area/Handle");

                if(editorPartListScrollHandle != null) {
                    Debug.Log("[EditorPartListScrollZDepth]: Position was: " + editorPartListScrollHandle.localPosition);
                    editorPartListScrollHandle.localPosition = new Vector3(editorPartListScrollHandle.localPosition.x, editorPartListScrollHandle.localPosition.y, -1f);
                    Debug.Log("[EditorPartListScrollZDepth]: Position now: " + editorPartListScrollHandle.localPosition);
                } else {
                    Debug.Log("[EditorPartListScrollZDepth]: Couldnt get handle.");
                }
            } else {
                Debug.Log("[EditorPartListScrollZDepth]: Couldnt find it.");
            }
            Debug.Log("[EditorPartListScrollZDepth]: Done");

        }
    }
}
