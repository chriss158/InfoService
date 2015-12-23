#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using TwitterConnector.Data;
using TwitterConnector.OAuth;

#endregion

namespace TwitterConnector
{
    internal static class Utils
    {
        internal static XDocument DownloadTwitterXml(string user, string password, string url)
        {
            int rateLimit = 0, limitRemaining = 0;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Credentials = new NetworkCredential(user, password);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                GetInfoFromResponse(resp, out rateLimit, out limitRemaining);
                using (XmlReader reader = XmlReader.Create(resp.GetResponseStream()))
                {
                    return XDocument.Load(reader);
                }
            }
            catch (WebException wex)
            {
                LogEvents.InvokeOnError(new TwitterArgs(string.Format("Twitter rate limit exceeded, max of {0}/hr allowed. Remaining = {1}", rateLimit, limitRemaining), wex.Message, wex.StackTrace));
                return null;
            }

            catch (Exception ex)
            {
                LogEvents.InvokeOnError(new TwitterArgs("Error downloading twitter status from " + url, ex.Message, ex.StackTrace));
                return null;
            }
        }
        internal static XDocument DownloadTwitterXml(AccessToken accessToken, string url)
        {
            int rateLimit = 0, limitRemaining = 0;
            try
            {
                Consumer c = new Consumer(Twitter.CONSUMER_KEY, Twitter.CONSUMER_SECRET);
                HttpWebResponse resp = c.AccessProtectedResource(
                    accessToken,
                    url,
                    "GET",
                    "http://twitter.com/", new[]{ new Parameter("since_id","1")});
                GetInfoFromResponse(resp, out rateLimit, out limitRemaining);
                using (XmlReader reader = XmlReader.Create(resp.GetResponseStream()))
                {
                    return XDocument.Load(reader);
                }
            }
            catch (WebException wex)
            {
                LogEvents.InvokeOnError(new TwitterArgs(string.Format("Twitter rate limit exceeded, max of {0}/hr allowed. Remaining = {1}", rateLimit, limitRemaining), wex.Message, wex.StackTrace));
                return null;
            }

            catch (Exception ex)
            {
                LogEvents.InvokeOnError(new TwitterArgs("Error downloading twitter status from " + url, ex.Message, ex.StackTrace));
                return null;
            }
        }
        internal static XDocument DownloadTwitterXml(string url)
        {
            try
            {
                return XDocument.Load(url);
            }
            catch(Exception ex)
            {
                LogEvents.InvokeOnError(new TwitterArgs("Error downloading twitter status from " + url, ex.Message, ex.StackTrace));
                return null;
            }
        }

        internal static void GetInfoFromResponse(WebResponse resp, out int rateLimit, out int limitRemaining)
        {
            rateLimit = 0;
            limitRemaining = 0;

            for (int i = 0; i < resp.Headers.Keys.Count; i++)
            {
                string s = resp.Headers.GetKey(i);
                if (s == "X-RateLimit-Limit")
                {
                    rateLimit = int.Parse(resp.Headers.GetValues(i).First());
                }
                if (s == "X-RateLimit-Remaining")
                {
                    limitRemaining = int.Parse(resp.Headers.GetValues(i).First());
                }
            }
        }


