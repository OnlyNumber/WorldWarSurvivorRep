using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilitiesUI
{
    public static bool IsInsideOfSquareImage(Vector2 center, float width, float height, Vector2 position)
    {
        Vector3 min = new Vector3(center.x - width / 2, center.y - height / 2);
        Vector3 max = new Vector3(center.x + width / 2, center.y + height / 2);

        return position.x > min.x && position.x <= max.x && position.y > min.y && position.y <= max.y;
    }

    public static bool IsInsideOfSquareImage(Vector2 center, Rect rect, Vector2 position)
    {
        return IsInsideOfSquareImage(center, rect.width, rect.height, position);
    }
}
