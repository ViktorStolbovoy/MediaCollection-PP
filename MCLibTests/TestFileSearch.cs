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
		public void TestDirectoryWithSeriesProcesing4()
		{
			var di = new System.IO.DirectoryInfo("c:\\temp\\DEXTER_S6D3");
			var si = new StoredItem(di, MediaType.BluRayFolder, "c:\\temp", 11);


			Assert.AreEqual("Dexter (S6D3)", si.Title);
			Assert.AreEqual(6, si.Season);
			Assert.AreEqual(3, si.Disk);
		}

		[TestMethod]
		public void TestDirectoryWithSeriesProcesing5()
		{
			var di = new System.IO.DirectoryInfo("c:\\temp\\DEXTER_S7_D1");
			var si = new StoredItem(di, MediaType.BluRayFolder, "c:\\temp", 11);


			Assert.AreEqual("Dexter (S7D1)", si.Title);
			Assert.AreEqual(7, si.Season);
			Assert.AreEqual(1, si.Disk);
		}

		[TestMethod]
		public void TestDirectoryWithSeriesProcesing6()
		{
			var di = new System.IO.DirectoryInfo("c:\\temp\\V Season 1 Disc 2");
			var si = new StoredItem(di, MediaType.BluRayFolder, "c:\\temp", 11);

			Assert.AreEqual("V (S1D2)", si.Title);
			Assert.AreEqual(1, si.Season);
			Assert.AreEqual(2, si.Disk);
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
