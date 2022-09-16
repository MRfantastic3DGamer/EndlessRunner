using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

namespace State_machine
{
    [Serializable]
    public class Condition
    {
        private const String StrRaycast = "Raycast/";
        private bool _drawRays;
        private List<String> st = new List<string>();

        public List<String> ConditionTypesString
        {
            get
            {
                foreach (RaycastConditionUnit c in raycastConditionUnits)
                    st.Add(StrRaycast + c.conditionName);

                return st;
            }
        }

        public Dictionary<String, RaycastConditionUnit> raycastConditions = new Dictionary<string, RaycastConditionUnit>();
        [OnValueChanged("OnRaycastConditionUnitsChange")]
        [ListDrawerSettings(OnTitleBarGUI = "DrawToggleRayLogButton")]
        public List<RaycastConditionUnit> raycastConditionUnits;
        private void OnRaycastConditionUnitsChange()
        {
            raycastConditions.Clear();
            foreach (RaycastConditionUnit r in raycastConditionUnits)
            { 
                raycastConditions.Add(StrRaycast + r.conditionName, r);
                
            }
        }
        private void DrawToggleRayLogButton()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Pen)) _drawRays = !_drawRays;
        }
        private void DebugRays()
        {
            foreach (RaycastConditionUnit r in raycastConditionUnits)
                r.DebugRay();
        }

        
        
        public void Debug()
        {
            if(_drawRays) DebugRays();
            
        }
    }
}