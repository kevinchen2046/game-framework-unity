using UnityEngine;
using System;
namespace vitamin
{
    public enum TweenProp
    {
        None,
        X,
        Y,
        Z,
        XY,
        Position,
        Width,
        Height,
        Size,
        ScaleX,
        ScaleY,
        Scale,
        Rotation,
        RotationX,
        RotationY,
        Alpha,
        Progress,

        ScaleZ,
        RotationZ
    }
    public enum EaseType
    {
        Linear,
        SineIn,
        SineOut,
        SineInOut,
        QuadIn,
        QuadOut,
        QuadInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuartIn,
        QuartOut,
        QuartInOut,
        QuintIn,
        QuintOut,
        QuintInOut,
        ExpoIn,
        ExpoOut,
        ExpoInOut,
        CircIn,
        CircOut,
        CircInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        BackIn,
        BackOut,
        BackInOut,
        BounceIn,
        BounceOut,
        BounceInOut,
        Custom
    }

    public enum TweenTargetType
    {
        GameObject,
        FairyObject
    }

    public delegate void TweenStartHandler(Tween tween);
    public delegate void TweenCompleteHandler(Tween tween);
    public delegate void TweenUpdateHandler(Tween tween, float progress);
    public class Tween : FairyGUI.ITweenListener
    {
        public object target;
        private TweenTargetType targettype;
        public TweenProp prop;
        public EaseType ease;
        public object endValue;
        public object startValue;
        public float duration;
        public float delay;
        private TweenStartHandler starthandler;
        private TweenCompleteHandler completehandler;
        private TweenUpdateHandler updatehandler;
        public Tween(object target)
        {
            this.target = target;
            if (Util.InstaneOf<GameObject>(target))
            {
                targettype = TweenTargetType.GameObject;
            }
            else if (Util.InstaneOf<FairyGUI.GObject>(target))
            {
                targettype = TweenTargetType.FairyObject;
            }
        }
        public static Tween Get(object target)
        {
            return new Tween(target);
        }
        public Tween Prop(TweenProp prop)
        {
            this.prop = prop;
            return this;
        }
        public Tween To(object endValue, object startValue)
        {
            this.endValue = endValue;
            this.startValue = startValue;
            return this;
        }
        public Tween To(object endValue)
        {
            this.endValue = endValue;
            return this;
        }
        public Tween Ease(EaseType type)
        {
            this.ease = type;
            return this;
        }
        public Tween Delay(float value)
        {
            this.delay = value;
            return this;
        }

