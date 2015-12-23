using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TwitterConnector.OAuth;

namespace TwitterConnector
{
    public static class StatusUpdate
    {
        public static bool PostStatus(string user, string password, string msg)
        {
            try
            {
                // encode the username/password
                string username = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user + ":" + password));
                // determine what we want to upload as a status
                byte[] bytes = Encoding.ASCII.GetBytes("status=" + msg);

                // connect with the update page
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.twitter.com/1/statuses/update.xml");
                // set the method to POST
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false; // thanks to argodev for this recent change!
                // set the authorisation levels
                request.Headers.Add("Authorization", "Basic " + username);
                request.ContentType = "application/x-www-form-urlencoded";
                // set the length of the content
                request.ContentLength = bytes.Length;

                // set up the stream
                Stream reqStream = request.GetRequestStream();
                // write to the stream
                reqStream.Write(bytes, 0, bytes.Length);
                // close the stream
                reqStream.Close();
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnError(new TwitterArgs("Error posting status update", ex.Message, ex.StackTrace));
                return false;
            }
            return true;
        }
        public static bool PostStatus(AccessToken accessToken, string msg)
        {
            try
            {
                Consumer c = new Consumer(Twitter.CONSUMER_KEY, Twitter.CONSUMER_SECRET);
                WebResponse resp =
                    c.AccessProtectedResource(
                        accessToken,
                        "http://api.twitter.com/1/statuses/update.xml",
                        "POST",
                        "http://twitter.com/",
                        new Parameter[]{
                        new Parameter(
                            "status",
                            msg
                            )
                        }
                    );
                if (resp != null) resp.Close();
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnError(new TwitterArgs("Error posting status update", ex.Message, ex.StackTrace));
                return false;
            }
            return true;
        }
    }
}
