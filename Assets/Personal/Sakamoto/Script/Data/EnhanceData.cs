using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnhanceData
{
    public int EnhanceNum;
    public string EnhanceDescription;

    public EnhanceData(int enhanceNum, string enhanceDescription) 
    {
        EnhanceNum = enhanceNum;
        EnhanceDescription = enhanceDescription;
    }
}
