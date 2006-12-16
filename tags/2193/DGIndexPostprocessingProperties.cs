using System.Xml.Serialization;
using System;

namespace MeGUI
{
	/// <summary>
	/// Summary description for DGIndexPostprocessingProperties.
	/// </summary>
	public class DGIndexPostprocessingProperties
	{
		private bool autoDeriveAR, signalAR, autoDeint;
		private int horizontalOutputResolution, splitSize;
        private ContainerFileType container;
		private long outputSize;
		private AspectRatio ar;
		private VideoCodecSettings videoSettings;
        private AviSynthSettings avsSettings;
        private OneClickWindow.PartialAudioStream[] audioStreams;
		private double customAR;
		private string chapterFile, finalOutput, aviSynthScript;

		public DGIndexPostprocessingProperties()
		{
			autoDeriveAR = false;
			signalAR = false;
			ar = AspectRatio.A1x1;
            audioStreams = new OneClickWindow.PartialAudioStream[2];
            avsSettings = new AviSynthSettings();
			horizontalOutputResolution = 640;
			customAR = 1.0;
			container = ContainerFileType.MKV;
			outputSize = 734003200; // 700 mb
			splitSize = 0;
		}
        public OneClickWindow.PartialAudioStream[] AudioStreams
        {
            get { return audioStreams; }
            set { audioStreams = value; }
        }
        public bool AutoDeinterlace
        {
            get { return autoDeint; }
            set { autoDeint = value; }
        }
        /// <summary>
		/// gets / sets whether the aspect ratio should be derived from the dgindex project
		/// </summary>
		public bool AutoDeriveAR
		{
			get {return autoDeriveAR;}
			set {autoDeriveAR = value;}
		}
		/// <summary>
		/// gets / sets whether the aspect ratio should be signalled in the output and thus
		/// resizing should not unstretch anamorphically stretched content
		/// </summary>
		public bool SignalAR
		{
			get {return signalAR;}
			set {signalAR = value;}
		}
		/// <summary>
		/// gets / sets the horizontal output resolution the output should have
		/// </summary>
		public int HorizontalOutputResolution
		{
			get {return horizontalOutputResolution;}
			set {horizontalOutputResolution = value;}
		}
		/// <summary>
		/// gets / sets the container of the output
		/// </summary>
		[XmlIgnore]
        public ContainerFileType Container
		{
			get {return container;}
			set {container = value;}
		}

        public ContainerType ContainerType
        {
            get { return Container.ContainerType; }
            set
            {
                foreach (ContainerFileType f in new MuxProvider().GetSupportedContainers())
                {
                    if (f.ContainerType == value)
                    {
                        Container = f;
                        return;
                    }
                }
            }
        }

        
        /// <summary>
		/// gets / sets the output size
		/// </summary>
		public long OutputSize
		{
			get {return outputSize;}
			set {outputSize = value;}
		}
		/// <summary>
		/// gets / sets the split size for the output
		/// </summary>
		public int SplitSize
		{
			get {return splitSize;}
			set {splitSize = value;}
		}
		/// <summary>
		/// gets / sets the aspect ratio of the video input (if known)
		/// </summary>
		public AspectRatio AR
		{
			get {return ar;}
			set {ar = value;}
		}
        public AviSynthSettings AvsSettings
        {
            get { return avsSettings; }
            set { avsSettings = value; }
        }
		/// <summary>
		/// gets / sets the video codec settings used for video encoding
		/// </summary>
		public VideoCodecSettings VideoSettings
		{
			get {return videoSettings;}
			set {videoSettings = value;}
		}
		/// <summary>
		/// gets / sets a custom aspect ratio for the input
		/// (requires AR set to AspectRatio.Custom to be taken into account)
		/// </summary>
		public double CustomAR
		{
			get {return customAR;}
			set {customAR = value;}
		}
		/// <summary>
		/// gets / sets the chapter file for the output
		/// </summary>
		public string ChapterFile
		{
			get {return chapterFile;}
			set {chapterFile = value;}
		}
		/// <summary>
		/// gets / sets the path and name of the final output file
		/// </summary>
		public string FinalOutput
		{
			get {return finalOutput;}
			set {finalOutput = value;}
		}
		/// <summary>
		/// gets / sets the path and name of the AviSynth script created from the dgindex project
		/// this is filled in during postprocessing
		/// </summary>
		public string AviSynthScript
		{
			get {return aviSynthScript;}
			set {aviSynthScript = value;}
		}
	}
}