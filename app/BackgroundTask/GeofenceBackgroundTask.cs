using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class GeofenceBackgroundTask : IBackgroundTask
    {
        void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                // Handle geofence state change reports
                GetGeofenceStateChangedReports();
            }
            catch (Exception e)
            {
                DoToast("Unautorized");
            }
        }

        private void GetGeofenceStateChangedReports()
        {
            GeofenceMonitor monitor = GeofenceMonitor.Current;
            String geofenceItemEvent = null;

            var reports = GeofenceMonitor.Current.ReadReports();
            foreach (var report in reports)
            {
                GeofenceState state = report.NewState;
                geofenceItemEvent = report.Geofence.Id;
                if (state == GeofenceState.Removed)
                {
                    GeofenceRemovalReason reason = report.RemovalReason;
                    if (reason == GeofenceRemovalReason.Expired)
                    {
                        geofenceItemEvent += " (Removed/Expired)";
                    }
                    else if (reason == GeofenceRemovalReason.Used)
                    {
                        geofenceItemEvent += " (Removed/Used)";
                    }
                }
                else if (state == GeofenceState.Entered)
                {
                    geofenceItemEvent += " (Entered)";
                }
                else if (state == GeofenceState.Exited)
                {
                    geofenceItemEvent += " (Exited)";
                }
            }
            DoToast(geofenceItemEvent);
        }

        private void DoToast(String eventName)
        {
            // pop a toast for each geofence event
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();

            // Create a two line toast and add audio reminder

            // Here the xml that will be passed to the 
            // ToastNotification for the toast is retrieved
            // https://msdn.microsoft.com/en-us/library/windows/apps/br208660.aspx <-- All the different ToastTemplateTypes.
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            // Set both lines of text
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("Geolocation GeofenceBackgroundTask"));
            if (!String.IsNullOrEmpty(eventName))
            {
                toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(eventName));
            }
            String secondLine = "geofenceBackgroundTask";
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(secondLine));

            // now create a xml node for the audio source
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            // https://msdn.microsoft.com/en-us/library/windows/apps/br230842.aspx <-- Different notification sounds
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotifier.Show(toast);
        }
    }
}
