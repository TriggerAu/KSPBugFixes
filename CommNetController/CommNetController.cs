﻿using System;

using UnityEngine;
using CommNet;

namespace CommNetControllerPlugin
{
    /// <summary>
    /// Plugin that will control CommNet Control Level for the active vessel
    /// 
    /// Most of this is simply GUI that shows whats going on
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class CommNetControllerPlugin : MonoBehaviour
    {
        /// <summary>
        /// This is what does teh actual control
        /// </summary>
        MyCommNetControlSource controlSource;


        public void Start()
        {
            Debug.Log("CommNet Controller Plugin Start");
        }

        #region Window Code
        public Rect windowRect = new Rect(20, 20, 300, 400);
        public void OnGUI()
        {
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "Situation");

        }
        void DoMyWindow(int windowID)
        {
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Register")) {
                controlSource = new MyCommNetControlSource();
                controlSource.v = FlightGlobals.ActiveVessel;

                controlSource.v.connection.RegisterCommandSource(controlSource);

            }
            if(GUILayout.Button("UnRegister")) {
                if(controlSource != null) {
                    controlSource.v.connection.UnregisterCommandSource(controlSource);
                    controlSource = null;
                }
            }
            GUILayout.EndHorizontal();

            if(controlSource == null) {
                GUILayout.Label("ControlMe State: Not registered");
            } else {
                GUILayout.Label("ControlMe State: " + controlSource.GetControlSourceState().ToString());

                GUILayout.BeginHorizontal();
                if(GUILayout.Button("Full")) {
                    controlSource.state = VesselControlState.Full;
                }
                if(GUILayout.Button("Part")) {
                    controlSource.state = VesselControlState.Partial;
                }
                if(GUILayout.Button("None")) {
                    controlSource.state = VesselControlState.None;
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.Label("----Vessel Stats----");
            if(FlightGlobals.ActiveVessel == null) {
                GUILayout.Label("Cant Read Active Vessel");
            } else {
                Vessel v = FlightGlobals.ActiveVessel;

                GUILayout.Label("CurrentControlLevel: " + v.CurrentControlLevel);
                GUILayout.Label("IsControllable: " + v.IsControllable);
                GUILayout.Label("MaxControlLevel: " + v.maxControlLevel);

                GUILayout.Label("----Control Sources on Vessel ----");
                for(int i = 0; i < v.connection.CommandSources.Count; i++) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(v.connection.CommandSources[i].name + ":" + v.connection.CommandSources[i].GetControlSourceState());
                    GUILayout.EndHorizontal();
                }
            }


            GUI.DragWindow();
        }

        #endregion
    }



    /// <summary>
    /// This class implements the ControlSource interface so it can be part of the vessel control
    /// </summary>
    public class MyCommNetControlSource : CommNet.ICommNetControlSource
    {
        //Some public vars to control the state
        public Vessel v;
        public VesselControlState state = VesselControlState.Full;

        #region Interface Methods
        public string name
        {
            get
            {
                //What a name - so imaginitive
                return "TriggersCommNetController";
            }
        }

        public VesselControlState GetControlSourceState()
        {
            //simply return teh set state to commnet
            return state;
        }

        public bool IsCommCapable()
        {
            //This source is always capable
            return true;
        }

        public void UpdateNetwork()
        {
            //This part limits the max state of the vessel as well. so the controller sets the control state regardless of other parts
            switch(state) {
                case VesselControlState.Full:
                    v.maxControlLevel = Vessel.ControlLevel.FULL;
                    break;
                case VesselControlState.ProbePartial:
                    v.maxControlLevel = Vessel.ControlLevel.PARTIAL_UNMANNED;
                    break;
                case VesselControlState.Partial:
                    v.maxControlLevel = Vessel.ControlLevel.PARTIAL_MANNED;
                    break;
                case VesselControlState.None:
                    v.maxControlLevel = Vessel.ControlLevel.NONE;
                    break;
                default:
                    v.maxControlLevel = Vessel.ControlLevel.FULL;
                    break;
            }
        }
        #endregion
    }
}
