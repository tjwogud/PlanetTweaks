using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class ComponentUtils
    {
        public static scrFloor GetFloor(this Component component)
        {
            return component.transform.parent.GetChild(0).GetComponent<scrFloor>();
        }

        public static scrFloor GetFloor(this Transform transform)
        {
            return transform.GetChild(0).GetComponent<scrFloor>();
        }

        public static TextMesh GetName(this Component component)
        {
            return component.transform.parent.GetChild(1).GetComponent<TextMesh>();
        }

        public static TextMesh GetName(this Transform transform)
        {
            return transform.GetChild(1).GetComponent<TextMesh>();
        }

        public static TextMesh GetPreview(this Component component)
        {
            return component.transform.parent.GetChild(2).GetComponent<TextMesh>();
        }

        public static TextMesh GetPreview(this Transform transform)
        {
            return transform.GetChild(2).GetComponent<TextMesh>();
        }

        public static SpriteRenderer GetIcon(this Component component)
        {
            return component.transform.parent.GetChild(3).GetComponent<SpriteRenderer>();
        }

        public static SpriteRenderer GetIcon(this Transform transform)
        {
            return transform.GetChild(3).GetComponent<SpriteRenderer>();
        }
    }
}
