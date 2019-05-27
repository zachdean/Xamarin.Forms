using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Xamarin.Forms.Controls
{
	public class FeedItem
	{
		public FeedItem()
		{
		}

		public string Link { get; set; }
		public string PublishDate { get; set; }
		public string Author { get; set; }
		public int Id { get; set; }
		public string CommentCount { get; set; }
		public string Category { get; set; }
		public string Title { get; set; }
		public string Caption { get; set; }
		public string FirstImage { get; set; }
	}

	public static class StringUtils
	{

		internal static string ExtractCaption(this string caption)
		{
			//get rid of HTML tags
			caption = Regex.Replace(caption, "<[^>]*>", string.Empty);


			//get rid of multiple blank lines
			caption = Regex.Replace(caption, @"^\s*$\n", string.Empty, RegexOptions.Multiline);

			return caption.Substring(0, Math.Min(caption.Length, 200)).Trim() + "...";
		}

		internal static string ExtractImage(this string description)
		{
			var regx = new Regex("https://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?.(?:jpg|bmp|gif|png)", RegexOptions.IgnoreCase);

			var matches = regx.Matches(description);

			string firstImage;
			if (matches.Count == 0)
				firstImage = ScottHead;
			else
				firstImage = matches[0].Value;

			return firstImage;
		}

		static string ScottHead => "http://www.hanselman.com/images/photo-scott-tall.jpg";
	}

	public class Tweet
	{
		public Tweet()
		{
		}


		public ulong StatusID { get; set; }

		public string ScreenName { get; set; }

		public string Text { get; set; }

		//[JsonIgnore]
		public string Date { get { return CreatedAt.ToString("g"); } }
		//[JsonIgnore]
		public string RTCount { get { return CurrentUserRetweet == 0 ? string.Empty : CurrentUserRetweet + " RT"; } }

		public string Image { get; set; }

		public DateTime CreatedAt
		{
			get;
			set;
		}

		public ulong CurrentUserRetweet
		{
			get;
			set;
		}
	}

	public interface ITweetStore
	{
		void Save(System.Collections.Generic.List<Tweet> tweets);
		//System.Collections.Generic.List<Hanselman.Shared.Tweet> Load ();
	}
}

