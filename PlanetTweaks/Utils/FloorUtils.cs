using ByteSheep.Events;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class FloorUtils
    {
        public static scrFloor AddFloor(float x, float y, Transform parent = null)
        {
            var obj = Object.Instantiate(GetGameObjectAt(0, -20).GetComponent<scrFloor>(), parent);
            obj.transform.position = new Vector3(x, y);
            return obj;
        }

        public static scrFloor AddFloorAt(float floorX, float floorY, float x, float y, Transform parent = null)
        {
            var floor = GetGameObjectAt(floorX, floorY)?.GetComponent<scrFloor>();
            if (floor == null)
                return null;
            var obj = Object.Instantiate(floor, parent);
            obj.transform.position = new Vector3(x, y);
            return obj;
        }

        public static scrFloor AddEventFloor(float x, float y, QuickAction action, Transform parent = null)
        {
            var obj = Object.Instantiate(GetGameObjectAt(0, -3).GetComponent<scrFloor>(), parent);
            Object.Destroy(obj.GetComponent<ffxCallFunction>());
            obj.transform.position = new Vector3(x, y);
            obj.transform.Find("SpecIcon").gameObject.SetActive(false);
            var func = obj.gameObject.AddComponent<ffxCallFunction>();
            func.ue = new QuickEvent();
            func.ue.persistentCalls = new QuickPersistentCallGroup();
            func.ue.AddListener(action);
            return obj;
        }

        public static scrFloor AddTeleportFloor(float x, float y, float targetX, float targetY, float cameraX, float cameraY, bool cameraMoving = true, PositionState state = PositionState.None, QuickAction action1 = null, QuickAction action2 = null, Transform parent = null)
        {
            return AddEventFloor(x, y, delegate
            {
                if (action1 != null)
                    action1.Invoke();
                scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, delegate
                {
                    if (action2 != null)
                        action2.Invoke();
                    scrController.instance.chosenplanet.transform.LocalMoveXY(targetX, targetY);
                    scrController.instance.chosenplanet.transform.position = new Vector3(targetX, targetY);
                    scrController.instance.camy.ViewObjectInstant(scrController.instance.chosenplanet.transform, false);
                    scrController.instance.camy.ViewVectorInstant(new Vector2(cameraX, cameraY), false);
                    scrController.instance.camy.isMoveTweening = cameraMoving;
                    scrController.instance.camy.positionState = state;
                    scrUIController.instance.WipeFromBlack();
                    scrFloor component = GetGameObjectAt(targetX, targetY).GetComponent<scrFloor>();
                    scrController.instance.chosenplanet.currfloor = component;
                });
            }, parent);
        }

        public static GameObject GetGameObjectAt(float x, float y)
        {
            var array = Physics2D.OverlapPointAll(new Vector2(x, y), 1 << LayerMask.NameToLayer("Floor"));
            return array.Length == 0 ? null : array[0].gameObject;
        }
    }
}
