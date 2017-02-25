using System;
using System.Text;
using System.Collections.Generic;
using NPoco;


namespace MediaCollection 
{
	public enum MediaType { Unknown = 0, MediaFileAudio, MediaFileVideo, HddvdFolder, DvdFolder, BluRayFolder,HddvdImage, DvdImage, BluRayImage, AudioFolder, Dvd, BlueRay, Cd, CdImage, Picture, PictureFolder };

	[TableName("LOCATION")]
	[PrimaryKey("LOCATION_ID", AutoIncrement = true)]
	public class Location : IComparable<Location>, IModelWithId
	{
		public Location() { }
		public Location(Location src) 
		{
			Id = src.Id;
			TitleId = src.TitleId;
			LocationBaseId = src.LocationBaseId;
			MediaKind = src.MediaKind;
			DateAddedUtc = src.DateAddedUtc;
			DateModifiedUtc = src.DateModifiedUtc;
			LocationData = src.LocationData;
		}

		[Column("LOCATION_ID")]
		public virtual long Id { get; set; }

		[Column("TITLE_ID")]
		public virtual long TitleId { get; set; }

		[Column("LOCATION_BASE_ID")]
		public virtual long LocationBaseId { get; set; }

		[Column("MEDIA_KIND")]
		public virtual MediaType MediaKind { get; set; }

		[Column("DATE_ADDED_UTC")]
		public virtual string DateAddedUtc { get; set; }

		[Column("DATE_MODIFIED_UTC")]
		public virtual string DateModifiedUtc { get; set; }

		[Column("LOCATION_DATA")]
		public virtual string LocationData { get; set; }

		#region IComparable<Location> Members

		public int CompareTo(Location other)
		{
			if (LocationData == null || LocationData == null)
			{
				return (other == null || other.LocationData == null) ? 0 : -1;
			}
			if (other == null || other.LocationData == null) return 1;

			return LocationData.CompareTo(other.LocationData);
		}

		#endregion

		public virtual void Delete()
		{
			using(var db = DB.GetDatabase())
			{
				db.Delete(this);
			}
		}
	}

	/// <summary>
	/// Location with location base
	/// </summary>
	public class LocationForDisplay : Location
	{
		[Column("LOCATION_KIND")]
		public virtual LocationBaseKind LocationKind { get; set; }
		[Column("LOCATION")]
		public virtual string LocationBase { get; set; }

		public override string ToString()
		{
			return string.Format("{0}({1}) {1}>{2}", LocationKind, MediaKind, LocationBase, LocationData);
		}
	}
}
