﻿using UnityEngine;
using UnityEditor;

namespace CreAtom
{
    [CustomPropertyDrawer (typeof(PartNode))]
    public class PartNodeDrawer:PropertyDrawer
    {
        const float row = 16;
        const float row2 = 19;
        const float height = 95;
        const float widthRA = 0.35f;
        const float indent = 20;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            Rect _rect = EditorGUI.IndentedRect (position);
            _rect.width = 245;
            EditorGUIUtility.wideMode = true;
            var defIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var p_Tpos = property.FindPropertyRelative ("Tpos");
            var p_Trot = property.FindPropertyRelative ("Trot");
            var p_part = property.FindPropertyRelative ("part");
            var p_parentId = property.FindPropertyRelative ("parentId");
            var p_childHides = property.FindPropertyRelative ("childHides");
            var p_childIds = property.FindPropertyRelative ("childIds");
            var p_part_Instance = property.FindPropertyRelative ("part_Instance");

            EditorGUI.BeginProperty (_rect, label, property);

            //Title
            _rect.y -= 2;
            GUI.Box (_rect, "", "helpbox");
            _rect.y += 2;

            _rect.height = row;
            _rect.x += indent + row - 4;
            _rect.width -= indent + row;
            EditorGUI.PropertyField (_rect, p_part, new GUIContent (""));

            Rect _rectI = new Rect (
                              _rect.x - row + 2,
                              _rect.y + 2,
                              row - 3,
                              row - 3
                          );
            GameObject pI_obj = p_part_Instance.objectReferenceValue as GameObject;
            bool pI_Show = pI_obj != null && pI_obj.activeInHierarchy;
            GUIContent pI_content = new GUIContent ("", pI_Show ? "Show" : "Hide");
            GUI.Box (_rectI, pI_content, pI_Show ? "WinBtnMinMac" : "WinBtnInactiveMac");

            _rect.x -= indent + row - 6;
            _rect.width += indent + row - 4;

            //Transform
            Rect _rectColT = new Rect (
                                 _rect.x + _rect.width * widthRA,
                                 _rect.y + row2,
                                 _rect.width - 3 - _rect.width * widthRA,
                                 row + row2
                             );
            EditorGUI.DrawRect (_rectColT, new Color (0.1f, 0.5f, 0.5f, 0.5f));
            EditorGUIUtility.labelWidth = 25f;
            _rectColT.height = row;
            EditorGUI.PropertyField (_rectColT, p_Tpos, new GUIContent ("Pos"), true);
            _rectColT.y += row2;
            EditorGUI.PropertyField (_rectColT, p_Trot, new GUIContent ("Rot"), true);

            //IDs
            Rect _rectColI = new Rect (
                                 _rect.x,
                                 _rect.y + row2,
                                 _rect.width * widthRA - 5,
                                 row
                             );
            EditorGUIUtility.labelWidth = 58f;
            EditorGUI.LabelField (_rectColI,"ParentId : " + p_parentId.intValue);
            _rectColI.y += row2;
            EditorGUI.LabelField (_rectColI,"ChildsCnt : " + p_childIds.arraySize);

            //Child Ids
            int childCount = p_childIds.arraySize;
            float childSpace = (_rect.width - EditorGUIUtility.labelWidth) / childCount;
            Rect _rectColC = new Rect (
                                 _rect.x + 10,
                                 _rectColI.y + row2,
                                 EditorGUIUtility.labelWidth,
                                 _rectColI.height
                             );
            EditorGUI.LabelField (_rectColC, "ID");
            _rectColC.y += row;
            EditorGUI.LabelField (_rectColC, "Hide");
            _rectColC.x += 20;
            _rectColC.width = 22;
            _rectColC.y -= row;
            _rectColC.x += EditorGUIUtility.labelWidth - 30;
            for (int i = 0; i < p_childIds.arraySize; i++) {
                _rectColC.height += row2;
                GUI.Box (_rectColC, "", "textfield");
                _rectColC.x += 3;
                _rectColC.height -= row2;
                SerializedProperty _int = p_childIds.GetArrayElementAtIndex (i);
                EditorGUI.LabelField (_rectColC, _int.intValue.ToString ());
                _rectColC.y += row;
                if (i < p_childHides.arraySize) {
                    SerializedProperty _bool = p_childHides.GetArrayElementAtIndex (i);
                    _bool.boolValue = EditorGUI.Toggle (_rectColC, _bool.boolValue);
                }
                _rectColC.x += childSpace - 3;
                _rectColC.y -= row;
            }

            EditorGUI.EndProperty ();
            EditorGUI.indentLevel = defIndent;
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            return height;
        }
    }
}
