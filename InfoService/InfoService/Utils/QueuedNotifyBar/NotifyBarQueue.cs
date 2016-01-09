using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoService.GUIWindows;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using System.Collections.Generic;

namespace InfoService.Utils.QueuedNotifyBar
{
    public static class NotifyBarQueue
    {
        private static readonly Logger logger = Logger.GetInstance();

        private static Queue<NotifyBarPopupInfo> _notifyQueue = new Queue<NotifyBarPopupInfo>();

        private static void ShowDialogNotifyWindow(string header, string text, string imagePath, System.Drawing.Size imageSize, int timeout, System.Action action = null)
        {
            logger.WriteLog("Show notify window with image", LogLevel.Info, InfoServiceModul.InfoService);
            object window = null;
            bool notifyBarFound;
            if (!InfoServiceUtils.AreNotifyBarSkinFilesInstalled())
            {
                window = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                notifyBarFound = false;
            }
            else
            {
                window = (GUINotifyBar)GUIWindowManager.GetWindow(GUINotifyBar.ID);
                notifyBarFound = true;
            }

            if (window != null)
            {
                if (notifyBarFound)
                {
                    if (action != null) ((GUINotifyBar)window).OkAction = action;
                    ((GUINotifyBar)window).OnPageDestroy += NotifyBarQueue_OnPageDestroy;
                    ((GUINotifyBar)window).SetHeading(header);
                    ((GUINotifyBar)window).SetText(text);
                    ((GUINotifyBar)window).SetImage(imagePath);
                    ((GUINotifyBar)window).TimeOut = timeout;
                    ((GUINotifyBar)window).SetImageDimensions(imageSize, true, true);
                    ((GUINotifyBar)window).DoModal(GUIWindowManager.ActiveWindow);

                }
                else
                {
                    ((GUIDialogNotify)window).SetHeading(header);
                    ((GUIDialogNotify)window).SetText(text);
                    ((GUIDialogNotify)window).SetImage(imagePath);
                    ((GUIDialogNotify)window).TimeOut = timeout;
                    ((GUIDialogNotify)window).SetImageDimensions(imageSize, true, true);
                    ((GUIDialogNotify)window).DoModal(GUIWindowManager.ActiveWindow);
                }
            }
        }

        private static void NotifyBarQueue_OnPageDestroy(string header, string text)
        {
            if (_notifyQueue.Count >= 1)
            {
                DequeueAndShowNotifyWindow();
            }
        }

        private static void DequeueAndShowNotifyWindow()
        {
            NotifyBarPopupInfo barInfo = _notifyQueue.Dequeue();
            if (barInfo.Action != null)
            {
                ShowDialogNotifyWindow(barInfo.Header, barInfo.Text, barInfo.ImagePath, barInfo.ImageSize,
                    barInfo.Timeout, barInfo.Action);
            }
            else
            {
                ShowDialogNotifyWindow(barInfo.Header, barInfo.Text, barInfo.ImagePath, barInfo.ImageSize,
                    barInfo.Timeout);
            }
        }

        public static void ShowDialogNotifyWindowQueued(string header, string text, string imagePath, System.Drawing.Size imageSize, int timeout)
        {
            ShowDialogNotifyWindowQueued(header, text, imagePath, imageSize, timeout, null);
        }
        public static void ShowDialogNotifyWindowQueued(string header, string text, string imagePath, System.Drawing.Size imageSize, int timeout, System.Action action)
        {
            if (InfoServiceUtils.AreNotifyBarSkinFilesInstalled())
            {
                NotifyBarPopupInfo popoBarPopupInfo = action != null ? new NotifyBarPopupInfo(header, text, imagePath, imageSize, timeout, action) : new NotifyBarPopupInfo(header, text, imagePath, imageSize, timeout);
                _notifyQueue.Enqueue(popoBarPopupInfo);
                if (_notifyQueue.Count == 1)
                {
                    DequeueAndShowNotifyWindow();
                }
            }
            else
            {
                ShowDialogNotifyWindow(header, text, imagePath, imageSize, timeout);
            }
        }
    }
}
