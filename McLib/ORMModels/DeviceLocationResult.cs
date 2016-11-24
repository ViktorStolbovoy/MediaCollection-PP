using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco;

namespace MediaCollection
{
	public class DeviceLocationResult
	{
		[Column("DEVICE_DATA")]
		public string DeviceData {get; set;}
		[Column("LOCATION_MAPPING")]
		public string LocationMapping {get; set;}
		[Column("LOCATION_DATA")]
		public string LocationData {get; set;}
		[Column("MEDIA_KIND")]
		public MediaType MediaKind { get; set; }
		[Column("DEVICE_KIND")]
		public DeviceType DeviceKind { get; set; }


		public void Run()
		{ 
			switch(DeviceKind)
			{
				case DeviceType.Dune: 
					ExecHttpCommand(DuneCommand());
					break;
				case DeviceType.PC: 
					ExecPcCommand(PcCommand());
					return;
				default: throw new NotSupportedException(string.Format("Device type {0} is not supported", DeviceKind));
			}
		}

		private string DuneCommand()
		{
			if (string.IsNullOrWhiteSpace(DeviceData)) return null;
			//192.168.0.7/cgi-bin/do?cmd=start_bluray_playback&media_url=smb://rd:p@192.168.0.3/F/BIG
			//http://192.168.0.7/cgi-bin/do?cmd=start_playlist_playback&media_url=smb://rd:p@192.168.0.3/Music/Mike%20Oldfield/Tr3s%20Lunas%20Cd1

			string locationParam = Uri.EscapeDataString(LocationMapping + LocationData.Replace('\\', '/'));

			switch(MediaKind)
			{
				case MediaType.PictureFolder:
				case MediaType.AudioFolder: return string.Format("http://{0}/cgi-bin/do?cmd=start_playlist_playback&media_url={1}", DeviceData, locationParam);
				case MediaType.Picture:
				case MediaType.CdImage:
				case MediaType.MediaFileVideo:
				case MediaType.MediaFileAudio: return string.Format("http://{0}/cgi-bin/do?cmd=start_file_playback&media_url={1}", DeviceData, locationParam);
				case MediaType.DvdImage:
				case MediaType.DvdFolder: return string.Format("http://{0}/cgi-bin/do?cmd=start_dvd_playback&media_url={1}", DeviceData, locationParam);

				case MediaType.BluRayFolder:
				case MediaType.HddvdFolder:
				case MediaType.BluRayImage:
				case MediaType.HddvdImage: return string.Format("http://{0}/cgi-bin/do?cmd=start_bluray_playback&media_url={1}", DeviceData, locationParam);

				default: throw new NotSupportedException(string.Format("Media type {0} is not supported by Dune", MediaKind));
			}
		}

		private string PcCommand()
		{
			//DeviceData is ignored
			return System.IO.Path.Combine(LocationMapping, LocationData);
		}

		public void ExecPcCommand(string command)
		{
			if(string.IsNullOrWhiteSpace(DeviceData))
			{
				System.Diagnostics.Process.Start(command);
			}
			else
			{
				System.Diagnostics.Process.Start(DeviceData, command);
			}
		}


		public void ExecHttpCommand(string command)
		{
			//TODO: Add error handling and cancellation
			using (var cts = new System.Threading.CancellationTokenSource())
			{
				HttpHelper.MakeHttpRequest(command, cts.Token).Wait(); //Make it synchronous 
			}
		}
	}
}
