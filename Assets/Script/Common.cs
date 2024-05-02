using System.Collections;
using System.Collections.Generic;

namespace Common {
    public static class Hero
    {
        public static float HeroWidth = 0.5f;
        public static float HeroHeight = 1f;
    }

    public enum EventType
    {
        None = 0,
        Sound, Light, Heat
    }

    public struct Event { 
        public EventType Type;
        public float x;
        public float y;
        public float strength;
        public float vecX;
        public float vecY;
    }

    public enum Weapon
    {
        None = 0,
        Katateken
    }
}
