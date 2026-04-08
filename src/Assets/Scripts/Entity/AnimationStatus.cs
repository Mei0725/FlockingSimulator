using System;
using System.Collections.Generic;

namespace FlockingEntity
{
    [Serializable]
    public class Agents
    {
        public List<V3> positions = new List<V3>();
    }

    [Serializable]
    public class AnimationStatus
    {
        public float t;
        public List<Agents> agents = new List<Agents> ();
        public List<Obstacle> obs;
    }
}
