using System;
using System.Collections.Generic;
using System.Linq;
using FlurryWP7SDK.Models;
using Microsoft.Phone.Info;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public sealed class FlurryPublisherImpl : FlurryPublisher
    {
        private static string GetPhoneUniqueId()
        {
            object uniqueId;
            if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
            {
                var result = (byte[]) uniqueId;
                return Convert.ToBase64String(result);
            }
            else
            {
                return "unknown";
            }
            
        }

        protected override void InitSession()
        {
            FlurryWP7SDK.Api.StartSession(Configuration.FlurryApiKey);
            FlurryWP7SDK.Api.SetUserId(GetPhoneUniqueId());
            FlurryWP7SDK.Api.SetVersion(Configuration.Version);
        }

        protected override void FlushEvent(string eventName, EventParameter[] parameters)
        {
            if (parameters.Length > 0)
            {
                var list = ToFlurryParameters(parameters);
                FlurryWP7SDK.Api.LogEvent(eventName, list);
            }
            else
            {
                FlurryWP7SDK.Api.LogEvent(eventName);
            }
        }

        protected override void SendError(Exception exception)
        {
            FlurryWP7SDK.Api.LogError(exception.Message, exception);
        }

        protected override void CloseSesstion()
        {
            FlurryWP7SDK.Api.EndSession();
        }

        private static List<Parameter> ToFlurryParameters(EventParameter[] parameters)
        {
            var result = new List<Parameter>();
            if (parameters.Length == 0) return result;

            result.AddRange(parameters.Select(eventParameter => new Parameter(eventParameter.Name, eventParameter.Value)));

            return result;
        }

    }

}