        private static Image DownloadImage(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    WebRequest requestPic = WebRequest.Create(new Uri(url));

                    WebResponse responsePic = requestPic.GetResponse();
                    Image rImage = Image.FromStream(responsePic.GetResponseStream());

                    return rImage;
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnWarning(new TwitterArgs("Error downloading image from " + url, ex.Message, ex.StackTrace));
                }
            }
            return null;
        }
        private static bool IsUserPictureCached(string user, string cacheFolder, out string cachedImagePath)
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Checking if user picture is cached"));
            string md5hash = GenerateMd5Hash(user);
            if(cacheFolder.EndsWith(@"\")) cachedImagePath = cacheFolder + md5hash + ".png";
            else cachedImagePath = cacheFolder + @"\" + md5hash + ".png";
            return File.Exists(cachedImagePath);
        }

        private static TwitterUser GetUserPicture(TwitterUser user)
        {
            if (user != null)
            {
                LogEvents.InvokeOnDebug(new TwitterArgs("Downloading user picture for user \"" + user.ScreenName + "\" from url " + user.PicturePath));
                try
                {
                    user.Picture = DownloadImage(user.PicturePath);
                    user.PicturePath = string.Empty;
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnWarning(new TwitterArgs("Error downloading user picture from url " + user.PicturePath, ex.Message, ex.StackTrace));
                }
            }
            return user;
        }
        private static TwitterUser GetUserPicture(TwitterUser user, string cacheFolder)
        {
            if (user != null)
            {
                string imgpath;
                LogEvents.InvokeOnDebug(new TwitterArgs("Downloading user picture for user " + user.ScreenName));
                string name = user.ScreenName;
                if (IsUserPictureCached(name, cacheFolder, out imgpath))
                {
                    LogEvents.InvokeOnDebug(new TwitterArgs("User picture is cached. Try to load picture"));
                    try
                    {
                        user.Picture = Image.FromFile(imgpath);
                        user.PicturePath = imgpath;
                        LogEvents.InvokeOnDebug(new TwitterArgs("Loading user picture from cache path " + imgpath + " successful"));
                    }
                    catch (Exception ex)
                    {
                        user.PicturePath = string.Empty;
                        LogEvents.InvokeOnWarning(new TwitterArgs("Error loading user picture from cache " + imgpath, ex.Message, ex.StackTrace));
                    }
                }
                else
                {
                    LogEvents.InvokeOnDebug(new TwitterArgs("User picture is not cached. Downloading user picture from url " + user.PicturePath));
                    user.Picture = DownloadImage(user.PicturePath);
                    if (user.Picture != null)
                    {
                        string dirName = Path.GetDirectoryName(imgpath);
                        if (!Directory.Exists(dirName))
                        {
                            LogEvents.InvokeOnDebug(new TwitterArgs("Twitter folder doesn't exist. Creating a new one -> " + dirName));
                            try
                            {
                                Directory.CreateDirectory(dirName);
                            }
                            catch(Exception ex)
                            {
                                LogEvents.InvokeOnWarning(new TwitterArgs("Error saving user picture from url " + user.PicturePath + " to directory " + dirName + ". Could not create directory " + dirName + ".", ex.Message, ex.StackTrace));
                                user.PicturePath = string.Empty;
                            }

                        }
                        try
                        {
                            LogEvents.InvokeOnDebug(new TwitterArgs("Saving user picture to cache folder -> " + imgpath));
                            user.Picture.Save(imgpath, ImageFormat.Png);
                            user.PicturePath = imgpath;
                        }
                        catch (Exception ex)
                        {
                            LogEvents.InvokeOnWarning(new TwitterArgs("Error saving user picture from url " + user.PicturePath, ex.Message, ex.StackTrace));
                            user.PicturePath = string.Empty;
                        }
                    }
                    else user.PicturePath = string.Empty;
                }
            }
            return user;
        }

        public static List<TwitterItem> GetUserPictures(List<TwitterItem> items, string cacheFolder)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].User = GetUserPicture(items[i].User, cacheFolder);
                if (items[i].Retweet != null) items[i].Retweet.User = GetUserPicture(items[i].User, cacheFolder);
            }
            return items;
        }
        public static List<TwitterItem> GetUserPictures(List<TwitterItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].User = GetUserPicture(items[i].User);
                if (items[i].Retweet != null) items[i].Retweet.User = GetUserPicture(items[i].User);
            }
            return items;
        }

        internal static bool IsValidPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
                return r.IsMatch(path);
            }
            return false;
        }
        private static string GenerateMd5Hash(string input)
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Generating MD5 Hash for user " + input));
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            LogEvents.InvokeOnDebug(new TwitterArgs("MD5 Hash for user \"" + input + "\" is " + sBuilder));
            return sBuilder.ToString();
        }
        internal static string Clean(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string output = input.Trim();

                output = output.Replace("&#39;", "'");
                output = output.Replace("&iquest;", "¿");
                output = output.Replace("&Agrave;", "À");
                output = output.Replace("&Aacute;", "Á");
                output = output.Replace("&Acirc;", "Â");
                output = output.Replace("&Atilde;", "Ã");
                output = output.Replace("&Auml;", "Ä");
                output = output.Replace("&Aring;", "Å");
                output = output.Replace("&AElig;", "Æ");
                output = output.Replace("&Ccedil;", "Ç");
                output = output.Replace("&Egrave;", "È");
                output = output.Replace("&Eacute;", "É");
                output = output.Replace("&Ecirc;", "Ê");
                output = output.Replace("&Euml;", "Ë");
                output = output.Replace("&Igrave;", "Ì");
                output = output.Replace("&Iacute;", "Í");
                output = output.Replace("&Icirc;", "Î");
                output = output.Replace("&Iuml;", "Ï");
                output = output.Replace("&ETH;", "Ð");
                output = output.Replace("&Ntilde;", "Ñ");
                output = output.Replace("&Ograve;", "Ò");
                output = output.Replace("&Oacute;", "Ó");
                output = output.Replace("&Ocirc;", "Ô");
                output = output.Replace("&Otilde;", "Õ");
                output = output.Replace("&Ouml;", "Ö");
                output = output.Replace("&times;", "×");
                output = output.Replace("&Oslash;", "Ø");
                output = output.Replace("&Ugrave;", "Ù");
                output = output.Replace("&Uacute;", "Ú");
                output = output.Replace("&Ucirc;", "Û");
                output = output.Replace("&Uuml;", "Ü");
                output = output.Replace("&Yacute;", "Ý");
                output = output.Replace("&THORN;", "Þ");
                output = output.Replace("&szlig;", "ß");
                output = output.Replace("&agrave;", "à");
                output = output.Replace("&aacute;", "á");
                output = output.Replace("&acirc;", "â");
                output = output.Replace("&atilde;", "ã");
                output = output.Replace("&auml;", "ä");
                output = output.Replace("&aring;", "å");
                output = output.Replace("&aelig;", "æ");
                output = output.Replace("&ccedil;", "ç");
                output = output.Replace("&egrave;", "è");
                output = output.Replace("&eacute;", "é");
                output = output.Replace("&ecirc;", "ê");
                output = output.Replace("&euml;", "ë");
                output = output.Replace("&igrave;", "ì");
                output = output.Replace("&iacute;", "í");
                output = output.Replace("&icirc;", "î");
                output = output.Replace("&iuml;", "ï");
                output = output.Replace("&eth;", "ð");
                output = output.Replace("&ntilde;", "ñ");
                output = output.Replace("&ograve;", "ò");
                output = output.Replace("&oacute;", "ó");
                output = output.Replace("&ocirc;", "ô");
                output = output.Replace("&otilde;", "õ");
                output = output.Replace("&ouml;", "ö");
                output = output.Replace("&divide;", "÷");
                output = output.Replace("&oslash;", "ø");
                output = output.Replace("&ugrave;", "ù");
                output = output.Replace("&uacute;", "ú");
                output = output.Replace("&ucirc;", "û");
                output = output.Replace("&uuml;", "ü");
                output = output.Replace("&yacute;", "ý");
                output = output.Replace("&thorn;", "þ");
                output = output.Replace("&yuml;", "ÿ");
                output = output.Replace("&ndash;", "–");
                output = output.Replace("&mdash;", "—");
                output = output.Replace("&nbsp;", " ");
                output = output.Replace("&iexcl;", "¡");
                output = output.Replace("&cent;", "¢");
                output = output.Replace("&pound;", "£");
                output = output.Replace("&curren;", "¤");
                output = output.Replace("&yen;", "¥");
                output = output.Replace("&brvbar;", "¦");
                output = output.Replace("&brkbar;", "¦");
                output = output.Replace("&sect;", "§");
                output = output.Replace("&uml;", "¨");
                output = output.Replace("&die;", "¨");
                output = output.Replace("&copy;", "©");
                output = output.Replace("&ordf;", "ª");
                output = output.Replace("&laquo;", "«");
                output = output.Replace("&not;", "¬");
                output = output.Replace("&shy;", "­");
                output = output.Replace("&reg;", "®");
                output = output.Replace("&hibar;", "¯");
                output = output.Replace("&macr;", "¯");
                output = output.Replace("&deg;", "°");
                output = output.Replace("&plusmn;", "±");
                output = output.Replace("&sup2;", "²");
                output = output.Replace("&sup3;", "³");
                output = output.Replace("&acute;", "´");
                output = output.Replace("&micro;", "µ");
                output = output.Replace("&para;", "¶");
                output = output.Replace("&middot;", "·");
                output = output.Replace("&cedil;", "¸");
                output = output.Replace("&sup1;", "¹");
                output = output.Replace("&ordm;", "º");
                output = output.Replace("&raquo;", "»");
                output = output.Replace("&frac14;", "¼");
                output = output.Replace("&frac12;", "½");
                output = output.Replace("&frac34;", "¾");
                output = output.Replace("&lsquo;", "‘");
                output = output.Replace("&rsquo;", "’");
                output = output.Replace("&sbquo;", "‚");
                output = output.Replace("&ldquo;", "“");
                output = output.Replace("&rdquo;", "”");
                output = output.Replace("&bdquo;", "„");
                output = output.Replace("&dagger;", "†");
                output = output.Replace("&Dagger;", "‡");
                output = output.Replace("&permil;", "‰");
                output = output.Replace("&lsaquo;", "‹");
                output = output.Replace("&rsaquo;", "›");
                output = output.Replace("&spades;", "?");
                output = output.Replace("&clubs;", "?");
                output = output.Replace("&hearts;", "?");
                output = output.Replace("&diams;", "?");
                output = output.Replace("&oline;", "?");
                output = output.Replace("&larr;", "?");
                output = output.Replace("&uarr;", "?");
                output = output.Replace("&rarr;", "?");
                output = output.Replace("&darr;", "?");
                output = output.Replace("&trade;", "™");
                output = output.Replace("&quot;", "");
                output = output.Replace("&amp;", "&");
                output = output.Replace("&frasl;", "/");
                output = output.Replace("&lt;", "<");
                output = output.Replace("&gt;", ">");

                Regex r = new Regex("<.+?>", RegexOptions.Singleline);

                return r.Replace(output, "");
            }
            return input;
        }
    }
}
