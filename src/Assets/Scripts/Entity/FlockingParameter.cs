using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlockingEntity
{
    [Serializable]
    public class V3
    {
        public float x;
        public float y;
        public float z;

        public V3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public V3(float ix, float iy, float iz)
        {
            x = ix;
            y = iy;
            z = iz;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

    [Serializable]
    public class Obstacle
    {
        public V3 p;
        public float r;
    }


    [Serializable]
    public class FlockingParameter
    {
        public float ka;
        public float kc;
        public float ks;
        public float ko;
        public float kg;

        public int n;

        public List<Obstacle> obs;

        public float t;

        public List<V3> starts = new List<V3>();
    }
}
