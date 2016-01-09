using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace InfoService.Utils.QueuedNotifyBar
{
    public class NotifyBarPopupInfo
    {
        public string Header { get; private set; }
        public string Text { get; private set; }
        public string ImagePath { get; private set; }
        public Size ImageSize { get; private set; }
        public int Timeout { get; private set; }
        public Action Action { get; private set; }

        public NotifyBarPopupInfo(string header, string text, string imagePath, Size imageSize, int timeout, Action action)
        {
            Header = header;
            Text = text;
            ImagePath = imagePath;
            ImageSize = imageSize;
            Timeout = timeout;
            Action = action;
        }
        public NotifyBarPopupInfo(string header, string text, string imagePath, Size imageSize, int timeout)
        {
            Header = header;
            Text = text;
            ImagePath = imagePath;
            ImageSize = imageSize;
            Timeout = timeout;
            Action = null;
        }
    }
}
