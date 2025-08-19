using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShopItem))]
public class ShopItemCustom : Editor
{
    private SerializedProperty _canvasShop;

    private SerializedProperty _selectPointer;

    private SerializedProperty _itemTab;
    private SerializedProperty _hairItem;
    private SerializedProperty _pantItem;
    private SerializedProperty _shieldItem;
    private SerializedProperty _skinItem;

    private SerializedProperty _itemPrice;

    private void OnEnable()
    {
        _canvasShop = serializedObject.FindProperty("_canvasShop");
        _selectPointer = serializedObject.FindProperty("_selectPointer");
        _itemTab = serializedObject.FindProperty("_itemTab");
        _hairItem = serializedObject.FindProperty("_hairItem");
        _pantItem = serializedObject.FindProperty("_pantItem");
        _shieldItem = serializedObject.FindProperty("_shieldItem");
        _skinItem = serializedObject.FindProperty("_skinItem");
        _itemPrice = serializedObject.FindProperty("_itemPrice");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_canvasShop, new GUIContent("Canvas Shop"));
        EditorGUILayout.PropertyField(_selectPointer, new GUIContent("Pointer"));

        EditorGUILayout.LabelField("Shop", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_itemTab, new GUIContent("Tab"));
        Shop tab = (Shop)_itemTab.enumValueIndex;
        EditorGUI.indentLevel++;
        switch (tab)
        {
            case Shop.Hair:
                EditorGUILayout.LabelField("Hairs", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_hairItem, new GUIContent("Hair"));
                break;
            case Shop.Pant:
                EditorGUILayout.LabelField("Pants", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_pantItem, new GUIContent("Pant"));
                break;
            case Shop.Shield:
                EditorGUILayout.LabelField("Shields", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_shieldItem, new GUIContent("Shield"));
                break;
            case Shop.Skin:
                EditorGUILayout.LabelField("Skin", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_skinItem, new GUIContent("Skin"));
                break;
        }
        EditorGUILayout.PropertyField(_itemPrice, new GUIContent("Price"));
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
