using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungAtt1
{
    [RequireComponent(typeof(Entity))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        KeyCode forwardKey = KeyCode.W;
        [SerializeField]
        KeyCode backKey = KeyCode.S;
        [SerializeField]
        KeyCode leftKey = KeyCode.A;
        [SerializeField]
        KeyCode rightKey = KeyCode.D;
        [SerializeField]
        KeyCode turnCWKey = KeyCode.E;
        [SerializeField]
        KeyCode turnCCWKey = KeyCode.Q;

        static string keyStorageLocation = "player.keybinding";

        static KeyCode LoadPlayerKeyBinding(string key, KeyCode defaultKey)
        {
            return (KeyCode)PlayerPrefs.GetInt(key, (int)defaultKey);
        }

        static string StorageLocation(Navigation navigation) => $"{keyStorageLocation}.{navigation.GetStringValue()}";

        Entity entity;

        private void Awake()
        {
            forwardKey = LoadPlayerKeyBinding(StorageLocation(Navigation.Forward), forwardKey);
            backKey = LoadPlayerKeyBinding(StorageLocation(Navigation.Backward), backKey);
            leftKey = LoadPlayerKeyBinding(StorageLocation(Navigation.Left), leftKey);
            rightKey = LoadPlayerKeyBinding(StorageLocation(Navigation.Right), rightKey);
            turnCWKey = LoadPlayerKeyBinding(StorageLocation(Navigation.TurnCW), turnCWKey);
            turnCCWKey = LoadPlayerKeyBinding(StorageLocation(Navigation.TurnCCW), turnCCWKey);

            entity = GetComponent<Entity>();
        }

        Navigation GetKeyPress()
        {
            if (Input.GetKeyDown(forwardKey)) return Navigation.Forward;
            if (Input.GetKeyDown(backKey)) return Navigation.Backward;
            if (Input.GetKeyDown(leftKey)) return Navigation.Left;
            if (Input.GetKeyDown(rightKey)) return Navigation.Right;
            if (Input.GetKeyDown(turnCWKey)) return Navigation.TurnCW;
            if (Input.GetKeyDown(turnCCWKey)) return Navigation.TurnCCW;
            return Navigation.None;
        }

        bool isMoving = false;

        List<Navigation> navigationQueue = new List<Navigation>(2) { Navigation.None, Navigation.None };

        void Update()
        {
            var nav = GetKeyPress();
            switch (nav)
            {
                case Navigation.None:
                    return;
                default:
                    if (isMoving)
                    {
                        navigationQueue[1] = nav;
                    }
                    else
                    {
                        navigationQueue[0] = nav;
                        StartCoroutine(Move());
                    }
                    break;
            }
        }
        [SerializeField, Range(0, 2)]
        float turnTime = 0.4f;
        [SerializeField, Range(0, 2)]
        float moveTime = 0.5f;
        [SerializeField, Range(0, 2)]
        float blockTime = 0.2f;

        /// <summary>
        /// Query level if position can be reserved / entered by player
        /// </summary>
        /// <param name="navigation">Relative navigation</param>
        /// <returns></returns>
        bool ClaimSpot(Navigation navigation, Vector3 target)
        {
            if (navigation.Translates())
            {
                if (Level.Instance.GetVoxel(entity.CurrentScale, target, out Voxel voxel))
                {

                    entity.ClaimPosition(voxel);
                    return true;
                }
                return false;
            }
            return true;
        }

        void PathBlocked()
        {

        }

        Vector3 GetNavigationTraget(Navigation navigation)
        {
            float scale = entity.CurrentScale;

            switch (navigation)
            {
                case Navigation.Forward:
                    return transform.position + transform.forward * scale;
                case Navigation.Backward:
                    return transform.position + transform.forward * -1 * scale;
                case Navigation.Left:
                    return transform.position + transform.right * -1 * scale;
                case Navigation.Right:
                    return transform.position + transform.right * scale;
                default:
                    return Vector3.zero;
            }
        }

        IEnumerator<WaitForSeconds> Move()
        {
            isMoving = true;
            int moveIndex = 0;
            while (true)
            {
                Navigation nav = navigationQueue[moveIndex];
                if (nav == Navigation.None)
                {
                    break;
                }
                navigationQueue[moveIndex] = Navigation.None;

                System.Action<float> action = null;
                System.Action afterAction = null;

                float duration = 0;

                if (nav.Translates())
                {
                    Vector3 origin = transform.position;
                    Vector3 target = GetNavigationTraget(nav);

                    if (ClaimSpot(nav, target))
                    {
                        target = entity.CurrentPosition;
                        duration = moveTime;

                        action = (float progress) => { transform.position = Vector3.Lerp(origin, target, progress); };
                        afterAction = () => { transform.position = target; };
                    }
                    else
                    {
                        PathBlocked();
                    }

                }
                else if (nav.Rotates())
                {
                    Quaternion origin = transform.rotation;
                    var startRotation = origin.eulerAngles.y;
                    var endRotation = startRotation + ((nav == Navigation.TurnCW) ? 90 : -90);
                    Quaternion target = Quaternion.Euler(0, endRotation, 0);

                    duration = turnTime;
                    action = (float progress) => { transform.rotation = Quaternion.Lerp(origin, target, progress); };
                    afterAction = () => { transform.rotation = target; };
                }

                if (action != null)
                {
                    float start = Time.timeSinceLevelLoad;
                    float progress = 0;
                    while (progress < 1)
                    {
                        progress = (Time.timeSinceLevelLoad - start) / duration;
                        action(progress);
                        yield return new WaitForSeconds(Mathf.Max(0.02f, duration / 100f));
                    }

                    if (afterAction != null) afterAction();
                }


                moveIndex = 1;
            }


            isMoving = false;
        }
    }

}