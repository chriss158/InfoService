using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using Alignment = MediaPortal.GUI.Library.GUIControl.Alignment;
using VAlignment = MediaPortal.GUI.Library.GUIControl.VAlignment;

namespace InfoService.GUIWindows
{
    public sealed class GUINotifyBar : GUIDialogWindow
    {
        [SkinControl(4)]
        private GUIButtonControl btnClose = null;
        [SkinControl(7)]
        private GUIButtonControl btnOk = null;
        [SkinControl(3)]
        private GUILabelControl lblHeading = null;
        [SkinControl(5)]
        private GUIImage imgLogo = null;
    
        [SkinControl(6)]
        //private GUITextControl txtArea = null;
        private GUITextScrollUpControl txtArea = null;

        private int timeOutInSeconds = 5;
        private DateTime timeStart = DateTime.Now;
        private bool m_bNeedRefresh = false;
        private string logoUrl = string.Empty;

        public const int ID = 16004;

        public System.Action OkAction;

        public GUINotifyBar()
        {
            GetID = ID;
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.GetThemedSkinFile(@"\infoservice.notifybar.xml"));
        }

        #region Base Dialog Members

        public override void DoModal(int dwParentId)
        {
            timeStart = DateTime.Now;
            base.DoModal(dwParentId);
        }

        public override bool ProcessDoModal()
        {
            base.ProcessDoModal();
            TimeSpan timeElapsed = DateTime.Now - timeStart;
            if (TimeOut > 0)
            {
                if (timeElapsed.TotalSeconds >= TimeOut)
                {
                    PageDestroy();
                    return false;
                }
            }
            return true;
        }

        #endregion

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            base.OnClicked(controlId, control, actionType);

            if (control == btnClose)
            {
                PageDestroy();
            }
            else if (control == btnOk)
            {
                if (OkAction != null)
                {                   
                    OkAction();
                }
            }
        }

        public override bool OnMessage(GUIMessage message)
        {
            //needRefresh = true;
            switch (message.Message)
            {
                case GUIMessage.MessageType.GUI_MSG_WINDOW_DEINIT:
                    {
                        lblHeading.Label = string.Empty;
                        base.OnMessage(message);
                        return true;
                    }

                case GUIMessage.MessageType.GUI_MSG_WINDOW_INIT:
                    {
                        base.OnMessage(message);
                        if (imgLogo != null)
                        {
                            SetImage(logoUrl);
                        }
                    }

                    return true;
            }

            return base.OnMessage(message);
        }

        public override void Reset()
        {
            timeOutInSeconds = 5;
            logoUrl = string.Empty;
        }

        public void SetHeading(string strLine)
        {
            //LoadSkin();
            AllocResources();
            InitControls();

            lblHeading.Label = strLine;
        }


        public void SetHeading(int iString)
        {
            SetHeading(GUILocalizeStrings.Get(iString));
        }

        public void SetText(string text)
        {
            txtArea.Label = text;
        }

        public void SetImage(string filename)
        {
            logoUrl = filename;
            if (MediaPortal.Util.Utils.FileExistsInCache(filename))
            {
                if (imgLogo != null)
                {
                    imgLogo.SetFileName(filename);
                    m_bNeedRefresh = true;
                    imgLogo.IsVisible = true;
                }
            }
            else
            {
                if (imgLogo != null)
                {
                    imgLogo.IsVisible = false;
                    m_bNeedRefresh = true;
                }
            }
        }

        public void SetImageDimensions(Size size, bool keepAspectRatio, bool centered)
        {
            if (imgLogo == null)
            {
                return;
            }
            imgLogo.Width = size.Width;
            imgLogo.Height = size.Height;
            imgLogo.KeepAspectRatio = keepAspectRatio;
            imgLogo.ImageAlignment = GUIControl.Alignment.ALIGN_CENTER;
            imgLogo.ImageVAlignment = GUIControl.VAlignment.ALIGN_MIDDLE;
        }

        public int TimeOut
        {
            get { return timeOutInSeconds; }
            set { timeOutInSeconds = value; }
        }

        public override bool NeedRefresh()
        {
            if (m_bNeedRefresh)
            {
                m_bNeedRefresh = false;
                return true;
            }
            return false;
        }
    }
}
