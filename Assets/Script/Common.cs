using System.Collections;
using System.Collections.Generic;

namespace Common {
    public static class TamaBaseInfo
    {
        public static float HeroWidth = 0.5f;
        public static float HeroHeight = 1f;

        public const float MAX_HEALTH = 150f;
        public const float MAX_ENDURENCE = 150f;
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
        Katateken, Boom
    }

    public enum VideoName { 
        None = 0,
        BeginAnimation
    }

    public enum UIStatus
    {
        None,
        Common, Menu0, Menu1, Shotcut, Conversation, Shop
    }

    public enum UIDataType
    {
        None,
        Health, Endurence, Shotcut0, Shotcut1, Shotcut2, Shotcut3, Time, Temperature, Detector
    }

    public enum UIIconType
    {
        None,
        Blank, Ability, Item 
    }
}
