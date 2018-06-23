// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Text;

using MediaInfoWrapper;

using MeGUI.core.util;
using MeGUI.packages.tools.hdbdextractor;

namespace MeGUI
{
    public class MediaInfoException : Exception
    {
        public MediaInfoException(Exception e)
        : base("Media info error: " + e.Message, e)
        {}
    }

    public class MediaInfoFileFactory : IMediaFileFactory
    {
        #region IMediaFileFactory Members

        public IMediaFile Open(string file)
        {
            return new MediaInfoFile(file);
        }

        #endregion

        #region IMediaFileFactory Members


        public int HandleLevel(string file)
        {
            return 5;
        }

        #endregion

        #region IIDable Members

        public string ID
        {
            get { return "MediaInfo/DirectShowSource"; }
        }

        #endregion
    }

    public class MediaInfoFile : IMediaFile
    {
        #region variables
        private static readonly CultureInfo culture = new CultureInfo("en-us");
        private static Dictionary<string, VideoCodec> knownVideoDescriptions;
        private static Dictionary<string, ContainerType> knownContainerTypes;
        private IMediaFile videoSourceFile = null;
        private IVideoReader videoReader = null; 
        private ContainerType cType;
        private string _strContainer = "";
        private string _file;
        private VideoInformation _VideoInfo;
        private AudioInformation _AudioInfo;
        private SubtitleInformation _SubtitleInfo;
        private ChapterInfo _ChapterInfo;
        private MkvInfo _MkvInfo;
        private Eac3toInfo _Eac3toInfo;
        private LogItem _Log;
        private List<string> _attachments;
        #endregion
        #region properties
        public AudioInformation AudioInfo
        {
            get { return _AudioInfo; }
        }

        public ChapterInfo ChapterInfo
        {
            get { return _ChapterInfo; }
        }

        public SubtitleInformation SubtitleInfo
        {
            get { return _SubtitleInfo; }
        }

        public VideoInformation VideoInfo
        {
            get { return _VideoInfo; }
        }

        public ContainerType ContainerFileType
        {
            get { return cType; }
        }

        public string ContainerFileTypeString
        {
            get { return _strContainer; }
        }

        public List<string> Attachments
        {
            get { return _attachments; }
        }
        #endregion

        public MediaInfoFile(string file, ref LogItem oLog) : this(file, ref oLog, 1, 0)
        {
        }

        public MediaInfoFile(string file, ref LogItem oLog, int iPGCNumber, int iAngleNumber)
        {
            GetSourceInformation(file, oLog, iPGCNumber, iAngleNumber);
        }

        public MediaInfoFile(string file) : this(file, 1, 0)
        {
        }

        public MediaInfoFile(string file, int iPGCNumber, int iAngleNumber)
        {
            GetSourceInformation(file, null, iPGCNumber, iAngleNumber);
        }

