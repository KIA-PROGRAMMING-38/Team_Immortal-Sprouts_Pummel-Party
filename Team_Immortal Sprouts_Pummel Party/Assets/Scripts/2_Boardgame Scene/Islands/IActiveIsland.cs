using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IActiveIsland
{
    void ActivateOnMoveStart(Transform playerTransform = null);
    void ActivateOnMoveInProgress(Transform playerTransform = null);
    void ActivateOnMoveEnd(Transform playerTransform = null);
    //void ActivateIsland(Transform playerTransform = null);
}

