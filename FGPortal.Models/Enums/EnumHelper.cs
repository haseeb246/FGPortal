using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models.Enums
{
    public static class EnumHelper
    {
            public static string GetKey(this Enum value)
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                if (fieldInfo == null) return null;
                var attribute = (DisplayAttribute)fieldInfo.GetCustomAttribute(typeof(DisplayAttribute));
                return attribute.Name.ToString();
            }
    }
}
