using System.Drawing;

namespace TwitterConnector.Data
{
    public class TwitterUser
    {
        public TwitterUser()
        {
            
        }
        public TwitterUser(string name, string location, string description, string screenName, int followersCount, int friendsCount)
        {
            Name = name;
            Location = location;
            Description = description;
            ScreenName = screenName;
            FollowersCount = followersCount;
            FriendsCount = friendsCount;
        }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ScreenName { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
        public Image Picture { get; set; }
        public string PicturePath { get; set; }
    }
}
