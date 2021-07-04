using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Sound
{
    /// <summary>
    /// Represents a collection of paths that lead to
    /// songs to be played by the MusicManager.
    /// </summary>
    [DebuggerDisplay("{Name} - {Songs.Count} Song(s)")]
    public class Playlist
    {
        public const string DefaultPlaylistName = "Default";
        // Choose a character that can't appear in a Windows file name
        private const char ActivePlaylistsSourceSeperator = '|';

        public string Name { get; }
        public string ResourcesBasePath { get; }

        public List<string> Songs { get; }


        private static string ActivePlaylistsSource
        {
            get => PlayerPrefs.GetString(PlayerPrefsUtil.ActivePlaylistsKey, DefaultPlaylistName);
            set => PlayerPrefs.SetString(PlayerPrefsUtil.ActivePlaylistsKey, value);
        }

        public static List<string> ActivePlaylists
        {
            get
            {
                List<string> value = ActivePlaylistsSource
                  .Split(new char[] { ActivePlaylistsSourceSeperator }, StringSplitOptions.RemoveEmptyEntries)
                  .ToList();

                if (!value.Any())
                    value.Add(DefaultPlaylistName);

                return value;
            }
            set
            {
                if (value.Any())
                    ActivePlaylistsSource = String.Join(ActivePlaylistsSourceSeperator.ToString(), value);
                else
                {
                    List<string> toSet = new List<string>() { DefaultPlaylistName };
                    ActivePlaylistsSource = String.Join(ActivePlaylistsSourceSeperator.ToString(), toSet);
                }

                // TODO: Reload Music Manager
            }
        }

        public Playlist(DirectoryInfo dir)
        {
            Name = dir.Name;

            // dir.FullName example: C:\Users\TJ\Unity\Valkyrie Undying\Assets\Resources\Audio\Music\Default
            string resourcesPath = StringUtil.TextAfterFirst(dir.FullName, @"Resources\");
            ResourcesBasePath = $"{resourcesPath}\\";

            var files = dir.GetFiles().Where(x => x.Extension != ".meta").ToList();
            Songs = files.Select(x => x.Name.Replace(x.Extension, "")).ToList();
        }

        public IEnumerable<string> AllResourceNames()
        {
            return Songs.Select(x => $"{ResourcesBasePath}{x}");
        }

        #region Serialization

        // Unity causes issues with XmlSerializer.
        // Playlists must be manually serialized.

        public static string[] SerializePlaylists(List<Playlist> playlists)
        {
            if (!playlists.Any())
                return new string[0];

            StringBuilder sb = new StringBuilder();

            var playlist = playlists[0];
            playlist.Serialize(sb);

            for (int i = 1; i < playlists.Count; i++)
            {
                sb.AppendLine();

                playlist = playlists[i];
                playlist.Serialize(sb);
            }

            bool ShouldTrimSb()
            {
                char c = sb[sb.Length - 1];
                bool shouldTrim = c == '\n' || c == '\r';
                return shouldTrim;
            }

            while(ShouldTrimSb())
                sb.Length--;

            string str = sb.ToString();
            string[] lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            return lines;
        }

        private void Serialize(StringBuilder sb)
        {
            sb.AppendLine(Name);
            sb.AppendLine(ResourcesBasePath);

            foreach (var song in Songs)
                sb.AppendLine(song);
        }

        public static List<Playlist> DeserializePlaylistsFromBuild(string defaultResourcePath, string extraResourcePath)
        {
            // Environment.NewLine may differ depending on build target
            const string EnvironmentNewLine = "\r\n";
            const string NewLine = "\n";

            string[] GetLines(string _resourcePath)
            {
                TextAsset textFile = (TextAsset)Resources.Load(_resourcePath);

                if (textFile == null)
                    return new string[0];

                string deserialized = textFile.text;

                string[] lines = deserialized.Replace(EnvironmentNewLine, NewLine)
                    .Split(new string[] { NewLine }, StringSplitOptions.None)
                    .ToArray();

                return lines;
            }

            string[] defaultLines = GetLines(defaultResourcePath);
            string[] extraLines = GetLines(extraResourcePath);

            return DeserializePlaylists(defaultLines, extraLines);
        }

        public static List<Playlist> DeserializePlaylists(string defaultSerializationFilePath, string extraSerializationFilePath)
        {
            var defaultLines = File.ReadAllLines(defaultSerializationFilePath);
            var extraLines = File.ReadAllLines(extraSerializationFilePath);
            var playlists = Playlist.DeserializePlaylists(defaultLines, extraLines);

            return playlists;
        }

        private static List<Playlist> DeserializePlaylists(string[] defaultPlaylistLines, string[] extraPlaylistLines)
        {
            List<Playlist> playlists = new List<Playlist>();

            DeserializePlaylistIntoList(defaultPlaylistLines, playlists);
            DeserializePlaylistIntoList(extraPlaylistLines, playlists);

            return playlists;
        }

        private static void DeserializePlaylistIntoList(string[] lines, List<Playlist> playlists)
        {
            // In case we're deserializing an empty extra playlist set
            if (lines.Length <= 1)
                return;

            int currentLineNumber = 0;

            int maxLine = lines.Length;
            while (currentLineNumber < maxLine)
            {
                string name = lines[currentLineNumber++];
                string resourcesBasePath = lines[currentLineNumber++];

                List<string> songs = new List<string>();

                string currentLine = lines[currentLineNumber];
                while (!String.IsNullOrWhiteSpace(currentLine))
                {
                    songs.Add(currentLine);
                    currentLineNumber++;

                    if (currentLineNumber >= maxLine)
                        break;

                    currentLine = lines[currentLineNumber];
                }
                currentLineNumber++;

                Playlist playlist = new Playlist(name, resourcesBasePath, songs);
                playlists.Add(playlist);
            }
        }


        private Playlist(string name, string resourcesBasePath, List<string> songs)
        {
            Name = name;
            ResourcesBasePath = resourcesBasePath;
            Songs = songs;

        }

        #endregion Serialization
    }
}
