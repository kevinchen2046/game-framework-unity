using UnityEngine;
using System;
using System.Reflection;
namespace vitamin
{
    public delegate object FuncHandler(params object[] args);

    public class Util
    {
        public static void SetProperty(object entity, string fieldname, object value)
        {
            Type entityType = entity.GetType();
            FieldInfo fieldInfo = entityType.GetField(fieldname);
            if (fieldInfo != null) fieldInfo.SetValue(entity, value);
        }

        public static void FuncCall(FuncHandler func, params object[] args)
        {
            switch (args.Length)
            {
                case 1: func(args[0]); break;
                case 2: func(args[0], args[1]); break;
                case 3: func(args[0], args[1], args[2]); break;
                case 4: func(args[0], args[1], args[2], args[3]); break;
                case 5: func(args[0], args[1], args[2], args[3], args[4]); break;
                case 6: func(args[0], args[1], args[2], args[3], args[4], args[5]); break;
                case 7: func(args[0], args[1], args[2], args[3], args[4], args[5], args[6]); break;
                case 8: func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]); break;
                case 9: func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]); break;
                case 10: func(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]); break;
            }
        }
    }
}