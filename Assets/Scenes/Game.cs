
using UnityEngine;
using vitamin;

namespace game
{
    public class Game : Context
    {
        override internal void initialize()
        {
            GameObject cc = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cc.transform.parent = this.GetComponent<Transform>();
            bool b = true;
            //监听框架事件
            this.onEvent<vitamin.Event>("SHARE_CLICK", (vitamin.Event e) =>
            {
                //vitamin.Logger.Log(e.ToString());
                if (b)
                {
                    Tween.Get(cc).Prop(TweenProp.Rotation).To(new Vector3(45, 45, 0)).Ease(EaseType.CubicOut).Start(.4f);
                }
                else
                {
                    Tween.Get(cc).Prop(TweenProp.Rotation).To(Vector3.zero).Ease(EaseType.CubicOut).Start(.4f);
                }
                b = !b;
            });
        }

        void eventhandler(vitamin.Event e)
        {
            vitamin.Logger.Log(e.ToString());
        }

        void Update()
        {

        }
    }
}