        public Tween onStart(TweenStartHandler handler)
        {
            this.starthandler = handler;
            return this;
        }
        public Tween onComplete(TweenCompleteHandler handler)
        {
            this.completehandler = handler;
            return this;
        }
        public Tween onUpdate(TweenUpdateHandler handler)
        {
            this.updatehandler = handler;
            return this;
        }
        public Tween Start(float duration)
        {
            if (startValue == null)
            {
                startValue = GetPropValue(prop);
            }
            if (targettype == TweenTargetType.FairyObject)
            {
                FairyGUI.GTweener gtween = null;
                switch (prop)
                {
                    case TweenProp.X:
                    case TweenProp.Y:
                    case TweenProp.Z:
                    case TweenProp.Width:
                    case TweenProp.Height:
                    case TweenProp.ScaleX:
                    case TweenProp.ScaleY:
                    case TweenProp.RotationX:
                    case TweenProp.RotationY:
                    case TweenProp.Alpha:
                    case TweenProp.Progress:
                        gtween = FairyGUI.GTween.To((float)startValue, (float)endValue, duration);
                        break;
                    case TweenProp.Position:
                    case TweenProp.Rotation:
                        gtween = FairyGUI.GTween.To((Vector3)startValue, (Vector3)endValue, duration);
                        break;
                    case TweenProp.XY:
                    case TweenProp.Size:
                    case TweenProp.Scale:
                        gtween = FairyGUI.GTween.To((Vector2)startValue, (Vector2)endValue, duration);
                        break;
                }
                if (gtween != null)
                {
                    gtween.SetTarget(this.target, GetFairyTweenProp(prop)).SetEase(GetFairyEase(ease));
                    if (delay > 0)
                    {
                        gtween.SetDelay(delay);
                    }
                    if (this.starthandler != null || this.completehandler != null || this.updatehandler != null)
                    {
                        gtween.SetListener(this);
                    }
                }
            }
            else if (targettype == TweenTargetType.GameObject)
            {
                System.Collections.Hashtable hash = iTween.Hash("easeType", GetiTweenEase(ease),"time",duration);
                if (delay > 0)
                {
                    hash.Add("delay", delay);
                }
                if (starthandler != null)
                {
                    hash.Add("onstart", "OnTweenStart");
                }
                if (completehandler != null)
                {
                    hash.Add("oncomplete", "OnTweenComplete");
                }
                if (updatehandler != null)
                {
                    hash.Add("onupdate", "OnTweenUpdate");
                }
                switch (prop)
                {
                    case TweenProp.X:
                    case TweenProp.ScaleX:
                    case TweenProp.RotationX:
                        hash.Add("x", this.endValue); break;
                    case TweenProp.Y:
                    case TweenProp.ScaleY:
                    case TweenProp.RotationY:
                        hash.Add("y", this.endValue); break;
                    case TweenProp.Z:
                    case TweenProp.ScaleZ:
                    case TweenProp.RotationZ:
                        hash.Add("z", this.endValue); break;
                    case TweenProp.XY:
                        hash.Add("x", ((Vector3)this.endValue).x);
                        hash.Add("y", ((Vector3)this.endValue).y);
                        break;
                    case TweenProp.Position:
                    case TweenProp.Scale:
                    case TweenProp.Rotation:
                        hash.Add("x", ((Vector3)this.endValue).x);
                        hash.Add("y", ((Vector3)this.endValue).y);
                        hash.Add("z", ((Vector3)this.endValue).z);
                        break;
                }
                switch (prop)
                {
                    case TweenProp.X:
                    case TweenProp.Y:
                    case TweenProp.Z:
                    case TweenProp.XY:
                    case TweenProp.Position:
                        iTween.MoveTo((GameObject)target, hash);
                        break;
                    case TweenProp.Width:
                    case TweenProp.Height:
                    case TweenProp.Size:
                        break;
                    case TweenProp.ScaleX:
                    case TweenProp.ScaleY:
                    case TweenProp.Scale:
                        iTween.ScaleTo((GameObject)target, hash);
                        break;
                    case TweenProp.Rotation:
                    case TweenProp.RotationX:
                    case TweenProp.RotationY:
                        Debug.Log("Tween Rotation");
                        Debug.Log(hash);
                        iTween.RotateTo((GameObject)target, hash);
                        break;
                    case TweenProp.Alpha:
                        //iTween.ColorTo((GameObject)target, hash);
                        break;
                }
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tweener"></param>
        public void OnTweenStart(FairyGUI.GTweener tweener)
        {
            if (this.starthandler != null)
            {
                this.starthandler.Invoke(this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tweener"></param>
        public void OnTweenUpdate(FairyGUI.GTweener tweener)
        {
            if (this.updatehandler != null)
            {
                this.updatehandler.Invoke(this, tweener.duration / duration);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tweener"></param>
        public void OnTweenComplete(FairyGUI.GTweener tweener)
        {
            if (this.completehandler != null)
            {
                this.completehandler.Invoke(this);
            }
        }

        private object GetPropValue(TweenProp prop)
        {
            if (target == null) return 0;

            if (targettype == TweenTargetType.FairyObject)
            {
                FairyGUI.GObject gtarget = target as FairyGUI.GObject;
                switch (prop)
                {
                    case TweenProp.None: return 0;
                    case TweenProp.X: return gtarget.x;
                    case TweenProp.Y: return gtarget.y;
                    case TweenProp.Z: return gtarget.z;
                    case TweenProp.XY: return gtarget.xy;
                    case TweenProp.Position: return gtarget.position;
                    case TweenProp.Width: return gtarget.width;
                    case TweenProp.Height: return gtarget.height;
                    case TweenProp.Size: return gtarget.size;
                    case TweenProp.ScaleX: return gtarget.scaleX;
                    case TweenProp.ScaleY: return gtarget.scaleY;
                    case TweenProp.Scale: return gtarget.scale;
                    case TweenProp.Rotation: return gtarget.rotation;
                    case TweenProp.RotationX: return gtarget.rotationX;
                    case TweenProp.RotationY: return gtarget.rotationY;
                    case TweenProp.Alpha: return gtarget.alpha;
                    case TweenProp.Progress: return gtarget.asProgress;
                }
            }
            return 0;
        }

        private FairyGUI.TweenPropType GetFairyTweenProp(TweenProp prop)
        {
            switch (prop)
            {
                case TweenProp.None: return FairyGUI.TweenPropType.None;
                case TweenProp.X: return FairyGUI.TweenPropType.X;
                case TweenProp.Y: return FairyGUI.TweenPropType.Y;
                case TweenProp.Z: return FairyGUI.TweenPropType.Z;
                case TweenProp.XY: return FairyGUI.TweenPropType.XY;
                case TweenProp.Position: return FairyGUI.TweenPropType.Position;
                case TweenProp.Width: return FairyGUI.TweenPropType.Width;
                case TweenProp.Height: return FairyGUI.TweenPropType.Height;
                case TweenProp.Size: return FairyGUI.TweenPropType.Size;
                case TweenProp.ScaleX: return FairyGUI.TweenPropType.ScaleX;
                case TweenProp.ScaleY: return FairyGUI.TweenPropType.ScaleY;
                case TweenProp.Scale: return FairyGUI.TweenPropType.Scale;
                case TweenProp.Rotation: return FairyGUI.TweenPropType.Rotation;
                case TweenProp.RotationX: return FairyGUI.TweenPropType.RotationX;
                case TweenProp.RotationY: return FairyGUI.TweenPropType.RotationY;
                case TweenProp.Alpha: return FairyGUI.TweenPropType.Alpha;
                case TweenProp.Progress: return FairyGUI.TweenPropType.Progress;
            }
            return FairyGUI.TweenPropType.None;
        }

        private FairyGUI.EaseType GetFairyEase(EaseType type)
        {

            switch (type)
            {
                case EaseType.Linear: return FairyGUI.EaseType.Linear;
                case EaseType.SineIn: return FairyGUI.EaseType.SineIn;
                case EaseType.SineOut: return FairyGUI.EaseType.SineOut;
                case EaseType.SineInOut: return FairyGUI.EaseType.SineInOut;
                case EaseType.QuadIn: return FairyGUI.EaseType.QuadIn;
                case EaseType.QuadOut: return FairyGUI.EaseType.QuadOut;
                case EaseType.QuadInOut: return FairyGUI.EaseType.QuadInOut;
                case EaseType.CubicIn: return FairyGUI.EaseType.CubicIn;
                case EaseType.CubicOut: return FairyGUI.EaseType.CubicOut;
                case EaseType.CubicInOut: return FairyGUI.EaseType.CubicInOut;
                case EaseType.QuartIn: return FairyGUI.EaseType.QuartIn;
                case EaseType.QuartOut: return FairyGUI.EaseType.QuartOut;
                case EaseType.QuartInOut: return FairyGUI.EaseType.QuartInOut;
                case EaseType.QuintIn: return FairyGUI.EaseType.QuintIn;
                case EaseType.QuintOut: return FairyGUI.EaseType.QuintOut;
                case EaseType.QuintInOut: return FairyGUI.EaseType.QuintInOut;
                case EaseType.ExpoIn: return FairyGUI.EaseType.ExpoIn;
                case EaseType.ExpoOut: return FairyGUI.EaseType.ExpoOut;
                case EaseType.ExpoInOut: return FairyGUI.EaseType.ExpoInOut;
                case EaseType.CircIn: return FairyGUI.EaseType.CircIn;
                case EaseType.CircOut: return FairyGUI.EaseType.CircOut;
                case EaseType.CircInOut: return FairyGUI.EaseType.CircInOut;
                case EaseType.ElasticIn: return FairyGUI.EaseType.ElasticIn;
                case EaseType.ElasticOut: return FairyGUI.EaseType.ElasticOut;
                case EaseType.ElasticInOut: return FairyGUI.EaseType.ElasticInOut;
                case EaseType.BackIn: return FairyGUI.EaseType.BackIn;
                case EaseType.BackOut: return FairyGUI.EaseType.BackOut;
                case EaseType.BackInOut: return FairyGUI.EaseType.BackInOut;
                case EaseType.BounceIn: return FairyGUI.EaseType.BounceIn;
                case EaseType.BounceOut: return FairyGUI.EaseType.BounceOut;
                case EaseType.BounceInOut: return FairyGUI.EaseType.BounceInOut;
                case EaseType.Custom: return FairyGUI.EaseType.Custom;
            }
            return 0;
        }
        private iTween.EaseType GetiTweenEase(EaseType type)
        {
            switch (type)
            {
                case EaseType.Linear: return iTween.EaseType.linear;
                case EaseType.SineIn: return iTween.EaseType.easeInSine;
                case EaseType.SineOut: return iTween.EaseType.easeOutSine;
                case EaseType.SineInOut: return iTween.EaseType.easeInOutSine;
                case EaseType.QuadIn: return iTween.EaseType.easeInQuad;
                case EaseType.QuadOut: return iTween.EaseType.easeOutQuad;
                case EaseType.QuadInOut: return iTween.EaseType.easeInOutQuad;
                case EaseType.CubicIn: return iTween.EaseType.easeInCubic;
                case EaseType.CubicOut: return iTween.EaseType.easeOutCubic;
                case EaseType.CubicInOut: return iTween.EaseType.easeInOutCubic;
                case EaseType.QuartIn: return iTween.EaseType.easeInQuart;
                case EaseType.QuartOut: return iTween.EaseType.easeOutQuart;
                case EaseType.QuartInOut: return iTween.EaseType.easeInOutQuart;
                case EaseType.QuintIn: return iTween.EaseType.easeInQuint;
                case EaseType.QuintOut: return iTween.EaseType.easeOutQuint;
                case EaseType.QuintInOut: return iTween.EaseType.easeInOutQuint;
                case EaseType.ExpoIn: return iTween.EaseType.easeInExpo;
                case EaseType.ExpoOut: return iTween.EaseType.easeOutExpo;
                case EaseType.ExpoInOut: return iTween.EaseType.easeInOutExpo;
                case EaseType.CircIn: return iTween.EaseType.easeInCirc;
                case EaseType.CircOut: return iTween.EaseType.easeOutCirc;
                case EaseType.CircInOut: return iTween.EaseType.easeInOutCirc;
                case EaseType.ElasticIn: return iTween.EaseType.easeInElastic;
                case EaseType.ElasticOut: return iTween.EaseType.easeOutElastic;
                case EaseType.ElasticInOut: return iTween.EaseType.easeInOutElastic;
                case EaseType.BackIn: return iTween.EaseType.easeInBack;
                case EaseType.BackOut: return iTween.EaseType.easeOutBack;
                case EaseType.BackInOut: return iTween.EaseType.easeInOutBack;
                case EaseType.BounceIn: return iTween.EaseType.easeInBounce;
                case EaseType.BounceOut: return iTween.EaseType.easeOutBounce;
                case EaseType.BounceInOut: return iTween.EaseType.easeInOutBounce;
                case EaseType.Custom: return iTween.EaseType.spring;
            }
            return 0;
        }
    }
}