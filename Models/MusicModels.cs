using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using DotLastFm45.Api;
using dotLastFm45;

namespace BootstrapDiStaula.Models
{
	struct LastFmConfig : ILastFmConfig
	{
		public string BaseUrl
		{
			get { return "http://ws.audioscrobbler.com/2.0/"; }
		}

		public string ApiKey
		{
			get { return ConfigurationManager.AppSettings["LastFMApiKey"]; }
		}
		public string Secret
		{
			get { return ConfigurationManager.AppSettings["LastFMSecret"]; }
		}

		public string Username
		{
			get { return ConfigurationManager.AppSettings["LastFMUserName"]; }
		}
	}
	[Serializable]
	public static class TopArtists
	{
		public static async Task<List<Artist>> GetWeeklyTopArtists(int limit = 10)
		{

			ILastFmConfig config = new LastFmConfig();
			string userName = ConfigurationManager.AppSettings["LastFMUserName"];
			string apiKey = ConfigurationManager.AppSettings["LastFMApiKey"];
			string secret = ConfigurationManager.AppSettings["LastFMSecret"];
			var lastFm = new LastFmApi(config);

			var things = lastFm.User.GetArtistTracks("llamabroth", "Radiohead");


			string url =
				string.Format(
					@"http://ws.audioscrobbler.com/2.0/?method=user.gettopartists&user={0}&period=7day&api_key={1}&limit={2}",
					userName, apiKey, limit);

			//var request = WebRequest.Create(_url + _json) as HttpWebRequest;
			HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.GetAsync(url);

			XmlDocument doc = new XmlDocument();
			doc.Load(await response.Content.ReadAsStreamAsync());

			var artists = doc.DocumentElement.SelectNodes("/lfm/topartists/artist");

			var artistList = (from XmlNode artist in artists select new Artist(artist)).ToList();

			InitilizeArtistList(artistList);

			return artistList;
		}

		private static void InitilizeArtistList(List<Artist> artistList)
		{
			int i = 0;
			Parallel.ForEach(artistList, a => { i += a.SongList.Count; });
		}
	}

	[Serializable]
	public class Artist
	{
		private XmlNode _artistNode;

		private string _name;
		[Display(Name = "Artist name")]
		public string Name
		{
			get
			{
				if (_name != null) return _name;

				var singleNode = _artistNode.SelectSingleNode("name");
				if (singleNode != null) _name = singleNode.FirstChild.Value;
				return _name;
			}
		}

		private int _playCount;
		[Display(Name = "Play Count")]
		public int PlayCount
		{
			get
			{
				if (_playCount != 0)
					return _playCount;

				var singleNode = _artistNode.SelectSingleNode("playcount");
				if (singleNode != null)
					_playCount = int.Parse(singleNode.FirstChild.Value);

				return _playCount;
			}
		}

		private string _id;
		[Display(Name = "ID")]
		public string ID
		{
			get
			{
				if (_id == null)
					return _id;

				var singleNode = _artistNode.SelectSingleNode("mbid");
				if (singleNode != null) _id = singleNode.FirstChild.Value;
				return _id;
			}
		}

		private string _url;
		[Display(Name = "Link")]
		[DataType(DataType.Url)]
		public string Url
		{
			get
			{
				if (_url != null)
					return _url;

				var singleNode = _artistNode.SelectSingleNode("url");
				if (singleNode != null) _url = singleNode.FirstChild.Value;

				return _url;
			}
		}

		private bool? _canStream;
		[Display(Name = "Streamable")]
		public bool? CanStream
		{
			get
			{
				if (_canStream != null)
					return _canStream;

				var singleNode = _artistNode.SelectSingleNode("streamable");
				if (singleNode != null)
					_canStream = singleNode.FirstChild.Value == "1";
				return _canStream;
			}
		}

		private string _smallImage;
		[Display(Name = "Small Image")]
		[DataType(DataType.ImageUrl)]
		public string SmallImage
		{
			get
			{
				if (_smallImage == null)
				{
					var singleNode = _artistNode.SelectSingleNode("image");
					if (singleNode != null) _smallImage = singleNode.FirstChild.Value;
					return _smallImage;
				}
				return _smallImage;
			}
		}

		private string _mediumImage;
		[Display(Name = "Medium Image")]
		[DataType(DataType.ImageUrl)]
		public string MediumImage
		{
			get { return _mediumImage; }
		}

