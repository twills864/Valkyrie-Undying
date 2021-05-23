﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Sound
{
    [DebuggerDisplay("{Name} - {Songs.Count} Song(s)")]
    public class Playlist
    {
        public string Name { get; }
        public string ResourcesBasePath { get; }

        public List<string> Songs { get; }

        public Playlist(DirectoryInfo dir)
        {
            Name = dir.Name;

            // dir.FullName example: C:\Users\TJ\Unity\Valkyrie Undying\Assets\Resources\Audio\Music\Default
            string resourcesPath = StringUtil.TextAfterFirst(dir.FullName, @"Resources\");
            ResourcesBasePath = resourcesPath.Replace(Name, "");

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
            StringBuilder sb = new StringBuilder();

            var playlist = playlists[0];
            playlist.Serialize(sb);

            for (int i = 1; i < playlists.Count; i++)
            {
                sb.AppendLine();

                playlist = playlists[i];
                playlist.Serialize(sb);
            }

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

        public static List<Playlist> DeserializePlaylists(string[] lines)
        {
            List<Playlist> playlists = new List<Playlist>();

            int currentLineNumber = 0;

            int maxLine = lines.Length;
            while(currentLineNumber < maxLine)
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

            return playlists;
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