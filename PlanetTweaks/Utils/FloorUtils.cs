using ADOFAI.ModdingConvenience;
using ByteSheep.Events;
using DG.Tweening;
using UnityEngine;

namespace PlanetTweaks.Utils
{
    public static class FloorUtils
    {
        public static scrFloor AddFloor(float x, float y, Transform parent = null)
        {
            var obj = CreateFloor(parent);
            obj.transform.position = new Vector3(x, y);
            return obj;
        }

        public static scrFloor AddEventFloor(float x, float y, QuickAction action, Transform parent = null)
        {
            var obj = CreateGem(parent);
            obj.transform.position = new Vector3(x, y);
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
                    scrFloor component = GetFloor(targetX, targetY).GetComponent<scrFloor>();
                    scrController.instance.planetList.ForEach(p => p.currfloor = component);
                });
            }, parent);
        }

        public static scrFloor CreateFloor(Transform parent = null)
        {
            return Object.Instantiate(PrefabLibrary.instance.scnLevelSelectFloorPrefab, parent);
        }

        public static scrFloor CreateGem(Transform parent = null)
        {
            scrFloor floor = Object.Instantiate(GameObject.Find("outer ring").transform.Find("ChangingRoomGem").Find("MovingGem"), parent).GetComponent<scrFloor>();
            Object.DestroyImmediate(floor.GetComponent<scrGem>());
            Object.DestroyImmediate(floor.GetComponent<scrDisableIfWorldNotComplete>());
            Object.DestroyImmediate(floor.GetComponent<ffxCallFunction>());
            floor.DOKill(true);
            floor.gameObject.SetActive(false);
            scrGem gem = floor.gameObject.AddComponent<scrGem>();
            gem.Method("LocalRotate");
            Object.DestroyImmediate(gem);
            floor.gameObject.SetActive(true);
            return floor;
        }

        public static scrFloor GetFloor(float x, float y)
        {
            var array = Physics2D.OverlapPointAll(new Vector2(x, y), 1 << LayerMask.NameToLayer("Floor"));
            return array.Length == 0 ? null : array[0].gameObject.GetComponent<scrFloor>();
        }
    }
}