        /// <summary>
        /// gets information about a source using MediaInfo
        /// </summary>
        /// <param name="file">the file to be analyzed</param>
        /// <param name="oLog">the log item</param>
        private void GetSourceInformation(string file, LogItem oLog, int iPGCNumber, int iAngleNumber)
        {
            if (file.Contains("|"))
                file = file.Split('|')[0];
            _Log = oLog;
            _file = file;
            _indexerToUse = FileIndexerWindow.IndexType.NONE;
            this._AudioInfo = new AudioInformation();
            this._SubtitleInfo = new SubtitleInformation();
            this._VideoInfo = new VideoInformation(false, 0, 0, Dar.A1x1, 0, 0, 0, 1);
            this._ChapterInfo = new ChapterInfo();
            this._attachments = new List<string>();

            MediaInfo info = null;

            try
            {
                LogItem infoLog = null;
                if (oLog != null)
                {
                    infoLog = oLog.LogValue("MediaInfo", String.Empty);
                    infoLog.LogEvent("File: " + _file);
                }

                if (!File.Exists(file))
                {
                    if (oLog != null)
                        infoLog.LogEvent("The file cannot be opened", ImageType.Warning);
                    else
                    {
                        oLog = MainForm.Instance.Log.Info("MediaInfo");
                        oLog.LogEvent("File: " + _file);
                        oLog.LogEvent("The file cannot be opened", ImageType.Warning);
                    }  
                    return;
                }

                // if an index file is used extract the real file name
                if (Path.GetExtension(file).ToLowerInvariant().Equals(".d2v") ||
                    Path.GetExtension(file).ToLowerInvariant().Equals(".dgi"))
                {
                    using (StreamReader sr = new StreamReader(file, Encoding.Default))
                    {
                        string line = null;
                        int iLineCount = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            iLineCount++;
                            if (iLineCount == 3 && Path.GetExtension(file).ToLowerInvariant().Equals(".d2v"))
                            {
                                string strSourceFile = line;
                                if (!File.Exists(strSourceFile))
                                {
                                    // try it also with absolute path name
                                    strSourceFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), strSourceFile));
                                }
                                if (File.Exists(strSourceFile))
                                {
                                    _file = file = strSourceFile;
                                    if (infoLog != null)
                                        infoLog.Info("Indexed file: " + _file);
                                }
                                else if (infoLog != null)
                                    infoLog.Error("Indexed file not found: " + line);
                                break;
                            }
                            else if (iLineCount == 4)
                            {
                                string strSourceFile = line.Substring(0, line.LastIndexOf(" "));
                                if (!File.Exists(strSourceFile))
                                {
                                    // try it also with absolute path name
                                    strSourceFile = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), strSourceFile));
                                }
                                if (File.Exists(strSourceFile))
                                {
                                    _file = file = strSourceFile;
                                    if (infoLog != null)
                                        infoLog.Info("Indexed file: " + _file);
                                }
                                else if (infoLog != null)
                                    infoLog.Error("Indexed file not found: " + line);
                                break;
                            }
                        }
                    }
                }
                else if (Path.GetExtension(file).ToLowerInvariant().Equals(".lwi"))
                {
                    using (StreamReader sr = new StreamReader(file, Encoding.Default))
                    {
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("<InputFilePath>"))
                            {
                                string strSourceFile = line.Substring(15, line.LastIndexOf("</InputFilePath>") - 15);
                                if (File.Exists(strSourceFile))
                                    _file = file = strSourceFile;
                                break;
                            }
                        }
                    }
                    if (infoLog != null)
                        infoLog.Info("Indexed File: " + _file);
                }
                else if (Path.GetExtension(file).ToLowerInvariant().Equals(".bup"))
                {
                    // ignore .BUP (DVD) files
                    if (oLog != null)
                        infoLog.LogEvent("The file is not supported", ImageType.Warning);
                    return;
                }

                // get basic media information
                if (Path.GetExtension(file.ToLowerInvariant()) == ".vob")
                {
                    string ifoFile;
                    if (Path.GetFileName(file).ToUpperInvariant().Substring(0, 4) == "VTS_")
                        ifoFile = file.Substring(0, file.LastIndexOf("_")) + "_0.IFO";
                    else
                        ifoFile = Path.ChangeExtension(file, ".IFO");

                    if (File.Exists(ifoFile))
                    {
                        if (infoLog != null)
                            infoLog.Info("DVD source detected. Using IFO file: " + ifoFile);
                        file = ifoFile;
                    }
                }

                // open MediaInfo in a thread to prevent GUI locking 
                Thread processMediaInfo = new Thread(new ThreadStart(delegate
                {
                    info = new MediaInfo(file);
                }));
                processMediaInfo.Start();
                while (processMediaInfo.ThreadState == ThreadState.Running || processMediaInfo.ThreadState == ThreadState.WaitSleepJoin)
                    MeGUI.core.util.Util.Wait(100);

                if (info == null || !info.OpenSuccess)
                {
                    if (oLog != null)
                        infoLog.LogEvent("The file cannot be opened", ImageType.Warning);
                    else
                    {
                        oLog = MainForm.Instance.Log.Info("MediaInfo");
                        oLog.LogEvent("File: " + _file);
                        oLog.LogEvent("The file cannot be opened", ImageType.Warning);
                    }
                    return;
                }

                CorrectSourceInformation(ref info, file, infoLog, iPGCNumber, iAngleNumber);
                if (infoLog != null)
                    WriteSourceInformation(info, file, infoLog);

                // container detection
                if (info.General.Count < 1)
                {
                    cType = null;
                }
                else
                {
                    _strContainer = info.General[0].Format;
                    if (_strContainer.Contains(" / "))
                        _strContainer = _strContainer.Substring(0, _strContainer.IndexOf(" / "));

                    cType = getContainerType(_strContainer, info.General[0].FormatString);

                    _attachments.AddRange(info.General[0].Attachments.Split(new string[] { " / " }, StringSplitOptions.RemoveEmptyEntries));
                }

                // audio detection
                for (int counter = 0; counter < info.Audio.Count; counter++)
                {
                    MediaInfoWrapper.AudioTrack atrack = info.Audio[counter];

                    if (String.IsNullOrEmpty(atrack.Delay) && String.IsNullOrEmpty(atrack.SamplingRate) 
                        && String.IsNullOrEmpty(atrack.FormatProfile) && String.IsNullOrEmpty(atrack.Channels))
                        continue;

                    AudioTrackInfo ati = new AudioTrackInfo();
                    ati.SourceFileName = _file;
                    ati.Name = atrack.Title;
                    ati.DefaultTrack = atrack.DefaultString.ToLowerInvariant().Equals("yes");
                    ati.ForcedTrack = atrack.ForcedString.ToLowerInvariant().Equals("yes");
                    // DGIndex expects audio index not ID for TS
                    ati.ContainerType = _strContainer;
                    ati.TrackIndex = counter;
                    int iID = 0;
                    if (ati.ContainerType == "CDXA/MPEG-PS")
                        // MediaInfo doesn't give TrackID for VCD, specs indicate only MP1L2 is supported
                        ati.TrackID = (0xC0 + counter);
                    else if (atrack.ID != "0" && atrack.ID != "" &&
                             (Int32.TryParse(atrack.ID, out iID) ||
                             (atrack.ID.Contains("-") && Int32.TryParse(atrack.ID.Split('-')[1], out iID))))
                        ati.TrackID = iID;
                    else
                    {
                        // MediaInfo failed to get ID try guessing based on codec
                        switch (atrack.Format.Substring(0, 3))
                        {
                            case "AC-":
                            case "AC3": ati.TrackID = (0x80 + counter); break;
                            case "PCM": ati.TrackID = (0xA0 + counter); break;
                            case "MPE": // MPEG-1 Layer 1/2/3
                            case "MPA": ati.TrackID = (0xC0 + counter); break;
                            case "DTS": ati.TrackID = (0x88 + counter); break;
                        }
                    }
                    if (Int32.TryParse(atrack.StreamOrder, out iID) ||
                        (atrack.StreamOrder.Contains("-") && Int32.TryParse(atrack.StreamOrder.Split('-')[1], out iID)))
                        ati.MMGTrackID = iID;
                    if (atrack.FormatProfile != "") // some tunings to have a more useful info instead of a typical audio Format
                    {
                        switch (atrack.FormatProfile)
                        {
                            case "Layer 1": ati.Codec = "MPA"; break;
                            case "Layer 2": ati.Codec = "MP2"; break;
                            case "Layer 3": ati.Codec = "MP3"; break;
                            default: ati.Codec = atrack.Format; break;
                        }
                    }
                    else
                        ati.Codec = atrack.Format;

                    ati.AudioCodec = getAudioCodec(ati.Codec);
                    if (ati.AudioCodec != null)
                        ati.AudioType = getAudioType(ati.AudioCodec, cType, file);

                    if ((atrack.Format.Equals("E-AC-3") && atrack.MuxingMode.Equals("Stream extension"))
                        || (atrack.Format.Equals("DTS") && atrack.FormatProfile.Contains("/"))
                        || (ati.AudioCodec == AudioCodec.THDAC3))
                        ati.HasCore = true;

                    if (MainForm.Instance.Settings.ShowDebugInformation && ati.AudioCodec == AudioCodec.UNKNOWN)
                    {
                        if (_Log == null)
                            _Log = MainForm.Instance.Log.Info("MediaInfo");
                        _Log.LogEvent("Unknown audio codec found: \"" + atrack.Format + "\"" 
                            + (!String.IsNullOrEmpty(atrack.FormatProfile) ? (", \"" + atrack.FormatProfile + "\"") : String.Empty), ImageType.Warning);
                    }

                    if (atrack.BitRateMode == "VBR")
                        ati.BitrateMode = BitrateManagementMode.VBR;

                    ati.NbChannels = atrack.ChannelsString;
                    ati.ChannelPositions = atrack.ChannelPositionsString2;
                    ati.SamplingRate = atrack.SamplingRateString;
                    int delay = 0;
                    Int32.TryParse(atrack.Delay, out delay);
                    ati.Delay = delay;
                    // gets SBR/PS flag from AAC streams
                    if (atrack.Format == "AAC")
                    {
                        ati.AACFlag = 0;
                        if (atrack.FormatSettingsSBR == "Yes")
                        {
                            if (atrack.FormatSettingsPS == "Yes")
                                ati.AACFlag = 2;
                            else 
                                ati.AACFlag = 1;
                        }
                        if (atrack.SamplingRate == "24000")
                        {
                            if ((atrack.Channels == "2") || (atrack.Channels == "1")) // workaround for raw aac
                                ati.AACFlag = 1;
                        }
                    }
                    ati.Language = getLanguage(atrack.Language, atrack.LanguageString, ref infoLog, infoLog == null, false);
                    _AudioInfo.Tracks.Add(ati);
                }

                //subtitle detection
                int i = 0;
                foreach (TextTrack oTextTrack in info.Text)
                {
                    int mmgTrackID = 0;
                    if (!Int32.TryParse(oTextTrack.StreamOrder, out mmgTrackID) && oTextTrack.StreamOrder.Contains("-"))
                        Int32.TryParse(oTextTrack.StreamOrder.Split('-')[1], out mmgTrackID);
                    SubtitleTrackInfo oTrack = new SubtitleTrackInfo(mmgTrackID, getLanguage(oTextTrack.Language, oTextTrack.LanguageString, ref infoLog, infoLog == null, false), oTextTrack.Title);
                    oTrack.DefaultTrack = oTextTrack.DefaultString.ToLowerInvariant().Equals("yes");
                    oTrack.ForcedTrack = oTextTrack.ForcedString.ToLowerInvariant().Equals("yes");
                    oTrack.SourceFileName = file;
                    oTrack.Codec = oTextTrack.CodecString;
                    oTrack.ContainerType = _strContainer;
                    oTrack.TrackIndex = i++;
                    int delay = 0;
                    Int32.TryParse(oTextTrack.Delay, out delay);
                    oTrack.Delay = delay;

                    // only add supported subtitle tracks
                    string strCodec = oTrack.Codec.ToUpperInvariant();
                    if (cType == ContainerType.MKV)
                    {
                        string[] arrCodec = new string[] { };
                        arrCodec = strCodec.Split('/');
                        if (arrCodec[0].Substring(1, 1).Equals("_"))
                            arrCodec[0] = arrCodec[0].Substring(2);
                        strCodec = arrCodec[0];
                    }
                    switch (strCodec)
                    {
                        case "VOBSUB": 
                        case "ASS": 
                        case "UTF-8": 
                        case "SSA": 
                        case "USF": 
                        case "HDMV":
                        case "PGS": break;
                        default: strCodec = "unknown"; break;
                    }

                    if (!strCodec.Equals("unknown"))
                        _SubtitleInfo.Tracks.Add(oTrack);
                }

                // video detection
                _VideoInfo.HasVideo = (info.Video.Count > 0);
                if (_VideoInfo.HasVideo)
                {
                    MediaInfoWrapper.VideoTrack track = info.Video[0];
                    checked
                    {
                        int _trackID = 0;
                        Int32.TryParse(track.ID, out _trackID);
                        int _mmgTrackID = 0;
                        Int32.TryParse(track.StreamOrder, out _mmgTrackID);

                        VideoTrackInfo videoInfo = new VideoTrackInfo(_trackID, _mmgTrackID, getLanguage(track.Language, track.LanguageString, ref infoLog, infoLog == null, true), track.Title, track.CodecString, track.Codec);
                        videoInfo.ContainerType = _strContainer;
                        _VideoInfo.Track = videoInfo;

                        _VideoInfo.Width = (ulong)easyParseInt(track.Width).Value;
                        _VideoInfo.Height = (ulong)easyParseInt(track.Height).Value;
                        _VideoInfo.FrameCount = (ulong)(easyParseInt(track.FrameCount) ?? 0);
                        _VideoInfo.ScanType = track.ScanTypeString;
                        _VideoInfo.Codec = getVideoCodec(track.Codec);
                        if (_VideoInfo.Codec == null)
                            _VideoInfo.Codec = getVideoCodec(track.Format); // sometimes codec info is not available, check the format then...
                        if (MainForm.Instance.Settings.ShowDebugInformation && _VideoInfo.Codec == null && !track.Format.Equals("AVS"))
                        {
                            if (_Log == null)
                                _Log = MainForm.Instance.Log.Info("MediaInfo");
                            _Log.LogEvent("Unknown video codec found: " + track.Codec + " / " + track.Format, ImageType.Warning);
                        }
                        _VideoInfo.Type = getVideoType(_VideoInfo.Codec, cType, file);
                        _VideoInfo.DAR = Resolution.GetDAR((int)_VideoInfo.Width, (int)_VideoInfo.Height, track.AspectRatio, easyParseDecimal(track.PixelAspectRatio), track.AspectRatioString);

                        double? fps = easyParseDouble(track.FrameRate) ?? easyParseDouble(track.FrameRateOriginal);
                        if (fps == null)
                        {
                            fps = 23.976;
                            if (infoLog == null)
                            {
                                infoLog = MainForm.Instance.Log.Info("MediaInfo");
                                infoLog.Info("File: " + _file);
                                infoLog.Info("FrameRate: " + track.FrameRate);
                                infoLog.Info("FrameRateOriginal: " + track.FrameRateOriginal);

                            }
                            infoLog.LogEvent("fps cannot be determined. 23.976 will be used as default.", ImageType.Warning);
                        }
                        _VideoInfo.FPS = (double)fps;

                        Int32.TryParse(track.BitDepth, out _VideoInfo.BitDepth);
                    }
                }

                // chapter detection
                foreach (MediaInfoWrapper.ChaptersTrack t in info.Chapters)
                {
                    if (_ChapterInfo.HasChapters)
                        break;

                    _ChapterInfo.SourceFilePath = _file;
                    _ChapterInfo.FramesPerSecond = _VideoInfo.FPS;
                    _ChapterInfo.Title = Path.GetFileNameWithoutExtension(_file);
                    if (info.General.Count > 0)
                    {
                        if (TimeSpan.TryParse(info.General[0].PlayTimeString3, new System.Globalization.CultureInfo("en-US"), out TimeSpan result))
                            _ChapterInfo.Duration = result;
                    }
                    foreach (KeyValuePair<string, string> ChapterKeyPair in t.Chapters)
                    {
                        string chapterTime = ChapterKeyPair.Key;
                        TimeSpan chapterSpan;
                        if (!TimeSpan.TryParse(chapterTime, out chapterSpan))
                            continue;

                        string chapterName = ChapterKeyPair.Value;
                        if (chapterName.Contains(":"))
                            chapterName = chapterName.Substring(chapterName.IndexOf(":") + 1);
                        Chapter oChapter = new Chapter();
                        oChapter.Name = chapterName;
                        oChapter.SetTimeBasedOnString(ChapterKeyPair.Key);
                        _ChapterInfo.Chapters.Add(oChapter);
                    }
                }
                if (Path.GetExtension(file).ToLowerInvariant().Equals(".mpls"))
                {
                    // Blu-ray playlist chapters
                    // MediaInfo sometimes does not extract all chapters (only 00:00:00 = 1 chapter)
                    // therefore check if all chapters have been detected properly
                    ChapterInfo pgc = new ChapterInfo();
                    MplsExtractor ex = new MplsExtractor();
                    pgc = ex.GetChapterInfo(file, _VideoInfo.FPS);
                    if (pgc.HasChapters && _ChapterInfo.Chapters.Count != pgc.Chapters.Count)
                        _ChapterInfo = pgc;
                }
            }
            catch (Exception ex)
            {
                if (oLog == null)
                    oLog = MainForm.Instance.Log.Info("MediaInfo");
                oLog.LogValue("MediaInfo - Unhandled Error", ex, ImageType.Error);
            }
            finally
            {
                if (info != null)
                    info.Dispose();
            }
        }

        private string getLanguage(string languageISO, string language, ref LogItem oLog, bool bFullProcess, bool bVideoTrack)
        {
            if (!bFullProcess)
                return language;

            string temp = string.Empty;
            if (!String.IsNullOrEmpty(languageISO))
            {
                temp = LanguageSelectionContainer.LookupISOCode(languageISO);
                if (!String.IsNullOrEmpty(temp))
                    return temp;
            }

            if (!String.IsNullOrEmpty(language))
            {
                temp = LanguageSelectionContainer.LookupISOCode(language);
                if (!String.IsNullOrEmpty(temp))
                    return temp;

                if (LanguageSelectionContainer.IsLanguageAvailable(language))
                    return language;
            }

            if (bVideoTrack)
            {
                if (!String.IsNullOrEmpty(language) || !String.IsNullOrEmpty(languageISO))
                {
                    if (oLog == null)
                        oLog = MainForm.Instance.Log.Info("MediaInfo");
                    oLog.LogEvent("The language information \"" + languageISO + "/" + language + "\" is unknown and has been skipped.", ImageType.Warning);
                }
                return "";
            }
            else
            {
                if (String.IsNullOrEmpty(language) && String.IsNullOrEmpty(languageISO))
                {
                    if (oLog != null)
                        oLog.LogEvent("The language information is not available for this track. The default MeGUI language has been selected.", ImageType.Information);
                }
                else
                {
                    if (oLog == null)
                        oLog = MainForm.Instance.Log.Info("MediaInfo");
                    oLog.LogEvent("The language information \"" + languageISO + "/" + language + "\" is unknown. The default MeGUI language has been selected instead.", ImageType.Warning);
                }
               return MeGUI.MainForm.Instance.Settings.DefaultLanguage1;
            }
        }

        private void WriteSourceInformation(MediaInfo oInfo, string strFile, LogItem infoLog)
        {
            try
            {
                // general track
                foreach (MediaInfoWrapper.GeneralTrack t in oInfo.General)
                {
                    LogItem oTrack = new LogItem("General");
                      
                    oTrack.Info("Format: " + t.Format);
                    oTrack.Info("FormatString: " + t.FormatString);
                    oTrack.Info("FileSize: " + t.FileSize);
                    oTrack.Info("PlayTime: " + t.PlayTimeString3);
                    if (_VideoInfo.PGCCount > 0)
                    {
                        oTrack.Info("PGCCount: " + _VideoInfo.PGCCount);
                        oTrack.Info("PGCNumber: " + _VideoInfo.PGCNumber);
                    }
                    if (_VideoInfo.AngleCount > 0)
                    {
                        oTrack.Info("AngleCount: " + _VideoInfo.AngleCount);
                        oTrack.Info("AngleNumber: " + _VideoInfo.AngleNumber);
                    }
                    if (!String.IsNullOrEmpty(t.Attachments))
                        oTrack.Info("Attachments: " + t.Attachments);

                    infoLog.Add(oTrack);
                }

                // video track
                foreach (MediaInfoWrapper.VideoTrack t in oInfo.Video)
                {
                    LogItem oTrack = new LogItem("Video");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Width: " + t.Width);
                    oTrack.Info("Height: " + t.Height);
                    oTrack.Info("FrameCount: " + t.FrameCount);
                    oTrack.Info("FrameRate: " + t.FrameRate);
                    oTrack.Info("FrameRateOriginal: " + t.FrameRateOriginal);
                    oTrack.Info("FrameRateMode: " + t.FrameRateMode);
                    oTrack.Info("ScanType: " + t.ScanTypeString);
                    oTrack.Info("Codec: " + t.Codec);
                    oTrack.Info("CodecString: " + t.CodecString);
                    oTrack.Info("Bits Depth: " + t.BitDepth);
                    oTrack.Info("Format: " + t.Format);
                    oTrack.Info("AspectRatio: " + t.AspectRatio);
                    oTrack.Info("AspectRatioString: " + t.AspectRatioString);
                    oTrack.Info("Delay: " + t.Delay);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);
                    oTrack.Info("Default: " + t.Default);
                    oTrack.Info("DefaultString: " + t.DefaultString);
                    oTrack.Info("Forced: " + t.Forced);
                    oTrack.Info("ForcedString: " + t.ForcedString);
                    t.LanguageString = getLanguage(t.Language, t.LanguageString, ref oTrack, true, true);

                    infoLog.Add(oTrack);
                }

                // audio track
                foreach (MediaInfoWrapper.AudioTrack t in oInfo.Audio)
                {
                    LogItem oTrack = new LogItem("Audio");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Format: " + t.Format);
                    oTrack.Info("FormatProfile: " + t.FormatProfile);
                    oTrack.Info("FormatSettingsSBR: " + t.FormatSettingsSBR);
                    oTrack.Info("FormatSettingsPS: " + t.FormatSettingsPS);
                    oTrack.Info("Muxing Mode: " + t.MuxingMode);
                    oTrack.Info("SamplingRate: " + t.SamplingRate);
                    oTrack.Info("SamplingRateString: " + t.SamplingRateString);
                    oTrack.Info("Channels: " + t.Channels);
                    oTrack.Info("ChannelsString: " + t.ChannelsString);
                    oTrack.Info("ChannelPositionsString2: " + t.ChannelPositionsString2);
                    oTrack.Info("BitRateMode: " + t.BitRateMode);
                    oTrack.Info("Delay: " + t.Delay);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);
                    oTrack.Info("Default: " + t.Default);
                    oTrack.Info("DefaultString: " + t.DefaultString);
                    oTrack.Info("Forced: " + t.Forced);
                    oTrack.Info("ForcedString: " + t.ForcedString);
                    t.LanguageString = getLanguage(t.Language, t.LanguageString, ref oTrack, true, false);

                    infoLog.Add(oTrack);
                }

                // text track
                foreach (MediaInfoWrapper.TextTrack t in oInfo.Text)
                {
                    LogItem oTrack = new LogItem("Text");

                    oTrack.Info("ID: " + t.ID);
                    oTrack.Info("StreamOrder: " + t.StreamOrder);
                    oTrack.Info("Codec: " + t.Codec);
                    oTrack.Info("CodecString: " + t.CodecString);
                    oTrack.Info("Delay: " + t.Delay);
                    oTrack.Info("Title: " + t.Title);
                    oTrack.Info("Language: " + t.Language);
                    oTrack.Info("LanguageString: " + t.LanguageString);
                    oTrack.Info("Default: " + t.Default);
                    oTrack.Info("DefaultString: " + t.DefaultString);
                    oTrack.Info("Forced: " + t.Forced);
                    oTrack.Info("ForcedString: " + t.ForcedString);
                    t.LanguageString = getLanguage(t.Language, t.LanguageString, ref oTrack, true, false);

                    infoLog.Add(oTrack);
                }

                // chapter track
                foreach (MediaInfoWrapper.ChaptersTrack t in oInfo.Chapters)
                {
                    LogItem oTrack = new LogItem("Chapters");

                    foreach (KeyValuePair<string, string> oChapter in t.Chapters)
                        oTrack.Info(oChapter.Key + ": " + oChapter.Value);
                   
                    infoLog.Add(oTrack);
                }                
            }
            catch (Exception ex)
            {
                infoLog.LogValue("Error parsing media file", ex, ImageType.Information);
            }
        }

        private void CorrectSourceInformation(ref MediaInfo oInfo, String strFile, LogItem infoLog, int iPGCNumber, int iAngleNumber)
        {
            try
            {
                if (oInfo.Video.Count > 0 && Path.GetExtension(strFile.ToLowerInvariant()) == ".ifo")
                {
                    // PGC handling
                    int iPGCCount = IFOparser.GetPGCCount(strFile);
                    _VideoInfo.PGCCount = iPGCCount;
                    if (iPGCNumber < 1 || iPGCNumber > iPGCCount)
                        _VideoInfo.PGCNumber = 1;
                    else
                        _VideoInfo.PGCNumber = iPGCNumber;

                    // Angle handling
                    _VideoInfo.AngleCount = IFOparser.GetAngleCount(strFile, _VideoInfo.PGCNumber);
                    if (_VideoInfo.AngleCount > 0 && (iAngleNumber < 1 || iAngleNumber > _VideoInfo.AngleCount))
                        _VideoInfo.AngleNumber = 1;
                    else if (iAngleNumber < 0 || iAngleNumber > _VideoInfo.AngleCount)
                        _VideoInfo.AngleNumber = 0;
                    else
                        _VideoInfo.AngleNumber = iAngleNumber;

                    // subtitle information is wrong in VOB/IFO (mediainfo), use IFO directly instead
                    oInfo.Text.Clear();
                    string[] arrSubtitle = IFOparser.GetSubtitlesStreamsInfos(strFile, _VideoInfo.PGCNumber, false);
                    foreach (string strSubtitle in arrSubtitle)
                    {
                        TextTrack oTextTrack = new TextTrack();
                        oTextTrack.StreamOrder = Int32.Parse(strSubtitle.Substring(1, 2)).ToString();
                        string[] strLanguage = strSubtitle.Split(new string[] { " - " }, StringSplitOptions.None);
                        if (strLanguage.Length == 3)
                        {
                            oTextTrack.LanguageString = strLanguage[1].Trim();
                            oTextTrack.Title = strLanguage[2].Trim();
                        }
                        else if (strLanguage.Length == 2)
                            oTextTrack.LanguageString = strLanguage[1].Trim();
                        else
                            oTextTrack.Title = strSubtitle;

                        if (strSubtitle.ToLowerInvariant().Contains("forced"))
                            oTextTrack.ForcedString = "yes";
                        oTextTrack.CodecString = SubtitleType.VOBSUB.ToString();
                        oInfo.Text.Add(oTextTrack);
                    }
                }
                else if (oInfo.General.Count > 0 && oInfo.Video.Count > 0 && oInfo.General[0].FormatString.ToLowerInvariant().Equals("blu-ray playlist"))
                {
                    // Blu-ray Input File
                    if (infoLog != null)
                        infoLog.LogEvent("Blu-ray playlist detected. Getting information from eac3to.", ImageType.Information);

                    _Eac3toInfo = new Eac3toInfo(new List<string>() { strFile }, this, infoLog);
                    _Eac3toInfo.FetchAllInformation();

                    int iAudioCount = 0; int iAudioEac3toCount = 0;
                    int iTextCount = 0; int iTextEac3toCount = 0;
                    bool bVideoFound = false;
                    int iCount = 0;
                    foreach (eac3to.Stream oTrack in _Eac3toInfo.Features[0].Streams)
                    {
                        if (oTrack.Number < iCount)
                            break;
                        else
                            iCount = oTrack.Number;

                        if (oTrack.Type == eac3to.StreamType.Subtitle)
                        {
                            iTextEac3toCount++;
                            string strLanguageEac3To = oTrack.Language;
                            string strLanguageMediaInfo = getLanguage(oInfo.Text[iTextCount].Language, oInfo.Text[iTextCount].LanguageString, ref infoLog, true, false);
                            while (oInfo.Text.Count > iTextCount && !strLanguageMediaInfo.Equals(strLanguageEac3To))
                            {
                                // this workaround works only if there are additional tracks in MediaInfo which are not available in eac3to (already seen in the wild)
                                // it works not when tracks are flipped (not noticed yet)
                                infoLog.LogEvent("Language information does not match. MediaInfo subtitle track will be removed: " + strLanguageMediaInfo + " <--> " + oTrack.Language, ImageType.Information);
                                oInfo.Text.RemoveRange(iTextCount, 1);
                            }

                            if (oInfo.Text.Count > iTextCount)
                                oInfo.Text[iTextCount++].StreamOrder = oTrack.Number.ToString();
                        }
                        else if (oTrack.Type == eac3to.StreamType.Audio)
                        {
                            iAudioEac3toCount++;
                            string strLanguageEac3To = oTrack.Language;
                            string strLanguageMediaInfo = getLanguage(oInfo.Audio[iAudioCount].Language, oInfo.Audio[iAudioCount].LanguageString, ref infoLog, true, false);
                            while (oInfo.Audio.Count > iAudioCount && !strLanguageMediaInfo.Equals(strLanguageEac3To))
                            {
                                // this workaround works only if there are additional tracks in MediaInfo which are not available in eac3to (already seen in the wild)
                                // it works not when tracks are flipped (not noticed yet)
                                infoLog.LogEvent("Language information does not match. MediaInfo audio track will be removed: " + strLanguageMediaInfo + " <--> " + oTrack.Language, ImageType.Information);
                                oInfo.Audio.RemoveRange(iAudioCount, 1);
                            }

                            if (oInfo.Audio.Count > iAudioCount)
                            {
                                oInfo.Audio[iAudioCount].ID = oTrack.Number.ToString();
                                oInfo.Audio[iAudioCount++].StreamOrder = oTrack.Number.ToString();
                            }
                        }
                        else if (oTrack.Type == eac3to.StreamType.Video && !bVideoFound && !oTrack.Description.Contains("(right eye)"))
                        {
                            oInfo.Video[0].ID = oTrack.Number.ToString();
                            bVideoFound = true;
                        }
                    }

                    oInfo.Audio.RemoveRange(iAudioCount, oInfo.Audio.Count - iAudioCount);
                    if (iAudioEac3toCount != oInfo.Audio.Count)
                        infoLog.LogEvent((iAudioEac3toCount - oInfo.Audio.Count) + " eac3to audio tracks not found!", ImageType.Warning);

                    oInfo.Text.RemoveRange(iTextCount, oInfo.Text.Count - iTextCount);
                    if (iTextEac3toCount != oInfo.Text.Count)
                        infoLog.LogEvent((iTextEac3toCount - oInfo.Text.Count) + " eac3to subtitle tracks not found!", ImageType.Warning);
                }
                else if (oInfo.Audio.Count == 0 && oInfo.Video.Count == 0 && Path.GetExtension(strFile).ToLowerInvariant().Equals(".avs"))
                {
                    // AVS Input File
                    if (infoLog != null)
                        infoLog.Info("AVS input file detected. Getting media information from AviSynth.");

                    using (AvsFile avi = AvsFile.OpenScriptFile(strFile))
                    {
                        MediaInfoWrapper.VideoTrack oVideo = new MediaInfoWrapper.VideoTrack();
                        MediaInfoWrapper.AudioTrack oAudio = new MediaInfoWrapper.AudioTrack();

                        oInfo.General[0].Format = "AVS";
                        oInfo.General[0].FormatString = "AviSynth Script";
                        if (avi.Clip.HasVideo || avi.Clip.HasAudio)
                            oInfo.General[0].PlayTimeString3 = (TimeSpan.FromMilliseconds((avi.VideoInfo.FrameCount / avi.VideoInfo.FPS) * 1000)).ToString();

                        if (avi.Clip.HasVideo)
                        {
                            oVideo.ID = "0";
                            oVideo.Width = avi.VideoInfo.Width.ToString();
                            oVideo.Height = avi.VideoInfo.Height.ToString();
                            oVideo.FrameCount = avi.VideoInfo.FrameCount.ToString();
                            oVideo.FrameRate = avi.VideoInfo.FPS.ToString(culture);  
                            _VideoInfo.FPS_D = avi.VideoInfo.FPS_D;
                            _VideoInfo.FPS_N = avi.VideoInfo.FPS_N;
                            if (avi.Clip.interlaced_frame > 0)
                                oVideo.ScanTypeString = "Interlaced";
                            else
                                oVideo.ScanTypeString = "Progressive";
                            oVideo.Codec = "AVS Video";
                            oVideo.CodecString = "AVS";
                            oVideo.Format = "AVS";
                            _VideoInfo.DAR = avi.VideoInfo.DAR;
                            oVideo.AspectRatio = avi.VideoInfo.DAR.X + ":" + avi.VideoInfo.DAR.Y;
                            oVideo.Delay = "0";
                            oInfo.Video.Add(oVideo);
                        }

                        if (avi.Clip.HasAudio)
                        {
                            oAudio.ID = "0";
                            oAudio.Format = "AVS";
                            oAudio.SamplingRate = avi.Clip.AudioSampleRate.ToString();
                            oAudio.SamplingRateString = avi.Clip.AudioSampleRate.ToString();
                            oAudio.Channels = avi.Clip.ChannelsCount.ToString();
                            oAudio.ChannelsString = avi.Clip.ChannelsCount.ToString() + " channels";
                            oAudio.ChannelPositionsString2 = AudioUtil.getChannelPositionsFromAVSFile(strFile);
                            oAudio.BitRateMode = "CBR";
                            oAudio.Delay = "0";
                            oInfo.Audio.Add(oAudio);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (infoLog == null)
                    infoLog = MainForm.Instance.Log.Info("MediaInfo");
                infoLog.LogValue("Error parsing media file " + strFile, ex.Message, ImageType.Error);
            }
        }

        #region methods
        /// <summary>checks if the input file can be muxed to MKV</summary>
        /// <returns>true if MKV can be created</returns>
        public bool MuxableToMKV()
        {
            if (cType == ContainerType.MKV)
                return true;

            if (_MkvInfo == null)
                _MkvInfo = new MkvInfo(_file, ref _Log);

            return _MkvInfo.IsMuxable;
        }

        /// <summary>checks if the file is indexable by DGIndexNV</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isDGIIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // check if the indexer and the license file are available
            if (!MainForm.Instance.Settings.IsDGIIndexerAvailable())
                return false;

            // only AVC, VC1 and MPEG2 are supported
            if (!_VideoInfo.Track.Codec.ToUpperInvariant().Equals("AVC") &&
                !_VideoInfo.Track.Codec.ToUpperInvariant().Equals("VC-1") &&
                !_VideoInfo.Track.Codec.ToUpperInvariant().Equals("MPEG-2 VIDEO"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpperInvariant().Equals("MATROSKA") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG-TS") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG-PS") &&
                !_strContainer.ToUpperInvariant().Equals("DVD VIDEO") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG VIDEO") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG-4") &&
                !_strContainer.ToUpperInvariant().Equals("VC-1") &&
                !_strContainer.ToUpperInvariant().Equals("AVC") &&
                !_strContainer.ToUpperInvariant().Equals("BDAV") &&
                !_strContainer.ToUpperInvariant().Equals("BLU-RAY PLAYLIST"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by DGIndex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isD2VIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // only MPEG1 and MPEG2 are supported
            if (!_VideoInfo.Track.Codec.ToUpperInvariant().Equals("MPEG-1 VIDEO") &&
                !_VideoInfo.Track.Codec.ToUpperInvariant().Equals("MPEG-2 VIDEO"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpperInvariant().Equals("MPEG-TS") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG-PS") &&
                !_strContainer.ToUpperInvariant().Equals("DVD VIDEO") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG VIDEO") &&
                !_strContainer.ToUpperInvariant().Equals("BDAV"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by DGIndexIM</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isDGMIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // check if the indexer and the license file are available
            if (!MainForm.Instance.Settings.IsDGMIndexerAvailable())
                return false;

            // only AVC, VC1 and MPEG2 are supported
            if (!_VideoInfo.Track.Codec.ToUpperInvariant().Equals("AVC") &&
                !_VideoInfo.Track.Codec.ToUpperInvariant().Equals("VC-1") &&
                !_VideoInfo.Track.Codec.ToUpperInvariant().Equals("MPEG-2 VIDEO"))
                return false;

            // only the following container formats are supported
            if (!_strContainer.ToUpperInvariant().Equals("MATROSKA") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG-TS") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG-PS") &&
                !_strContainer.ToUpperInvariant().Equals("DVD VIDEO") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG VIDEO") &&
                !_strContainer.ToUpperInvariant().Equals("MPEG-4") &&
                !_strContainer.ToUpperInvariant().Equals("VC-1") &&
                !_strContainer.ToUpperInvariant().Equals("AVC") &&
                !_strContainer.ToUpperInvariant().Equals("BDAV") &&
                !_strContainer.ToUpperInvariant().Equals("BLU-RAY PLAYLIST"))
                return false;

            return true;
        }

        /// <summary>checks if the file is indexable by FFMSindex</summary>
        /// <returns>true if indexable, false if not</returns>
        public bool isFFMSIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // interlaced VC-1 is not supported
            if (_VideoInfo.Track.Codec.ToUpperInvariant().Equals("VC-1") &&
                !_VideoInfo.ScanType.ToUpperInvariant().Equals("PROGRESSIVE"))
                return false;

            // some codecs are not supported by FFMS
            if (_VideoInfo.Track.CodecMediaInfo.ToUpperInvariant().Equals("DFSC/VFW")
                || _VideoInfo.Track.CodecMediaInfo.ToUpperInvariant().Equals("CFHD/CINEFORM"))
                return false;

            // only the following container formats are supported
            if (_strContainer.ToUpperInvariant().Equals("MATROSKA") ||
                _strContainer.ToUpperInvariant().Equals("MPEG-TS") ||
                _strContainer.ToUpperInvariant().Equals("MPEG-PS") ||
                _strContainer.ToUpperInvariant().Equals("MPEG VIDEO") ||
                _strContainer.ToUpperInvariant().Equals("MPEG-4") ||
                _strContainer.ToUpperInvariant().Equals("FLASH VIDEO") ||
                _strContainer.ToUpperInvariant().Equals("OGG") ||
                _strContainer.ToUpperInvariant().Equals("WINDOWS MEDIA") ||
                _strContainer.ToUpperInvariant().Equals("AVI") ||
                _strContainer.ToUpperInvariant().Equals("BDAV") ||
                _strContainer.ToUpperInvariant().Equals("BLU-RAY PLAYLIST"))
                return true;

            return false;
        }

        /// <summary>checks if the file is indexable by LSMASH</summary>
        /// <param name="bWriteableDirOnly">if true only writebale input directories are supported</param>
        /// <returns>true if indexable, false if not</returns>
        public bool isLSMASHIndexable(bool bWriteableDirOnly)
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // index file must be created in the input directory
            if (bWriteableDirOnly && !FileUtil.IsDirWriteable(Path.GetDirectoryName(_file)))
                return false;

            // some codecs are not supported by LSMASH
            // VC-1: https://github.com/VFR-maniac/L-SMASH-Works/issues/58
            if (_VideoInfo.Track.Codec.ToUpperInvariant().Equals("VC-1"))
                return false;

            // only the following container formats are supported
            if (_strContainer.ToUpperInvariant().Equals("MATROSKA") ||
                _strContainer.ToUpperInvariant().Equals("MPEG-TS") ||
                _strContainer.ToUpperInvariant().Equals("MPEG-PS") ||
                _strContainer.ToUpperInvariant().Equals("MPEG VIDEO") ||
                _strContainer.ToUpperInvariant().Equals("AVI") ||
                _strContainer.ToUpperInvariant().Equals("MPEG-4") ||
                _strContainer.ToUpperInvariant().Equals("FLASH VIDEO") ||
                _strContainer.ToUpperInvariant().Equals("OGG") ||
                _strContainer.ToUpperInvariant().Equals("WINDOWS MEDIA") ||
                _strContainer.ToUpperInvariant().Equals("BDAV") ||
                _strContainer.ToUpperInvariant().Equals("BLU-RAY PLAYLIST"))
                return true;

            return false;
        }

        /// <summary>
        /// checks if the file is readable by AVISource
        /// </summary>
        /// <param name="bStrictFilesOnly">if true, it returns only true for files which ffms cannot parse</param>
        /// <returns>true if readable, false if not</returns>
        public bool isAVISourceIndexable(bool bStrictFilesOnly)
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            // only the following container format is supported
            if (!_strContainer.ToUpperInvariant().Equals("AVI"))
                return false;

            try
            {
                string tempAvs = "AVISource(\"" + _file + "\", audio=false)";
                IMediaFile iMediaFile = AvsFile.ParseScript(tempAvs);
                if (iMediaFile != null)
                    iMediaFile.Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            // if all AVI files should be processed or only the ones where FFMS cannot handle them
            if (!bStrictFilesOnly)
                return true;

            // some codecs are not supported by FFMS
            if (_VideoInfo.Track.CodecMediaInfo.ToUpperInvariant().Equals("DFSC/VFW") ||
                _VideoInfo.Track.CodecMediaInfo.ToUpperInvariant().Equals("CFHD/CINEFORM"))
                return true;

            return false;
        }

        /// <summary>checks if the file is readable by DirectShowSource/DSS2</summary>
        /// <returns>true if readable, false if not</returns>
        public bool isDirectShowSourceIndexable()
        {
            // check if the file is a video file
            if (!_VideoInfo.HasVideo)
                return false;

            try
            {
                string tempAvs = "DirectShowSource(\"" + _file + "\", audio=false)";
                if (MainForm.Instance.Settings.PortableAviSynth)
                    tempAvs = "LoadPlugin(\"" + Path.Combine(Path.GetDirectoryName(MainForm.Instance.Settings.AviSynth.Path), @"plugins\directshowsource.dll") + "\")\r\n" + tempAvs;
                IMediaFile iMediaFile = AvsFile.ParseScript(tempAvs);
                if (iMediaFile != null)
                    iMediaFile.Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>gets the recommended indexer</summary>
        /// <param name="oType">the recommended indexer</param>
        /// <param name="bWriteableDirOnly">if true only writebale input directories are supported for lsmash</param>
        /// <returns>true if a indexer can be recommended, false if no indexer is available</returns>
        public bool recommendIndexer(out FileIndexerWindow.IndexType oType, bool bWriteableDirOnly)
        {
            if (isDGIIndexable())
                oType = FileIndexerWindow.IndexType.DGI;
            else if (isDGMIndexable())
                oType = FileIndexerWindow.IndexType.DGM;
            else if (isD2VIndexable())
                oType = FileIndexerWindow.IndexType.D2V;
            else if (isAVISourceIndexable(true))
                oType = FileIndexerWindow.IndexType.AVISOURCE;
            else if (isLSMASHIndexable(bWriteableDirOnly))
                oType = FileIndexerWindow.IndexType.LSMASH;
            else if (isFFMSIndexable())
                oType = FileIndexerWindow.IndexType.FFMS;
            else
            {
                oType = FileIndexerWindow.IndexType.NONE;
                _indexerToUse = FileIndexerWindow.IndexType.NONE;
                return false;
            }
            if (_indexerToUse == FileIndexerWindow.IndexType.NONE)
                _indexerToUse = oType;
            return true;
        }

        /// <summary>gets the recommended indexer based on the priority</summary>
        /// <param name="arrIndexer">the indexer priority</param>
        /// <param name="bWriteableDirOnly">if true only writebale input directories are supported for lsmash</param>
        /// <returns>true if a indexer can be recommended, false if no indexer is available</returns>
        public bool recommendIndexer(List<string> arrIndexer, bool bWriteableDirOnly)
        {
            FileIndexerWindow.IndexType oType = FileIndexerWindow.IndexType.NONE;
            foreach (string strIndexer in arrIndexer)
            {
                if (strIndexer.Equals(FileIndexerWindow.IndexType.LSMASH.ToString()))
                {
                    if (isLSMASHIndexable(bWriteableDirOnly))
                    {
                        oType = FileIndexerWindow.IndexType.LSMASH;
                        break;
                    }
                    continue;
                }
                if (strIndexer.Equals(FileIndexerWindow.IndexType.FFMS.ToString()))
                {
                    if (isFFMSIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.FFMS;
                        break;
                    }
                    continue;
                }
                else if (strIndexer.Equals(FileIndexerWindow.IndexType.DGI.ToString()))
                {
                    if (isDGIIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.DGI;
                        break;
                    }
                    continue;
                }
                else if (strIndexer.Equals(FileIndexerWindow.IndexType.DGM.ToString()))
                {
                    if (isDGMIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.DGM;
                        break;
                    }
                    continue;
                }
                else if (strIndexer.Equals(FileIndexerWindow.IndexType.D2V.ToString()))
                {
                    if (isD2VIndexable())
                    {
                        oType = FileIndexerWindow.IndexType.D2V;
                        break;
                    }
                    continue;
                }
                else if (strIndexer.Equals(FileIndexerWindow.IndexType.AVISOURCE.ToString()))
                {
                    if (isAVISourceIndexable(false))
                    {
                        oType = FileIndexerWindow.IndexType.AVISOURCE;
                        break;
                    }
                    continue;
                }
            }

            if (oType != FileIndexerWindow.IndexType.NONE)
            {
                if (_indexerToUse == FileIndexerWindow.IndexType.NONE)
                    _indexerToUse = oType;
                return true;
            }

            return recommendIndexer(out oType, bWriteableDirOnly);
        }

        private FileIndexerWindow.IndexType _indexerToUse;
        /// <summary>gets the recommended indexer</summary>
        public FileIndexerWindow.IndexType IndexerToUse
        {
            get { return _indexerToUse; }
            set { _indexerToUse = value; }
        }

        /// <summary>checks if the file can be demuxed with eac3to</summary>
        /// <returns>true if demuxable, false if not</returns>
        public bool isEac3toDemuxable()
        {
            // only the following container formats are supported
            if (_strContainer.ToUpperInvariant().Equals("BLU-RAY PLAYLIST"))
                return true;

            return false;
        }

        private static int? easyParseInt(string value)
        {
            try
            {
                return (int.Parse(value, culture));
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static double? easyParseDouble(string value)
        {
            try
            {
                return double.Parse(value, culture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static decimal? easyParseDecimal(string value)
        {
            try
            {
                return decimal.Parse(value, culture);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ContainerType getContainerType(string codec, string description)
        {
            if (knownContainerTypes.ContainsKey(codec))
                return knownContainerTypes[codec];
            return null;
        }

        private static AudioCodec getAudioCodec(string description)
        {
            foreach (AudioCodec _codec in CodecManager.AudioCodecs.Values)
            {
                if (string.IsNullOrEmpty(_codec.MediaInfoRegex))
                    continue;

                if (System.Text.RegularExpressions.Regex.IsMatch(description, _codec.MediaInfoRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    return _codec;
            }
            return null;
        }

        private static VideoCodec getVideoCodec(string description)
        {
            description = description.ToLowerInvariant();
            foreach (string knownDescription in knownVideoDescriptions.Keys)
                if (description.Contains(knownDescription.ToLowerInvariant()))
                    return knownVideoDescriptions[knownDescription];
            return null;
        }

        private static VideoType getVideoType(VideoCodec codec, ContainerType cft, string filename)
        {
            string extension = Path.GetExtension(filename).ToLowerInvariant();
            foreach (VideoType t in ContainerManager.VideoTypes.Values)
            {
                if (t.ContainerType == cft && Array.IndexOf<VideoCodec>(t.SupportedCodecs, codec) >= 0 && "." + t.Extension == extension)
                    return t;
            }
            return null;
        }

        private static AudioType getAudioType(AudioCodec codec, ContainerType cft, string filename)
        {
            string extension = Path.GetExtension(filename).ToLowerInvariant();
            foreach (AudioType t in ContainerManager.AudioTypes.Values)
            {
                if (t.ContainerType == cft && Array.IndexOf<AudioCodec>(t.SupportedCodecs, codec) >= 0 && "." + t.Extension == extension)
                    return t;
            }
            return null;
        }
        #endregion
        static MediaInfoFile()
        {
            knownVideoDescriptions = new Dictionary<string, VideoCodec>();

            knownVideoDescriptions.Add("divx 5", VideoCodec.ASP);
            knownVideoDescriptions.Add("divx 4", VideoCodec.ASP);
            knownVideoDescriptions.Add("divx 6", VideoCodec.ASP);
            knownVideoDescriptions.Add("3ivx", VideoCodec.ASP);
            knownVideoDescriptions.Add("xvid", VideoCodec.ASP);
            knownVideoDescriptions.Add("asp", VideoCodec.ASP);
            knownVideoDescriptions.Add("mpeg-4 adv simple", VideoCodec.ASP);
            knownVideoDescriptions.Add("avc", VideoCodec.AVC);
            knownVideoDescriptions.Add("h264", VideoCodec.AVC);
            knownVideoDescriptions.Add("h.264", VideoCodec.AVC);
            knownVideoDescriptions.Add("huffman", VideoCodec.HFYU);
            knownVideoDescriptions.Add("ffvh", VideoCodec.HFYU);
            knownVideoDescriptions.Add("mpeg-4v", VideoCodec.ASP);
            knownVideoDescriptions.Add("vc-1", VideoCodec.VC1);
            knownVideoDescriptions.Add("mpeg-1v", VideoCodec.MPEG1);
            knownVideoDescriptions.Add("V_MPEG1", VideoCodec.MPEG1);
            knownVideoDescriptions.Add("mpeg-2v", VideoCodec.MPEG2);
            knownVideoDescriptions.Add("V_MPEG2", VideoCodec.MPEG2);
            knownVideoDescriptions.Add("hevc", VideoCodec.HEVC);

            knownContainerTypes = new Dictionary<string, ContainerType>();
            knownContainerTypes.Add("AVI", ContainerType.AVI);
            knownContainerTypes.Add("Matroska", ContainerType.MKV);
            knownContainerTypes.Add("MPEG-4", ContainerType.MP4);
            knownContainerTypes.Add("3GPP", ContainerType.MP4);
            knownContainerTypes.Add("BDAV", ContainerType.M2TS);
            knownContainerTypes.Add("MPEG-TS", ContainerType.M2TS);
        }

        #region IMediaFile Members

        public bool HasAudio
        {
            get { return (_AudioInfo.Tracks.Count > 0); }
        }

        public bool HasChapters
        {
            get { return (_ChapterInfo.HasChapters); }
        }

        public bool HasVideo
        {
            get { return _VideoInfo.HasVideo; }
        }

        public bool CanReadVideo
        {
            get { return true; }
        }

        public bool CanReadAudio
        {
            get { return false; }
        }

        public IVideoReader GetVideoReader()
        {
            if (!VideoInfo.HasVideo || !CanReadVideo)
                throw new Exception("Can't read the video stream");
            if (videoSourceFile == null || videoReader == null)
                lock (this)
                {
                    if (videoSourceFile == null)
                    {
                        videoSourceFile = AvsFile.ParseScript(ScriptServer.GetInputLine(_file, null, false,
                        PossibleSources.directShow, false, false, false, VideoInfo.FPS, false), true);
                        videoReader = null;
                    }
                    if (videoReader == null)
                    {
                        videoReader = videoSourceFile.GetVideoReader();
                    }
                }
            return videoReader;
        }

        public IAudioReader GetAudioReader(int track)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string FileName
        {
            get { return _file; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (videoSourceFile != null)
            {
                videoSourceFile.Dispose();
                videoSourceFile = null;
                videoReader = null;
            }
        }

        #endregion
    }

    public class VideoInformation
    {
        public bool HasVideo;
        public ulong Width;
        public ulong Height;
        public Dar DAR;
        public ulong FrameCount;
        public double FPS;
        public int FPS_N;  // online needed for AVS file check
        public int FPS_D;  // online needed for AVS file check
        public int PGCNumber;
        public int PGCCount; // if 0 then no PGC is available (no DVD source), if > 1 then a multi-pgc structure is found
        public int BitDepth;
        public int AngleNumber;
        public int AngleCount; // if 0 then not a multi-angle disc, > 0 multi-angles may be available

        private string _strVideoScanType;
        private VideoCodec _vCodec;
        private VideoType _vType;
        private VideoTrackInfo _videoInfo;

        public VideoInformation(bool hasVideo,
            ulong width, ulong height,
            Dar dar, ulong frameCount,
            double fps, int fps_n,
            int fps_d)
        {
            HasVideo = hasVideo;
            Width = width;
            Height = height;
            DAR = dar;
            FrameCount = frameCount;
            FPS = fps;
            FPS_N = fps_n;
            FPS_D = fps_d;
            PGCCount = 0;
            PGCNumber = 0;
            BitDepth = 8;
            AngleCount = 0;
            AngleNumber = 0;
        }

        public VideoTrackInfo Track
        {
            get { return _videoInfo; }
            set { _videoInfo = value; }
        }

        public string ScanType
        {
            get { return _strVideoScanType; }
            set { _strVideoScanType = value; }
        }

        public VideoType Type
        {
            get { return _vType; }
            set { _vType = value; }
        }

        public VideoCodec Codec
        {
            get { return _vCodec; }
            set { _vCodec = value; }
        }

        public VideoInformation Clone()
        {
            return (VideoInformation)this.MemberwiseClone();
        }
    }

    public class AudioInformation
    {
        private List<AudioTrackInfo> _arrAudioTracks = new List<AudioTrackInfo>();

        public AudioInformation()
        {
            _arrAudioTracks = new List<AudioTrackInfo>();
        }

        public List<AudioTrackInfo> Tracks
        {
            get { return _arrAudioTracks; }
            set { _arrAudioTracks = value; }
        }

        /// <summary>gets the first audio track ID for muxing with mkvmerge</summary>
        /// <returns>trackID or 0</returns>
        public int GetFirstTrackID()
        {
            // check if the file contains an audio track
            if (_arrAudioTracks.Count == 0)
                return 0;

            return _arrAudioTracks[0].MMGTrackID;
        }

        public VideoInformation Clone()
        {
            return (VideoInformation)this.MemberwiseClone();
        }
    }

    public class SubtitleInformation
    {
        private List<SubtitleTrackInfo> _arrSubtitleTracks = new List<SubtitleTrackInfo>();

        public SubtitleInformation()
        {

        }

        public List<SubtitleTrackInfo> Tracks
        {
            get { return _arrSubtitleTracks; }
            set { _arrSubtitleTracks = value; }
        }

        /// <summary>gets the first subtitle track ID for muxing with mkvmerge</summary>
        /// <returns>trackID or 0</returns>
        public int GetFirstTrackID()
        {
            // check if the file is a subtitle file
            if (_arrSubtitleTracks.Count == 0)
                return 0;
            return _arrSubtitleTracks[0].MMGTrackID;
        }

        public VideoInformation Clone()
        {
            return (VideoInformation)this.MemberwiseClone();
        }
    }
}