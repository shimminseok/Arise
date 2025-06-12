using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITable
{
    public Type Type { get; }
    public void AutoAssignDatas();

    public void CreateTable();
}