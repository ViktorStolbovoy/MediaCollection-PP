using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaCollection;

namespace MCLibTests
{
	[TestClass]
	public class TestFileSearch
	{
		[TestMethod]
		public void TestDirectoryWithSeriesProcesing1()
		{
			var di = new System.IO.DirectoryInfo("c:\\temp\\test_title_s2_d3");
			var si = new StoredItem( di, MediaType.BluRayFolder, "c:\\temp", 11);
			
			
			Assert.AreEqual("Test Title (S2D3)", si.Title);
			Assert.AreEqual(2, si.Season);
			Assert.AreEqual(3, si.Disk);
		}

		[TestMethod]
		public void TestDirectoryWithSeriesProcesing2()
		{
			var di = new System.IO.DirectoryInfo("c:\\temp\\test title s2 d3");
			var si = new StoredItem(di, MediaType.BluRayFolder, "c:\\temp", 11);


			Assert.AreEqual("Test Title (S2D3)", si.Title);
			Assert.AreEqual(2, si.Season);
			Assert.AreEqual(3, si.Disk);
		}

		[TestMethod]
		public void TestDirectoryWithSeriesProcesing3()
		{
			var di = new System.IO.DirectoryInfo("c:\\temp\\test title s23 d31");
			var si = new StoredItem(di, MediaType.BluRayFolder, "c:\\temp", 11);


			Assert.AreEqual("Test Title (S23D31)", si.Title);
			Assert.AreEqual(23, si.Season);
			Assert.AreEqual(31, si.Disk);
		}

		[TestMethod]
		public void TestDirectory1()
		{
			var di = new System.IO.DirectoryInfo("c:\\temp\\test title");
			var si = new StoredItem(di, MediaType.BluRayFolder, "c:\\temp", 11);


			Assert.AreEqual("Test Title", si.Title);
			Assert.AreEqual(0, si.Season);
			Assert.AreEqual(0, si.Disk);
		}
	}
}
