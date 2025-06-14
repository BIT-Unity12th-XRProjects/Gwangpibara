using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EnumParser
{
    public static T ParseEnum<T>(string inEnumStr) where T : Enum
    {
        T parseEnum = (T)Enum.Parse(typeof(T), inEnumStr);
        return parseEnum;
    }
}
