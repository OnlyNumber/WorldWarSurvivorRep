using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : Cell
{    
    public GridObject gridObject;

    public bool IsObstacle;

    #region removeLater
    public TMPro.TMP_Text FullCost;
    public TMPro.TMP_Text PassedCost;
    public TMPro.TMP_Text NeedToGoCost;
    #endregion

    public GridObject ShowCell()
    {
        if (gridObject != null)
            gridObject.ShowActions();

        return gridObject;
    }

    public void CloseCell()
    {
        
    }
}
