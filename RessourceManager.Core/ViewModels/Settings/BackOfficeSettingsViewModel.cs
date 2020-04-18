using RessourceManager.Core.Models.V1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RessourceManager.Core.ViewModels.Settings
{
    public class BackOfficeSettingsViewModel
    {
        public string Id { get; set; }
        public List<FieldType> EmailSettings  { get; set; } = new List<FieldType>();
        public List<FieldType> ReservationSettings { get; set; } = new List<FieldType>();
        public List<FieldType> CalendarSettings { get; set; } = new List<FieldType>();
        public BackOfficeSettingsViewModel(BackOfficeSettings settings)
        {
            Id = settings.Id;
            foreach (var propertyInfo in typeof(BackOfficeSettingsViewModel).GetProperties())
            {
                if (propertyInfo.Name == "Id")
                    continue;

                var viewModelProp = this.GetType().GetProperty(propertyInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                var viewModelPropType =  Type.GetType($"RessourceManager.Core.Models.V1.{propertyInfo.Name}, RessourceManager.Core");
                foreach (var subPropertyInfo in viewModelPropType.GetProperties())
                {
                    var subProp = viewModelPropType.GetProperty(subPropertyInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                    var subField = GetField(propertyInfo.Name, subPropertyInfo.Name, GetAttributeType(subPropertyInfo), subProp.GetValue(GetPropValue(settings, propertyInfo.Name)));

                    Type objTyp = typeof(FieldType);
                    var IListRef = typeof(List<>);
                    Type[] IListParam = { objTyp };
                    object Result = Activator.CreateInstance(IListRef.MakeGenericType(IListParam));
                    Result.GetType().GetMethod("Add").Invoke(viewModelProp.GetValue(this), new[] { subField }); // This will Fill the appropriate property : EmailSettings,ReservationSettings,CalendarSettings

                }
            }
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private FieldType GetField(string propertyName, string subPropertyName,string type,dynamic value)
        {
            var propertyDefaults = DefaultSettings.Fields.FirstOrDefault(field => field.Key == propertyName).Value;
            if (propertyDefaults == null)
                return null;
            var defaults = propertyDefaults.FirstOrDefault(field => field.Name == subPropertyName);
            if (defaults == null)
                return null;
            switch (type)
            {
                case "Boolean":
                    return new BooleanType { Value = value, Label = defaults.Label , Name = subPropertyName };
                case "Integer":
                    return new IntegerType { Value = value ,Label = defaults.Label, Name = subPropertyName };
                case "Select":
                    var selectDefaults = (SelectType)defaults;
                    return new SelectType { Value = value, Label = defaults.Label ,Options = selectDefaults.Options, Name = subPropertyName };
               default:
                    return new StringType { Value = value, Label = defaults.Label, Name = subPropertyName };
            }
        }

        private string GetAttributeType(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(FieldTypeAttributeAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as FieldTypeAttributeAttribute).Name;
        }
    }

    public static class DefaultSettings
    {
        public static Dictionary<string, List<FieldType>> Fields = new Dictionary<string, List<FieldType>>
        {
            {
                "EmailSettings",
                new List<FieldType>
                        {
                            new StringType
                            {
                                    Label = "MailServer",
                                    Name = "MailServer",
                            },
                            new StringType
                            {
                                    Label = "MailPort",
                                    Name = "MailPort",
                            },
                            new StringType
                            {
                                    Label = "Sender",
                                    Name = "Sender",
                            },
                            new StringType
                            {
                                    Label = "SenderName",
                                    Name = "SenderName",
                            },
                            new StringType
                            {
                                    Label = "Password",
                                    Name = "Password",
                            },
                        }
            },
            {
                "ReservationSettings",
                new List<FieldType>
                {
                    new IntegerType
                    {
                            Label = "Max number per user",
                            Name = "MaxNumberOfReservationsPerUser",
                    },
                    new IntegerType
                    {
                            Label = "Max duration",
                            Name = "MaxDurationPerReservation",
                    },
                    new IntegerType
                    {
                            Label = "Maxinum number of reservations at the same time",
                            Name = "MaxNumberOfReservationsAtSamePeriodPerUser",
                    },
                    new IntegerType
                    {
                            Label = "interval of reservations",
                            Name = "IntervalAllowedForReservations",
                    },
                }
            },
            {
                "CalendarSettings",
                new List<FieldType>
                {
                    new SelectType
                    {
                            Label = "min time to shown in calendar",
                            Name = "minTime",
                            Options = new Options[]{
                                 new Options
                                 {
                                     Value = "00:00:00",
                                     Name = "00:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "01:00:00",
                                     Name = "01:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "02:00:00",
                                     Name = "02:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "03:00:00",
                                     Name = "03:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "04:00:00",
                                     Name = "04:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "05:00:00",
                                     Name = "05:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "06:00:00",
                                     Name = "06:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "07:00:00",
                                     Name = "07:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "08:00:00",
                                     Name = "08:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "09:00:00",
                                     Name = "09:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "10:00:00",
                                     Name = "10:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "11:00:00",
                                     Name = "11:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "12:00:00",
                                     Name = "12:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "13:00:00",
                                     Name = "13:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "14:00:00",
                                     Name = "14:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "15:00:00",
                                     Name = "15:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "15:00:00",
                                     Name = "15:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "16:00:00",
                                     Name = "16:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "17:00:00",
                                     Name = "17:00:00",
                                 },
                                  new Options
                                 {
                                     Value = "18:00:00",
                                     Name = "18:00:00",
                                 },
                                   new Options
                                 {
                                     Value = "19:00:00",
                                     Name = "19:00:00",
                                 },
                                    new Options
                                 {
                                     Value = "20:00:00",
                                     Name = "20:00:00",
                                 },
                                     new Options
                                 {
                                     Value = "21:00:00",
                                     Name = "21:00:00",
                                 },
                                      new Options
                                 {
                                     Value = "22:00:00",
                                     Name = "22:00:00",
                                 },
                                       new Options
                                 {
                                     Value = "23:00:00",
                                     Name = "23:00:00",
                                 },
                                }
                    },
                    new SelectType
                    {
                            Label = "Max time shown in calendar",
                            Name = "maxTime",
                            Options = new Options[]{
                                 new Options
                                 {
                                     Value = "00:00:00",
                                     Name = "00:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "01:00:00",
                                     Name = "01:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "02:00:00",
                                     Name = "02:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "03:00:00",
                                     Name = "03:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "04:00:00",
                                     Name = "04:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "05:00:00",
                                     Name = "05:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "06:00:00",
                                     Name = "06:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "07:00:00",
                                     Name = "07:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "08:00:00",
                                     Name = "08:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "09:00:00",
                                     Name = "09:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "10:00:00",
                                     Name = "10:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "11:00:00",
                                     Name = "11:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "12:00:00",
                                     Name = "12:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "13:00:00",
                                     Name = "13:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "14:00:00",
                                     Name = "14:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "15:00:00",
                                     Name = "15:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "15:00:00",
                                     Name = "15:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "16:00:00",
                                     Name = "16:00:00",
                                 },
                                 new Options
                                 {
                                     Value = "17:00:00",
                                     Name = "17:00:00",
                                 },
                                  new Options
                                 {
                                     Value = "18:00:00",
                                     Name = "18:00:00",
                                 },
                                   new Options
                                 {
                                     Value = "19:00:00",
                                     Name = "19:00:00",
                                 },
                                    new Options
                                 {
                                     Value = "20:00:00",
                                     Name = "20:00:00",
                                 },
                                     new Options
                                 {
                                     Value = "21:00:00",
                                     Name = "21:00:00",
                                 },
                                      new Options
                                 {
                                     Value = "22:00:00",
                                     Name = "22:00:00",
                                 },
                                       new Options
                                 {
                                     Value = "23:00:00",
                                     Name = "23:00:00",
                                 },
                                }
                    },
                    new SelectType
                    {
                            Label = "First day of the week",
                            Name = "firstDay",
                            Options = new Options[]{
                                 new Options
                                 {
                                     Value = "0",
                                     Name = "Sunday",
                                 },
                                 new Options
                                 {
                                     Value = "1",
                                     Name = "Monday",
                                 },
                                 new Options
                                 {
                                     Value = "2",
                                     Name = "Tuesday"
                                 },
                                 new Options
                                 {
                                     Value = "3",
                                     Name = "Wensday",
                                 },
                                 new Options
                                 {
                                     Value = "4",
                                     Name = "Thursday",
                                 },
                                 new Options
                                 {
                                     Value = "5",
                                     Name = "Friday",
                                 },
                                 new Options
                                 {
                                     Value = "6",
                                     Name = "Saturday",
                                 },
                                }
                    },
                    new SelectType
                    {
                            Label = "format of hours",
                            Name = "HourSlotLabelFormat",
                            Options = new Options[]
                            {
                                new Options
                                {
                                    Value = "numeric",
                                    Name = "would produce something like 6"
                                },
                                new Options
                                {
                                    Value = "2-digit",
                                    Name = "would produce something like 06"
                                },
                            }
                    },
                    new SelectType
                    {
                            Label = "format of minutes",
                            Name = "MinuteSlotLabelFormat",
                            Options = new Options[]
                            {
                                new Options
                                {
                                    Value = "numeric",
                                    Name = "would produce something like 6"
                                },
                                new Options
                                {
                                    Value = "2-digit",
                                    Name = "would produce something like 06"
                                },
                            }
                    },
                    new BooleanType
                    {
                            Label = "12-hour clock",
                            Name = "Hour12SlotLabelFormat",
                    },
                }
            },

        };
    }
    public class FieldType
    {
        public string Name { get; set; }
        public string Label { get; set; }
    }

    public class StringType : FieldType
    {
        public string Value { get; set; }
        public string Type { get; } = "String";
    }

    public class IntegerType : FieldType
    {
        public int Value { get; set; }
        public string Type { get; } = "Integer";
    }

    public class BooleanType : FieldType
    {
        public bool Value { get; set; }
        public string Type { get; } = "Boolean";
    }

    public class SelectType : FieldType
    {
        public dynamic Value { get; set; }
        public string Type { get; } = "Select";
        public IEnumerable<Options> Options { get; set; }
    }

    public class Options
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