		private string _largeImage;
		[Display(Name = "Large Image")]
		[DataType(DataType.ImageUrl)]
		public string LargeImage
		{
			get { return _largeImage; }
		}

		private List<Song> _songList;
		[Display(Name = "Song List")]
		public List<Song> SongList
		{
			get
			{
				if (_songList != null) return _songList;

				string userName = ConfigurationManager.AppSettings["LastFMUserName"];
				string apiKey = ConfigurationManager.AppSettings["LastFMApiKey"];
				int startTime = (int)DateTime.Now.Subtract(TimeSpan.FromDays(7)).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

				string url =
				string.Format(
					@"http://ws.audioscrobbler.com/2.0/?method=user.getArtistTracks&user={0}&api_key={1}&artist={2}&startTimestamp={3}",
					userName, apiKey, Name, startTime);

				//var request = WebRequest.Create(_url + _json) as HttpWebRequest;
				var request = WebRequest.Create(url) as HttpWebRequest;
				if (request == null)
					return null;


				request.Method = "GET";
				var response = request.GetResponse();
				XmlDocument doc = new XmlDocument();
				doc.Load(response.GetResponseStream());


				var tracks = doc.DocumentElement.SelectNodes("/lfm/artisttracks/track");

				_songList = (from XmlNode track in tracks select new Song(track)).ToList();
				return _songList;
			}
		}

		public Artist(XmlNode artist)
		{
			_artistNode = artist;

			//_smallImage = artist["_smallImage"].Value;
			//_mediumImage = artist["_mediumImage"].Value;
			//_largeImage = artist["name"].Value;

		}
	}

	[Serializable]
	public class Song
	{
		private XmlNode _songNode;

		private string _artistID;
		[Display(Name = "Artist MBID")]
		public string ArtistID
		{
			get
			{
				if (_artistID == null)
				{
					var singleNode = _songNode.SelectSingleNode("artist");
					if (singleNode != null) _artistID = singleNode.Attributes["mbid"].Value;
					return _artistID;
				}
				return _artistID;
			}
		}

		private string _title;
		[Display(Name = "Song Title")]
		public string Title
		{
			get
			{
				if (_title == null)
				{
					var singleNode = _songNode.SelectSingleNode("name");
					if (singleNode != null)
						_title = singleNode.FirstChild.Value;
				}
				return _title;
			}
		}

		private bool? _canStream;
		[Display(Name = "Streamable")]
		public bool? CanStream
		{
			get
			{
				if (_canStream == null)
				{
					var singleNode = _songNode.SelectSingleNode("streamable");
					if (singleNode != null)
						_canStream = singleNode.FirstChild.Value == "1";
				}
				return _canStream;
			}
		}

		private string _albumID;
		[Display(Name = "Album MBID")]
		public string AlbumID
		{
			get
			{
				if (_albumID == null)
				{
					var singleNode = _songNode.SelectSingleNode("album");
					if (singleNode != null) _albumID = singleNode.Attributes["mbid"].Value;
					return _albumID;
				}
				return _albumID;
			}
		}

		private string _url;
		[Display(Name = "Link")]
		[DataType(DataType.Url)]
		public string Url
		{
			get
			{
				if (_url != null)
					return _url;

				var singleNode = _songNode.SelectSingleNode("url");
				if (singleNode != null) _url = singleNode.FirstChild.Value;

				return _url;
			}
		}

		private string _smallImage;
		[Display(Name = "Small Image")]
		[DataType(DataType.ImageUrl)]
		public string SmallImage
		{
			get { return _smallImage; }
		}

		private string _mediumImage;
		[Display(Name = "Medium Image")]
		[DataType(DataType.ImageUrl)]
		public string MediumImage
		{
			get { return _mediumImage; }
		}

		private string _largeImage;
		[Display(Name = "Large Image")]
		[DataType(DataType.ImageUrl)]
		public string LargeImage
		{
			get { return _largeImage; }
		}

		public Song(XmlNode track)
		{
			_songNode = track;
		}

		private DateTime? _datePlayed;
		[Display(Name = "Date Played")]
		public DateTime? DatePlayed
		{
			get
			{
				if (_datePlayed != null)
					return _datePlayed;

				var singleNode = _songNode.SelectSingleNode("date");
				if (singleNode != null) _datePlayed = DateTime.Parse(singleNode.FirstChild.Value);

				return _datePlayed;
			}
		}
	}





}
