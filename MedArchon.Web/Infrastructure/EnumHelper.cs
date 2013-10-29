using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace MedArchon.Web.Infrastructure
{
    public class EnumHelper
    {
        internal static List<ListItem> CreateEnumListItems<T>(ResourceManager resourceManager = null)
        {
            Type enumType = typeof (T);

            List<ListItem> errorTypes = (from Enum value in Enum.GetValues(enumType)
                                         select new ListItem { Text = GetLocalizedLabel(@value, resourceManager), Value = @value.ToString("d") }).ToList();
            return errorTypes;
        }

        public static string GetLocalizedLabel(Enum enumeration, ResourceManager resourceManager)
        {
            return resourceManager == null ? enumeration.ToString() : resourceManager.GetString(enumeration.ToString("d"));
        }
    }
}