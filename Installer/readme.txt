If you never used InfoService please read carefully the documentation: https://github.com/hasenbolle/InfoService/wiki

There you will find some useful informations about the plugin configuration. Especially MediaPortal skin developers need to look into this wiki. They will find some informations about the used property names and hyperlinks to the GUI screen,

If you have some problems with the configuration part of the plugin, then click on the help button in the upper right corner of the configuration dialog. 


Changelog:

[-] - Fixed issue
[*] - Changed feature
[+] - Added feature
[i] - Information
[!] - Attention

Version v1.83:
[+] Added load paramaters (feedIndex, feedTitle, feedGuid, feedItemIndex, twitterTimeline, twitterId, twitterItemIndex). More about that on InfoService Github Wiki.
[+] You can go now directly to twitter/feed if you click the "Ok" button on your remote if the News/New Tweets popup is display
[+] Added developer mode. More about that on InfoService Github Wiki.
[*] Improved News/New Tweets popup (New dialog, better to read). Thanks to wizard123 
[*] Include skin files for Titan and TITANIUS
[-] Fixed #infoservice.feed.img is not set everytime
[-] Fixed problems of two many new feed messages on some feeds
[-] Fixed wrong error "Empty consumer key/secret" when pressing "Get PIN!" in the twitter config

Version v1.82:
[*] Updated Spanish translation
[-] Fixed wrong help text (%show%) for update twitter status on movie/series watching

Version v1.81:
[-] Fixed crash on second twitter update

Version v1.8:
[+] Re-enabled Twitter
[+] New property for twitter: #infoservice.twitter.selected.mediaimage. It holds the first image which is attached to a tweet
[+] Added popup on new tweet (configurable with timeout)
[*] Memory footprint improvements
[-] All over bugfixes

Version v1.74:
[+] Proxy support
[+] Support for caching to a network drive/unc path
[+] Added Czech language
[-] Fixed some caching problems

