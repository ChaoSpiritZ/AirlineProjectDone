using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class CopyMachine
    {
        public static T ShallowCopy<T>(T proto) //where T : new()
        {
            Type type_of_proto = proto.GetType();

            T clone = Activator.CreateInstance<T>();

            FieldInfo[] fields = type_of_proto.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                field.SetValue(clone, field.GetValue(proto));
            }

            return clone;
        }

        public static T DeepCopy<T>(T proto) where T : new()
        {
            Type type_of_proto = proto.GetType();

            T clone = (T)Activator.CreateInstance(type_of_proto);

            FieldInfo[] fields = type_of_proto.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType.IsClass && field.FieldType != typeof(string))
                {
                    field.SetValue(clone, DeepCopy(field.GetValue(proto)));
                }
                else
                {
                    field.SetValue(clone, field.GetValue(proto));
                }
            }

            return clone;
        }

        public static T DeepCopyBetter<T>(T proto, Dictionary<Type, object> references) where T : new()
        {

            Type type_of_proto = proto.GetType();

            if (references.ContainsValue(proto))
            {
                return proto;
            }
            else
            {
                references.Add(type_of_proto, proto);
            }

            T clone = (T)Activator.CreateInstance(type_of_proto);

            FieldInfo[] fields = type_of_proto.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType.IsClass && field.FieldType != typeof(string))
                {
                    field.SetValue(clone, DeepCopyBetter(field.GetValue(proto), references));
                }
                else
                {
                    field.SetValue(clone, field.GetValue(proto));
                }
            }

            return clone;
        }
    }
}
