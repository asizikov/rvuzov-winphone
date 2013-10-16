using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlurryWP7SDK.Models;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public sealed class FlurryPublisherImpl : FlurryPublisher
    {
        protected override void InitSession(string userId)
        {
            FlurryWP7SDK.Api.StartSession("B7ZKJVBYQFMP8V683XRY");
            FlurryWP7SDK.Api.SetUserId(userId);
            FlurryWP7SDK.Api.SetVersion("dev_phone");
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
