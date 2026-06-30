using System;
using System.Collections.Generic;

public class UnitAction
{
    public ActingObject Owner;

    public int ActionCost;

    public string DescribeText;

    public Action<Cell> Activation;

    public List<Cell> AvailableCells;
}
