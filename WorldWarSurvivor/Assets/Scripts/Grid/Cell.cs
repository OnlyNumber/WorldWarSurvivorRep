using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int Coordinate;

    public bool IsObstacle;

    #region removeLater
    public TMPro.TMP_Text FullCost;
    public TMPro.TMP_Text PassedCost;
    public TMPro.TMP_Text NeedToGoCost;
    #endregion

    public void Initialize(Vector2Int coordintes)
    {
        Coordinate = coordintes;
    }

    
}
