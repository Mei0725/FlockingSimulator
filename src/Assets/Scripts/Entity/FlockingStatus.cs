using System;
using System.Collections.Generic;

namespace FlockingEntity
{
    [Serializable]
    public class FlockingStatus
    {
        public float t;
        public List<float> meanSpeed = new List<float>();
        public List<float> alignMetric = new List<float>();
        public List<float> cohesion = new List<float>();
    }
}
