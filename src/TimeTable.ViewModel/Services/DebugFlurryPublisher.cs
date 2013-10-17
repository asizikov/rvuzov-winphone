using System;
using System.Diagnostics;
using System.Text;

namespace TimeTable.ViewModel.Services
{
    public class DebugFlurryPublisher : FlurryPublisher
    {
        protected override void InitSession()
        {
            Debug.WriteLine("Flurry::StartSession");
        }

        protected override void FlushEvent(string eventName, EventParameter[] parameters)
        {
            var eventParams = string.Empty;
            if (parameters.Length > 0)
            {
                var bs = new StringBuilder();
                foreach (var parameter in parameters)
                {
                    bs.AppendFormat("{0} - {1};{2}", parameter.Name, parameter.Value, Environment.NewLine);
                }
                eventParams = bs.ToString();
            }

            Debug.WriteLine("Flurry::Event: {0};{1}parameters:{1}{2};", eventName, Environment.NewLine, eventParams);
        }

        protected override void SendError(Exception exception)
        {
            Debug.WriteLine("Flurry::PublishExeption. Message = {0}: Exception = {1}", exception.Message, exception);
        }

        protected override void CloseSesstion()
        {
            Debug.WriteLine("Flurry::EndSession");
        }
    }
}