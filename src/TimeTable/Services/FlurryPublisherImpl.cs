using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FlurryWP8SDK.Models;
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
            FlurryWP8SDK.Api.StartSession(Configuration.FlurryApiKey);
            FlurryWP8SDK.Api.SetUserId(GetPhoneUniqueId());
            FlurryWP8SDK.Api.SetVersion(Configuration.Version);
        }

        protected override void FlushEvent(string eventName, EventParameter[] parameters)
        {
            if (parameters.Length > 0)
            {
                var list = ToFlurryParameters(parameters);
                FlurryWP8SDK.Api.LogEvent(eventName, list);
            }
            else
            {
                FlurryWP8SDK.Api.LogEvent(eventName);
            }
        }

        protected override void SendError(Exception exception)
        {
            //FlurryWP8SDK.Api.LogError(exception.Message, exception);
            LogStackTrace(exception);
        }

        protected override void CloseSesstion()
        {
            FlurryWP8SDK.Api.EndSession();
        }

        private static List<Parameter> ToFlurryParameters(EventParameter[] parameters)
        {
            var result = new List<Parameter>();
            if (parameters.Length == 0) return result;

            result.AddRange(parameters.Select(eventParameter => new Parameter(eventParameter.Name, eventParameter.Value)));

            return result;
        }

        private static void LogStackTrace(Exception exception)
        {
            var stacktrace = exception.StackTrace.Substring(0, exception.
            StackTrace.Length >= 255 ? 255: exception.StackTrace.Length);
            FlurryWP8SDK.Api.LogError(stacktrace, exception);
        }
    }
}