using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImYellowFish.Utility
{
    public static class Helper
    {
        public static float ClampAbs(float x, float maxMag)
        {
            if (x > maxMag)
                return maxMag;
            else if (x < -maxMag)
                return -maxMag;
            else
                return x;
        }
    }
}