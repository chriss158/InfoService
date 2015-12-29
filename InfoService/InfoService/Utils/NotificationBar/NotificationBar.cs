using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoService.Utils.NotificationBar
{
    public static class NotificationBar
    {
        public static void ShowNotificationBar(string text, string subText, string thumbnailPath, bool showFullscreenVideo, int timeout)
        {
            MPNotificationBar.NotificationBarManager.AddNotification(InfoServiceCore.GUIInfoServiceId, text, subText, thumbnailPath, timeout,
                                                   MPNotificationBar.NotificationBarManager.Types.Information, false,
                                                   showFullscreenVideo);
        }
    }
}
