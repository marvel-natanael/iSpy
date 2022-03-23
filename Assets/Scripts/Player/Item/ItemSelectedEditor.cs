using System;
using UnityEditor;


namespace Player.Item
{
    [CustomEditor(typeof(ItemPickUp))]
    public class ItemSelectedEditor : Editor
    {
        private SerializedProperty _itemChoice;
        private SerializedProperty _bulletType;

        private void OnEnable()
        {
            _itemChoice = serializedObject.FindProperty("itemChoice");
            _bulletType = serializedObject.FindProperty("bulletType");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _itemChoice.enumValueIndex = (int) (ItemChoice) EditorGUILayout.EnumPopup("Item Type",
                (ItemChoice) Enum.GetValues(typeof(ItemChoice)).GetValue(_itemChoice.enumValueIndex));

            if (_itemChoice.enumValueIndex == (int) ItemChoice.Amount)
            {
                EditorGUILayout.PropertyField(_bulletType);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}