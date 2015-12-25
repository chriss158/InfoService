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
using System.Xml.Linq;
using FeedReader.Data;
using FeedReader.Enums;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Net.Sockets;
#endregion

namespace FeedReader
{
    internal static class Utils
    {

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        internal static extern bool PathIsUNC([MarshalAsAttribute(UnmanagedType.LPWStr), In] string pszPath);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WNetGetConnection(
            [MarshalAs(UnmanagedType.LPTStr)] string localName,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
            ref int length);

        internal static Image DownloadImage(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    WebRequest requestPic = WebRequest.Create(new Uri(url));
                    requestPic.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
                    using (WebResponse responsePic = requestPic.GetResponse())
                    {
                        Image rImage = Image.FromStream(responsePic.GetResponseStream());
                        LogEvents.InvokeOnDebug(new FeedArgs("Downloading image from url " + url + " successfull"));
                        return rImage;
                    }
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnWarning(new FeedArgs("Error downloading image from " + url, ex.Message, ex.StackTrace));
                }
            }
            return null;
        }

        internal static string RemoveHTMLTags(string input)
        {
            Regex r = new Regex("<.+?>", RegexOptions.Singleline);

            return r.Replace(input, "");
        }
        internal static string ReplaceHTMLSpecialChars(string input)
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
                return output;
            }
            return input;
        }

        internal static string Clean(string input)
        {
            return Clean(input, false, false, false, true, new List<FeedItemFilter>());
        }


        internal static string Clean(string input, bool useFeedFiltersTitle, bool useFeedFiltersDesc, bool useCleanBefore, bool cleanHTML, List<FeedItemFilter> itemFilters)
        {
            string output = input;

            if (cleanHTML) output = ReplaceHTMLSpecialChars(input);

            if (useCleanBefore && useFeedFiltersTitle)
                output = CleanWithFeedFilters(output, FeedPartClean.Title, FeedWhenClean.Before, itemFilters);
            else if (useCleanBefore && useFeedFiltersDesc)
                output = CleanWithFeedFilters(output, FeedPartClean.Desc, FeedWhenClean.Before, itemFilters);

            if (cleanHTML)
            {
                output = RemoveHTMLTags(output);
                output = output.Trim();
            }

            if (useFeedFiltersTitle)
                output = CleanWithFeedFilters(output, FeedPartClean.Title, FeedWhenClean.After, itemFilters);
            else if (useFeedFiltersDesc)
                output = CleanWithFeedFilters(output, FeedPartClean.Desc, FeedWhenClean.After, itemFilters);
            return output;
        }





        private static string CleanWithFeedFilters(string input, FeedPartClean what, FeedWhenClean when, List<FeedItemFilter> itemFilters)
        {
            string output = input;

            if (itemFilters == null || itemFilters.Count < 1) return output;

            foreach (FeedItemFilter filter in itemFilters)
            {
                if (string.IsNullOrEmpty(filter.ReplaceThis)) continue;
                if (what == FeedPartClean.Title && !filter.UseInTitle) continue;
                if (what == FeedPartClean.Desc && !filter.UseInBody) continue;
                if (when == FeedWhenClean.Before && !filter.CleanBefore) continue;
                if (when == FeedWhenClean.After && filter.CleanBefore) continue;

                if (what == FeedPartClean.Title)
                    LogEvents.InvokeOnDebug(new FeedArgs(string.Format("Using feed item filter in title: {0} to {1}!", filter.ReplaceThis, filter.ReplaceWith)));
                else if (what == FeedPartClean.Desc)
                    LogEvents.InvokeOnDebug(new FeedArgs(string.Format("Using feed item filter in description: {0} to {1}!", filter.ReplaceThis, filter.ReplaceWith)));

                if (filter.IsRegEx)
                {
                    Regex regEx = new Regex(filter.ReplaceThis, RegexOptions.Compiled);
                    foreach (Match m in regEx.Matches(output))
                        output = output.Replace(m.Value, filter.ReplaceWith);
                }
                else
                {
                    output = output.Replace(filter.ReplaceThis, filter.ReplaceWith);
                }
            }

            return output;
        }

        internal static string GetCdata(string cdata)
        {
            if (!string.IsNullOrEmpty(cdata))
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Getting CDATA"));
                Regex r = new Regex(@"(?<=<!\[CDATA\[).*?(?=\]\])", RegexOptions.Singleline);
                return r.Replace(cdata, "");
            }
            return cdata;
        }

        internal static string GetImageOutOfDescription(string htmltag)
        {
            //Regex regex = new Regex(@"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            //Regex regex = new Regex(@"<?img\s*src\s*\=\s*\""?(?<imgURI>[^\""]+?(?<imgExtension>(?:jp[e]?g|bmp|gif|png)).*?)\"".+?(?:height\=\""?(?<imgHeight>\d+)\""?).+?(?:width\=\""?(?<imgWidth>\d+)\""?)[^>]*?>");
            Regex regex = new Regex(@"(?i-)<img.+?src\s*\=\s*(?:\""|\')?(?<imgURI>[^(?:\""|\')].*?(?<imgExtension>(?:jp[e]?g|bmp|gif|png)).*?)(?:\""|\').+?(?:height\=(?:\""|\')?(?<imgHeight>\d+)(?:\""|\')?)?.+?(?:width\=(?:\""|\')?(?<imgWidth>\d+)(?:\""|\')?)?[^>]*?>");

            foreach (Match Tag in regex.Matches(htmltag))
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Found a image tag in the description field -> " + Tag.Value + ". Parse img tag..."));

                string attrSrc = Tag.Groups["imgURI"].Value;
                string attrHeight = Tag.Groups["imgHeight"].Value;
                string attrWidth = Tag.Groups["imgWidth"].Value;

                if (string.IsNullOrEmpty(attrSrc))
                {
                    LogEvents.InvokeOnDebug(new FeedArgs("Image tag has no src attributes. Try to parse another img tag in the description field...."));
                    continue;
                }

                if (string.IsNullOrEmpty(attrWidth) || string.IsNullOrEmpty(attrHeight))
                {
                    LogEvents.InvokeOnDebug(new FeedArgs("Parsing of image tag succesful. Found a proper src link -> " + attrSrc));
                    return attrSrc;
                }

                try
                {
                    if (Convert.ToInt32(attrWidth) >= 10 && Convert.ToInt32(attrHeight) >= 10)
                    {
                        LogEvents.InvokeOnDebug(new FeedArgs("Parsing of image tag succesful. Found a proper src link -> " + attrSrc));
                        return attrSrc;
                    }
                    else
                    {
                        LogEvents.InvokeOnDebug(new FeedArgs("Image width or image height is too small. Try to parse another img tag in the description field...."));
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnDebug(new FeedArgs("The Image tag is no proper tag. Try to parse another img tag in the description field....", ex.Message, ex.StackTrace));
                    continue;
                }

                /*
                string link = string.Empty;
                string tag = Tag.Value;
                LogEvents.InvokeOnDebug(new FeedArgs("Found a image tag in the description field -> " + tag + ". Parse img tag..."));
                if (!tag.EndsWith("/>") && !tag.EndsWith("</img>"))
                {
                    tag += "</img>";
                    LogEvents.InvokeOnDebug(new FeedArgs("Image tag has no proper end tag. Adding </img> end tag..."));
                }
                XElement ele;
                try
                {
                    ele = XElement.Parse(tag);
                    LogEvents.InvokeOnDebug(new FeedArgs("The image tag is a proper tag. Now parsing src, width and height of image tag..."));
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnDebug(new FeedArgs("The Image tag is no proper tag. Try to parse another img tag in the description field....", ex.Message, ex.StackTrace));
                    continue;
                }

                if (ele.HasAttributes)
                {
                    var attrSrc = ele.Attribute("src");
                    var attrWidth = ele.Attribute("width");
                    var attrHeight = ele.Attribute("height");
                    if (attrSrc != null)
                    {
                        if (!string.IsNullOrEmpty(attrSrc.Value) && attrSrc.Value.Length<260 &&
                            (Path.GetExtension(attrSrc.Value).ToLowerInvariant() == ".gif" || Path.GetExtension(attrSrc.Value).ToLowerInvariant() == ".bmp" ||
                             Path.GetExtension(attrSrc.Value).ToLowerInvariant() == ".jpg" || Path.GetExtension(attrSrc.Value).ToLowerInvariant() == ".png" ||
                             Path.GetExtension(attrSrc.Value).ToLowerInvariant() == ".jpeg")
                            )
                        {
                            if (attrWidth != null && attrHeight != null)
                            {
                                try
                                {
                                    if (Convert.ToInt32(attrWidth.Value) >= 10 && Convert.ToInt32(attrHeight.Value) >= 10)
                                    {
                                        LogEvents.InvokeOnDebug(new FeedArgs("Parsing of image tag succesful. Found a proper src link -> " + attrSrc.Value));
                                        return attrSrc.Value;
                                    }
                                    else
                                    {
                                        LogEvents.InvokeOnDebug(new FeedArgs("Image width and image height is too small. Try to parse another img tag in the description field...."));
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogEvents.InvokeOnDebug(new FeedArgs("The Image tag is no proper tag. Try to parse another img tag in the description field....", ex.Message, ex.StackTrace));
                                    continue;
                                }

                            }
                            else
                            {
                                LogEvents.InvokeOnDebug(new FeedArgs("Parsing of image tag succesful. Found a proper src link -> " + attrSrc.Value));
                                return attrSrc.Value;
                            }

                        }
                        else
                        {
                            LogEvents.InvokeOnDebug(new FeedArgs("Image src has no valid image extension. Try to parse another img tag in the description field...."));
                            continue;
                        }
                    }
                    else
                    {
                        LogEvents.InvokeOnDebug(new FeedArgs("Image tag has no src attribute. Try to parse another img tag in the description field...."));
                        continue;
                    }
                }
                else
                {
                    LogEvents.InvokeOnDebug(new FeedArgs("Image tag has no attributes. Try to parse another img tag in the description field...."));
                    continue;
                }
                */
            }
            LogEvents.InvokeOnDebug(new FeedArgs("Didn't found an image in the description. Default feed item image will be used..."));
            return "";
        }

        private static bool IsFeedImageCached(string title, string cacheFolder, out string cachedImagePath)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Checking if feed image is cached"));

            //Short Title
            string sTitle = title;
            if (sTitle.Length > 30) sTitle = sTitle.Substring(0, 30);

            //Remove illegal chars from title
            sTitle = Path.GetInvalidFileNameChars().Aggregate(sTitle, (current, lDisallowed) => current.Replace(lDisallowed.ToString(), ""));
            sTitle = Path.GetInvalidPathChars().Aggregate(sTitle, (current, lDisallowed) => current.Replace(lDisallowed.ToString(), ""));
            sTitle = sTitle.Trim();
            cachedImagePath = cacheFolder + sTitle + @"\feedImage.png";
            return File.Exists(cacheFolder + sTitle + @"\feedImage.png");
        }

        private static bool IsFeedItemImageCached(string title, string itemTitle, string cacheFolder,
                                                 out string cachedImagePath)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Checking if feed item image is cached"));
            string md5hash = GenerateMd5Hash(itemTitle);

            //Short Title
            string sTitle = title;
            if (sTitle.Length > 30) sTitle = sTitle.Substring(0, 30);

            //Remove illegal chars from title
            sTitle = Path.GetInvalidFileNameChars().Aggregate(sTitle, (current, lDisallowed) => current.Replace(lDisallowed.ToString(), ""));
            sTitle = Path.GetInvalidPathChars().Aggregate(sTitle, (current, lDisallowed) => current.Replace(lDisallowed.ToString(), ""));
            sTitle = sTitle.Trim();
            if (cacheFolder.EndsWith(@"\")) cachedImagePath = cacheFolder + sTitle + @"\" + md5hash + ".png";
            else cachedImagePath = cacheFolder + @"\" + sTitle + @"\" + md5hash + ".png";
            return File.Exists(cacheFolder + sTitle + @"\" + md5hash + ".png");
        }

        private static string GenerateMd5Hash(string input)
        {
            byte[] data = null;
            LogEvents.InvokeOnDebug(new FeedArgs("Generating MD5 Hash out of \"" + input + "\""));
            using (MD5 md5Hasher = MD5.Create())
            {
                data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            }

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Clear();
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            LogEvents.InvokeOnDebug(new FeedArgs("MD5 Hash out of \"" + input + "\" is " + sBuilder));
            return sBuilder.ToString();
        }
        internal static bool LoadFeedImage(string url, string feedTitle, out Image feedImage)
        {

            LogEvents.InvokeOnDebug(new FeedArgs("Downloading feed[" + feedTitle + "] image"));
            feedImage = DownloadImage(url);
            if (feedImage != null) return true;
            else return false;

        }

        internal static bool LoadFeedItemImage(string url, string feedTitle, string feedItemTitle, out Image feedImage)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Downloading feed[" + feedTitle + "] item[" + feedItemTitle + "] image from url " + url));
            feedImage = DownloadImage(url);
            if (feedImage != null) return true;
            else return false;
        }
        private static bool LoadImage(string url, string feedTitle, string feedItemTitle, string cacheFolder, bool isItemImage, out Image feedImage, out string feedImagePath)
        {
            string imgpath;
            bool returnValue = false;
            bool errorCacheLoading = false;
            bool imageCached = false;

            feedImage = null;
            feedImagePath = string.Empty;

            if (isItemImage) imageCached = IsFeedItemImageCached(feedTitle, feedItemTitle, cacheFolder, out imgpath);
            else imageCached = IsFeedImageCached(feedTitle, cacheFolder, out imgpath);

            if (imageCached)
            {
                if (isItemImage) LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] item[" + feedItemTitle + "] image is cached. Try to load image"));
                else LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] image is cached. Try to load image"));
                try
                {
                    using (FileStream fs = new FileStream(imgpath, FileMode.Open, FileAccess.Read))
                    {
                        using (Image cacheImage = Image.FromStream(fs))
                        {
                            feedImage = cacheImage.Clone() as Image;
                        }
                    }
                    feedImagePath = imgpath;
                    returnValue = true;
                    if (isItemImage) LogEvents.InvokeOnDebug(new FeedArgs("Loading feed[" + feedTitle + "] item[" + feedItemTitle + "] image from cache path " + imgpath + " succesfull"));
                    else LogEvents.InvokeOnDebug(new FeedArgs("Loading feed[" + feedTitle + "] image from cache path " + imgpath + " succesfull"));
                }
                catch (Exception ex)
                {
                    feedImage = null;
                    feedImagePath = string.Empty;
                    errorCacheLoading = true;
                    if (isItemImage) LogEvents.InvokeOnError(new FeedArgs("Error loading feed[" + feedTitle + "] item[" + feedItemTitle + "] image from cache " + imgpath + " . Delete the cache and try again", ex.Message, ex.StackTrace));
                    else LogEvents.InvokeOnError(new FeedArgs("Error loading feed[" + feedTitle + "] image from cache " + imgpath + " . Delete the cache and try again", ex.Message, ex.StackTrace));
                }
            }
            if (!imageCached || errorCacheLoading)
            {
                if (isItemImage)
                {
                    if (errorCacheLoading) LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] item[" + feedItemTitle + "] image could not be loaded from cache. Downloading again..."));
                    else LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] item[" + feedItemTitle + "] image is not cached."));
                }
                else
                {
                    if (errorCacheLoading) LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] image could not be loaded from cache. Downloading again..."));
                    else LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] image is not cached."));
                }
                feedImage = DownloadImage(url);
                if (feedImage != null)
                {
                    string dirName = Path.GetDirectoryName(imgpath);
                    if (!Directory.Exists(dirName))
                    {
                        LogEvents.InvokeOnDebug(new FeedArgs("Feed folder doesn't exist. Creating a new one -> " + dirName));
                        try
                        {
                            Directory.CreateDirectory(dirName);
                        }
                        catch (System.Exception ex)
                        {
                            feedImage = null;
                            feedImagePath = string.Empty;
                            returnValue = false;
                            LogEvents.InvokeOnError(new FeedArgs("Error saving feed item image from " + imgpath + " into cache folder " + dirName + ". Could not create directory " + dirName + ". Delete the cache and try again", ex.Message, ex.StackTrace));
                        }

                    }
                    try
                    {

                        if (feedImage.Size.Height >= 5 && feedImage.Size.Width >= 5)
                        {
                            if (isItemImage) LogEvents.InvokeOnDebug(new FeedArgs("Saving feed[" + feedTitle + "] item[" + feedItemTitle + "] image to cache folder -> " + imgpath));
                            else LogEvents.InvokeOnDebug(new FeedArgs("Saving feed[" + feedTitle + "] image to cache folder -> " + imgpath));
                            using (Image cloneImage = feedImage.Clone() as Image)
                            {
                                cloneImage.Save(imgpath, ImageFormat.Png);
                            }
                            feedImagePath = imgpath;
                            returnValue = true;
                        }
                        else
                        {
                            if (isItemImage) LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] item[" + feedItemTitle + "] image is too small. Looks like this is not a valid feed image. Delete image..."));
                            else LogEvents.InvokeOnDebug(new FeedArgs("Feed[" + feedTitle + "] image is too small. Looks like this is not a valid feed image. Delete image..."));
                            feedImage = null;
                            feedImagePath = string.Empty;
                            returnValue = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        feedImage = null;
                        feedImagePath = string.Empty;
                        returnValue = false;
                        LogEvents.InvokeOnError(new FeedArgs("Error saving feed item image from " + imgpath + " . Delete the cache and try again", ex.Message, ex.StackTrace));
                    }
                }
                else
                {
                    feedImage = null;
                    feedImagePath = string.Empty;
                    returnValue = false;
                }
            }
            return returnValue;
        }
        internal static bool LoadFeedImage(string url, string feedTitle, string cacheFolder, out Image feedImage, out string feedImagePath)
        {
            return LoadImage(url, feedTitle, string.Empty, cacheFolder, false, out feedImage, out feedImagePath);
        }
        internal static bool LoadFeedItemImage(string url, string feedTitle, string feedItemTitle, string cacheFolder, out Image feedImage, out string feedImagePath)
        {
            return LoadImage(url, feedTitle, feedItemTitle, cacheFolder, true, out feedImage, out feedImagePath);
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
        internal static string GetUncPath(string originalPath)
        {

            StringBuilder sb = new StringBuilder(512);
            int size = sb.Capacity;

            // look for the {LETTER}: combination ...
            if (!IsUncPath(originalPath))
            //if (originalPath.Length > 2 && originalPath[1] == ':')
            {
                LogEvents.InvokeOnInfo(new FeedArgs("Convert path " + originalPath + " to UNC path..."));
                // don't use char.IsLetter here - as that can be misleading
                // the only valid drive letters are a-z && A-Z.
                char c = originalPath[0];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    int error = WNetGetConnection(originalPath.Substring(0, 2),
                        sb, ref size);
                    if (error == 0)
                    {
                        DirectoryInfo dir = new DirectoryInfo(originalPath);

                        string path = Path.GetFullPath(originalPath)
                            .Substring(Path.GetPathRoot(originalPath).Length);
                        string newPath = Path.Combine(sb.ToString().TrimEnd(), path);
                        LogEvents.InvokeOnInfo(new FeedArgs("Converted UNC path is " + newPath + "."));
                        return newPath;
                    }
                    else
                    {
                        LogEvents.InvokeOnError(new FeedArgs("Error converting path " + originalPath + " to UNC path. Error code -> " + error));
                    }
                }
            }

            return originalPath;
        }


        internal static bool IsUncPath(string path)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Checking if folder " + path + " is a UNC path..."));
            bool isUncPath = PathIsUNC(path);
            if (isUncPath)
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Folder " + path + " is a UNC path."));
                return true;
            }
            else
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Folder " + path + " is NOT a UNC path."));
                return false;
            }
        }
        internal static bool IsNetworkPath(string path)
        {
            if (IsUncPath(path))
            {
                return true;
            }

            if (IsNetworkDrive(path))
            {
                return true;
            }

            return false;
        }

        internal static bool IsNetworkDrive(string path)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Checking if folder " + path + " is a network drive path..."));
            string networkPath = Path.GetPathRoot(path);
            DriveInfo drive = new DriveInfo(networkPath);
            if (drive.DriveType == DriveType.Network)
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Folder " + path + " is a network drive path."));
                return true;
            }
            else
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Folder " + path + " is NOT a network path."));
                return false;
            }
        }
        internal static bool IsCacheFolderAvailable(string path)
        {
            string serverName = string.Empty;
            LogEvents.InvokeOnInfo(new FeedArgs("Checking if cache folder " + path + " is available..."));
            if (IsNetworkPath(path))
            {
                string uncPath = GetUncPath(path);

                LogEvents.InvokeOnInfo(new FeedArgs("Getting server name from unc path " + uncPath + "..."));
                serverName = uncPath.Trim(@"\".ToCharArray());
                if (serverName.Contains(@"\")) serverName = serverName.Remove(serverName.IndexOf(@"\", StringComparison.Ordinal));
                LogEvents.InvokeOnInfo(new FeedArgs("Server name is \"" + serverName + "\""));
                LogEvents.InvokeOnInfo(new FeedArgs("Checking server \"" + serverName + "\" is available..."));
                try
                {
                    LogEvents.InvokeOnInfo(new FeedArgs("Checking DNS entry for server \"" + serverName + "\"..."));
                    if (Dns.GetHostEntry(serverName) == null)
                    {
                        LogEvents.InvokeOnError(new FeedArgs("Could not resolve DNS entry for server \"" + serverName + "\". Server is NOT available."));
                        return false;
                    }
                }
                catch (SocketException se)
                {
                    LogEvents.InvokeOnError(new FeedArgs("Could not resolve DNS entry for server \"" + serverName + "\". Server is NOT available.", se.Message, se.StackTrace));
                    return false;
                }

                if (!IsServerShareReachable(serverName, 10000))
                {
                    LogEvents.InvokeOnError(new FeedArgs("Server \"" + serverName + "\" is NOT available."));
                    return false;
                }


                LogEvents.InvokeOnInfo(new FeedArgs("Server \"" + serverName + "\" is available."));
                return true;
            }
            else return true;
        }


        public static bool IsServerShareReachable(string host, int timeout)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Checking if shares of \"" + host + "\" are reachable..."));
            const int port = 139; //SMB Port

            bool IsOnline = false;
            try
            {
                IPAddress[] adresses = Dns.GetHostAddresses(host);
                if (adresses.Length > 0)
                {
                    IPAddress address = IPAddress.Parse(adresses[0].ToString());
                    using (TcpClient tcpClient = new TcpClient(address.AddressFamily))
                    {
                        tcpClient.NoDelay = true;
                        IAsyncResult connection = tcpClient.BeginConnect(address, port, null, null);

                        bool result = connection.AsyncWaitHandle.WaitOne(timeout, false);

                        if (connection.IsCompleted && result)
                        {
                            LogEvents.InvokeOnDebug(new FeedArgs("Server shares of \"" + host + "\" are reachable."));
                            tcpClient.EndConnect(connection);
                            IsOnline = true;
                        }
                        else
                        {
                            LogEvents.InvokeOnDebug(new FeedArgs("Server shares of \"" + host + "\" are NOT reachable."));
                        }
                        tcpClient.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Server shares of \"" + host + "\" are NOT reachable.", ex.Message, ex.StackTrace));
            }
            return IsOnline;
        }



        internal static bool DoesCacheFolderExists(string path)
        {
            LogEvents.InvokeOnInfo(new FeedArgs("Checking if cache folder \"" + path + "\" exists..."));
            if (Directory.Exists(path))
            {
                LogEvents.InvokeOnInfo(new FeedArgs("Cache folder \"" + path + "\" does exists."));
                return true;
            }
            else
            {
                LogEvents.InvokeOnError(new FeedArgs("Cache folder \"" + path + "\" does NOT exists."));
                return false;
            }
        }
        internal static bool IsValidUrl(ref string url)
        {
            Uri newUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out newUri))
            {
                url = newUri.AbsoluteUri;
                return true;
            }
            else
            {
                return false;
            }
        }
        internal static bool IsValidUrl(string url)
        {
            string checkUrl = url;
            return IsValidUrl(ref checkUrl);
        }
        internal static bool IsEnclosureTypeImage(string type)
        {
            return type == "image/gif" ||
                   type == "image/jpeg" ||
                   type == "image/png";
        }
        internal static Dictionary<string, XNamespace> GetNamespacesFromXml(XDocument xml)
        {
            return xml.Root.Attributes().
                    Where(a => a.IsNamespaceDeclaration).
                    GroupBy(a => a.Name.Namespace == XNamespace.None ? String.Empty : a.Name.LocalName,
                            a => XNamespace.Get(a.Value)).
                    ToDictionary(g => g.Key,
                                 g => g.First());
        }
        internal static XDocument RemoveNamespace(XDocument xdoc)
        {
            foreach (XElement e in xdoc.Root.DescendantsAndSelf())
            {
                if (e.Name.Namespace != XNamespace.None)
                {
                    e.Name = XNamespace.None.GetName(e.Name.LocalName);
                }
                if (e.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
                {
                    e.ReplaceAttributes(e.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));
                }
            }
            return xdoc;
        }
    }
}