using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BoolMatrix))]
public class BoolMatrixDrawer : PropertyDrawer
{
    private const float ToggleSize = 18f;
    private const float Spacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Заголовок компонента з можливістю згортання (Foldout)
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.isExpanded,
            label,
            true
        );

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            SerializedProperty widthProp = property.FindPropertyRelative("width");
            SerializedProperty heightProp = property.FindPropertyRelative("height");
            SerializedProperty rowsProp = property.FindPropertyRelative("rows");

            float lineHeight = EditorGUIUtility.singleLineHeight + Spacing;
            float currentY = position.y + lineHeight;

            // --- 1. Поля розмірів (Width / Height) ---
            EditorGUI.BeginChangeCheck();
            int newWidth = Mathf.Max(1, EditorGUI.IntField(
                new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight),
                "Width", widthProp.intValue));
            currentY += lineHeight;

            int newHeight = Mathf.Max(1, EditorGUI.IntField(
                new Rect(position.x, currentY, position.width, EditorGUIUtility.singleLineHeight),
                "Height", heightProp.intValue));
            currentY += lineHeight;

            if (EditorGUI.EndChangeCheck())
            {
                widthProp.intValue = newWidth;
                heightProp.intValue = newHeight;
                ResizeMatrix(rowsProp, newWidth, newHeight);
            }

            ValidateMatrixSize(rowsProp, widthProp.intValue, heightProp.intValue);

            currentY += 5f; // Відступ перед сіткою

            // --- 2. Малювання сітки (ЗНИЗУ ВГОРУ) ---
            int originalIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0; // Скидаємо відступ, щоб клік працював по всій площі

            float startX = EditorGUI.IndentedRect(position).x;
            int totalRows = rowsProp.arraySize;

            for (int r = 0; r < totalRows; r++)
            {
                SerializedProperty rowProp = rowsProp.GetArrayElementAtIndex(r);
                SerializedProperty colsProp = rowProp.FindPropertyRelative("cols");

                // Перераховуємо Y позицію: 
                // Рядок r = 0 буде намальовано в самому низу сітки.
                // Рядок r = totalRows - 1 буде намальовано в самому верху сітки.
                float visualRowY = currentY + ((totalRows - 1 - r) * ToggleSize);

                for (int c = 0; c < colsProp.arraySize; c++)
                {
                    SerializedProperty boolProp = colsProp.GetArrayElementAtIndex(c);

                    Rect toggleRect = new Rect(
                        startX + (c * ToggleSize),
                        visualRowY,
                        ToggleSize,
                        ToggleSize
                    );

                    boolProp.boolValue = GUI.Toggle(toggleRect, boolProp.boolValue, GUIContent.none);
                }
            }

            EditorGUI.indentLevel = originalIndent;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        SerializedProperty heightProp = property.FindPropertyRelative("height");
        int rows = heightProp != null ? Mathf.Max(1, heightProp.intValue) : 1;

        return EditorGUIUtility.singleLineHeight * 3 + (rows * ToggleSize) + 15f;
    }

    private void ValidateMatrixSize(SerializedProperty rowsProp, int width, int height)
    {
        if (rowsProp.arraySize != height)
        {
            ResizeMatrix(rowsProp, width, height);
            return;
        }

        for (int r = 0; r < rowsProp.arraySize; r++)
        {
            SerializedProperty colsProp = rowsProp.GetArrayElementAtIndex(r).FindPropertyRelative("cols");
            if (colsProp.arraySize != width)
            {
                ResizeMatrix(rowsProp, width, height);
                return;
            }
        }
    }

    private void ResizeMatrix(SerializedProperty rowsProp, int width, int height)
    {
        rowsProp.arraySize = height;

        for (int r = 0; r < height; r++)
        {
            SerializedProperty colsProp = rowsProp.GetArrayElementAtIndex(r).FindPropertyRelative("cols");
            colsProp.arraySize = width;
        }
    }
}