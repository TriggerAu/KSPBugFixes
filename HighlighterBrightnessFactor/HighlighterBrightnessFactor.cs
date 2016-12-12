using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using KSP;

namespace HighlighterBrightnessFactor
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor,false)]
    public class HighlighterBrightnessFactor : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("[HighlighterBrightnessFactor]: Setting highlighterlimit from gamesettings");

            Highlighting.Highlighter.HighlighterLimit = GameSettings.PART_HIGHLIGHTER_BRIGHTNESSFACTOR;

            Debug.Log("[HighlighterBrightnessFactor]: Done");
        }
    }
}
