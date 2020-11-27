using System.Collections;
using vitamin;
using UnityEngine;

public class Game1 : Entry
{
    // Start is called before the first frame update
    void Start()
    {

        GameObject cc = GameObject.CreatePrimitive(PrimitiveType.Cube);

        bool b = true;
        onUIEvent<vitamin.Event>("SHARE_CLICK", (vitamin.Event e) =>
        {
            vitamin.Logger.Log(e.ToString());
            if (b)
            {
                Tween.Get(cc).Prop(TweenProp.Rotation).To(new Vector3(45,45,0)).Ease(EaseType.CubicOut).Start(.4f);
            }else
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

    // Update is called once per frame
    void Update()
    {

    }
}