Version v1.73:
[*] Disabled twitter client. Twitter changed the API. Sorry now time for a new twitter libary :(
[i] Compatible for MediaPortal 1.7 Pre-Release

Version v1.72:
[+] Added support for the NotificationBar 0.8.3.0 plugin
[*] Removed public timeline from twitter. Twitter doesn't support this anymore
[-] Fixed twitter client. Status update, authorization and all Timelines should now work aigain
[-] Some special characters will now displayed correctly again
[i] Compatible for MediaPortal 1.4 Pre-Release

Version v1.71:
[+] Added support for the NotificationBar (http://forum.team-mediaportal.com/threads/notificationbar.99207/) plugin
[+] Added Greek, Polish and Norwegian language
[!] There are still some untranslated parts :/
[i] If the NotificationBar plugin and the NotificationBar skin file are not installed, the standard MP Dialog will be used for new feed popup

Version v1.7:
[*] Disabled weather service until i found a new weather provider
[-] Wrong behavior when using a custom MediaPortal configuration (thx to migue333)
[-] Fixed for the most circumstances the stuttering feed/twitter ticker (thx to Pirppuli)
[i] Compatible for MediaPortal 1.3 Alpha and above

Version v1.66:
[+] Added popup on news update (configurable with timeout)
[*] Disabled recently added/watched features
[*] Improved image parsing from feed (more feed images for RSS feeds should be parsed, too small images will be dropped)
[*] Made log better readable
[*] Improved support for RDF feeds
[*] Improved downloading of Twitter profile images
[*] Updated support for WebBrowser plugin
[-] Fixed image loading from cache (if fails, will be redownloaded)
[-] Fixed encoding issue for feed text
[-] Fixed cache deletion
[-] Fixed couple of bugs and updated localization
[-] Fixed wrong epsiode number when posting a Twitter message
[i] Release for MediaPortal 1.2.0 and above
[!] Many languages files are not updated yet for v1.66 (only German and English). If you have some with untranslated labels please have a look in your language file

Version v1.62:
[-] Fixed weather crash bug

Version v1.61:
[+] New Properties: #infoservice.feed.itemtype and #infoservice.twitter.itemtype to replace hard coded labels in the skin (Feed items and Twitter items)
[+] Added new placeholders for ticker (%itemindex%, %itemsource%)
[+] Added setting of properties #infoservice.feed.INDEX.titles and #infoservice.feed.INDEX.img where INDEX is feed index (0 = all feeds, 1 = first one, 2 = ...) 
[+] Added new language: Portuguese (Brazilian) 
[+] Options can now be used with keyboard shortcuts (ALT + underlined letter).
[+] Added feed item filtering
[+] Added separate tickers masks and separators for normal feeds and all feeds
[+] "Browse The Web" webbrowser is now supported
[*] Improved loading of localization files
[*] Better logging of parsing errors
[*] Improved image parsing from feed
[*] RecentlyAdded items now have age, will be removed after 7 days
[*] Current thread is now logged, too
[-] Fixed threading problems when updating GUI components
[-] Fixed some problems with recently added
[-] Fixed some typos in recently added
[-] Fixed feed & twitter selection on GUI screens
[-] Fixed update problem when users changes weather location via weather screen
[!] Many languages files are not updated yet for v1.61 (only German and English). If you have some with untranslated labels please have a look in your language file

Version v1.6:
[+] Added Danish, French, English (US) and English (GB) language
[+] Added support for local feed files
[+] Added check if the added feed file / feed url is a valid feed before adding feed
[+] Added own configuration file
[+] Added Blue3 skin files
[+] Added property #infoservice.feed.selected.sourcefeed
[+] Added selection for second line of the "All Feeds" view. The user can now select to show feed publish time, the source feed name or both.
[+] The used Webbrowser plugin (WebBrowser, GeckoBrowser or other) can now be selected in the advanced configuration 
[+] Last selected feed/twitter item will be selected on open Twitter or Feeds page
[+] Improved tweeting of TV series/shows a lot!
[*] Changed Blue3Wide and Blue3 to show the feed image instead of feed item image 
[*] Reworked SkinSettings. SkinSettings will now overwrite user settings, but the will not saved permanently
[*] Cache folder of twitter and feed is now configurable in the advanced configuration
[*] Overridden settings by skin are marked in the configuration dialog now
[*] Duo MediaPortal code change in property system, changed some weather and Recently Added properties (See Skin changelog v1.5 to v1.6)
[*] Removed MaxTemp and MinTemp from weather today properties, because they are not right for some cities 
[*] Log file is now not locked while MediaPortal is running
[-] Fixed some logging issues
[-] Fixed recently added system
[-] Fixed some weather inconstancies.
[-] Fixed some rare crashes duo threading issues.
[-] Fixed false download of other files (only image files are now downloaded) by the Feed Service
[-] Fixed language misspellings for English and Italian
[i] Compiled against MediaPortal 1.1.0 RC4
[!] Duo the use of a own configuration file, you have to setup InfoService again, sorry

Version v1.5:
[+] Added recently added feature, which shows you the recently added movies and series
[+] Improved twitter client
[+] A brand new twitter screen with all twitter messages and timelines
[+] Post twitter status updates
[+] Open web links in a twitter message in a WebBrowser window
[+] Download all twitter timelines, not only one
[+] Post automatically twitter status update, if you watching a video
[+] Twitter uses now OAuth to connect
[+] Faster overall download times of feed and twitter
[+] Improved logging (for better error detection)
[+] Added multilanguage support (English, German, Italian, Dutch and Spanish for now)
[+] Weather is now updated as soon as you change your weather in the MediaPortal weather GUI
[+] Many new properties (See Skin changelog v1.32 to v1.5)
[+] Added skin settings (See Skin developer guide)
[-] A tons of bugs fixed, so many to count them all
[i] Completely rewritten!
[i] InfoService is now open source!

Version v1.32:
[-] Fixed empty entry in the normal home menu

Version v1.31:
[-] Fixed forgotten humidity property for each day (night and day)
[-] Fixed some forgotten translations

Version v1.3:
[+] Added a "last updated" property for feed, weather and twitter
[+] Added a button to download the default feed name on the Add Feed dialog
[+] Added much more weather properties. See thread/readme for details
[+] Added possibility to change the ticker layout
[*] Default settings are now loaded if the plugin is used for the first time (Feed ticker with MediaPortal RSS on, Weather on)
[-] Fixed no image download for atom feeds
[-] Fixed that the plugin is not showing on the normal home screen
[!] Removed Monochrome skin files

Version v1.2:
[+] Added option to disable/enable the feed item publish time
[-] Fixed the rare crash when downloading twitter timeline, hopefully

Version v1.1:
[+] New propertys #infoservice.feed.separator/ #infoservice.twitter.separator which holds the separator string of feed/twitter line
[-] False feed image is showed when "Show all feeds on home" is activated and after entering InfoService screen
[-] Fixed download location of feed images
[i] Duo the false download location of the feed images you can delete the folder "C:\Temp\InfoService\"
[!] The webbrowser part of InfoSerivce moved into a separate WebBrowser plugin. So if you want to read your feeds completely, you need the WebBrowser plugin which can be found here ...

Version v1.0:
[+] There are to much changes to list them Mainly bugfixes and a better browser handling. Try and test yourself.

Version v0.99.3:
[*] Changed zoom keys on remote and keyboard to Play previous/next key
[-] Fixed false sizing of browser window, if MediaPortal is not in fullscreen mode
[-] Fixed no weather download (Sorry for that)

Version v0.99.2:
[+] Added default zoom option for each feed
[+] Added option to change zoom steps
[*] Improved browser zooming feature
[-] Fixed false size of feed browser

Version v0.99.1:
[-] Fixed false resolving of #infoservice.weather.today/dayX.img.small/big.filenamewithoutext and changed name of #infoservice.weather.today/dayX.img.small/big.filename to #infoservice.weather.today/dayX.img.small/big.filenamewithext
[-] Fixed feed downloading problem with some feeds

Version v0.99:
[+] Added and changed propertys #infoservice.weather.today/dayX.img.big/small.fullpath, #infoservice.weather.today/dayX.img.big/small.filename, #infoservice.weather.today/dayX.img.big/small.filenamewithoutext
[+] Added possibility to read the whole feed in a browser window (incl. zoom)
[-] Fixed not showing of own feed image

Version v0.94:
[+] Readded the #infoservice.feed.alltitles property
[-] Removed forgotten debug code

Version v0.93:
[+] Added sorting for feed items, because some feeds are not sorted by date
[-] Fixed publish time was shown even if the feed item has no publish time

Version v0.92:
[+] Added feed item publish time to each feed item
[+] Added wait notification on manual update
[-] Fixed no update of weather and twitter data when pressing "Refresh" button
[-] Fixed no weather data for some timezones

Version v0.9:
[+] Added new button on InfoService window to show itmes of all feeds on basichome
[+] Added possibilty to change weather in MediaPortal (just change weather in the weather screen and wait for the next update)
[*] #infoservice.feed.itemimg will be empty if there is no feed item image found
[*] Old feed data is used if there was a download error
[*] Reverted back to use the MediaPortal weather configuration
[*] Removed property #infoservice.feed.alltitles
[-] Fixed bugs here and there
[-] Fixed filling of #infoservice.feed.selectedindex
[-] Fixed weather data is off by one day

Version v0.85:
[+] Added new feed configuration dialog
[+] Added sort feature for your feeds in the feed configuration dialog
[*] #infoservice.feed.itemimg filled with default image if no feed item imaged is found
[-] Fixed last selected feed is not active on basichome after update
[-] Fixed no refresh of feed items in the infoscreen window after feed update
[-] Fixed that #infoservice.today.weekday and #infoservice.day2.weekday shows the same weekday

Version v0.81:
[-] #infoservice.feed.selectedfeed will now be filled
[-] Fixed crash if infoservice thumb dir not exists

Version v0.8:
[+] Added automatic download of feed logo (if feed logo is found)
[+] Added check if entered feed is an url
[+] Added feed download error dialog on infoservice window
[+] Added new option to change the max items per feed if you use the #infoservice.feed.alltitles
[+] Added new propetry items #infoservice.feed.selectedindex, #infoservice.feed.selectedtitle, #infoservice.feed.itemcount, #infoservice.feed.alltitles, #infoservice.feed.type and #infoservice.feed.selectedfeed
[+] Added plugin configuration to log output (Not the twitter user and password!)
[+] Feed item images are now shown in the listcontrol
[*] Better error handling for weather service
[*] Changed all property names for clearer indentification
[*] Increased the maximum of items to 100 for feeds and twitter (don't know why i've locked this)
[-] Fixed crash when weather.com returns a error
[-] Fixed some html encoding erros

Version v0.71:
[*] Removed word wrap between the conditions
[-] Fixed forgotten localization of forecast conditions

Version v0.7:
[+] Added new property #feeditemimg that holds a image of a feed item
[+] Added own configuration for weather
[+] Added possibility to read feeds and download pictures for feed item (only rss 2.0)

Version v0.6:
[+] Added support for RDF (RSS 1.0) and Atom feeds
[*] Again better error and log handling
[*] Changed property name from #rssfeed -> #feedtitles, #rssimg -> #feedimg as the plugin now supports more than rss feeds
[*] Removed the last separator on the feed and twitter line.

Version v0.5:
[+] Added possibility to add more than one rss feed
[+] Added possibility to change the rss feed on basichome with a dirty trick (hope it work as expected) :/
[*] Changed error and log handling
[i] Rewrite of ca. 75% code for a easier way to add more services

Version v0.22:
[*] Day labels are now translated by MediaPortal

Version v0.21:
[-] Fixed #day4label resolving
[-] Fixed #todaylabel resolves the wrong day

Version v0.2:
[+] Added a twitter ticker
[+] Added day labes for each day

Version v0.12:
[-] Fixed crash if there is no internet connection

Version v0.11:
[-] Fixed crash if no location is found
[-] Fixed crash if timeformat in rss feed wrong

Version v0.1:
[+] First release
