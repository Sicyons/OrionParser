using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrionCore.ErrorManagement;
using OrionParser.Json;
using OrionCore;

namespace OrionParserTests
{
    [TestClass]
    public class OrionJsonParserTests
    {
        #region Fields
        private const String strTESTFILENAME = "Movies.json";

        private static String strSourceDirectoryPath, strTestsDirectoryPath;
        #endregion

        #region Initializations
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            String strContentDirectoryPath, strContentTestFilepath, strSourceTestFilepath, strTargetDirectoryPath;
            String[] strDirectoryNames;
            Exception xException;

            xException = null;

            OrionJsonParserTests.strTestsDirectoryPath = Path.Combine(OrionDeploymentInfos.DataFolder, "OrionJsonParserTests");

            strContentDirectoryPath = Path.Combine(OrionDeploymentInfos.DataFolder, "Content");
            Assert.IsTrue(Directory.Exists(strContentDirectoryPath), "Class Initialize() method failed;");

            strContentTestFilepath = Path.Combine(strContentDirectoryPath, strTESTFILENAME);
            Assert.IsTrue(File.Exists(strContentTestFilepath), "Class Initialize() method failed;");

            OrionJsonParserTests.strSourceDirectoryPath = Path.Combine(OrionJsonParserTests.strTestsDirectoryPath, "Source");
            try
            {
                if (Directory.Exists(OrionJsonParserTests.strSourceDirectoryPath) == false) Directory.CreateDirectory(OrionJsonParserTests.strSourceDirectoryPath);
            }
            catch (Exception ex)
            {
                xException = ex;
            }
            Assert.IsNull(xException, "Class Initialize() method failed;");

            strSourceTestFilepath = Path.Combine(OrionJsonParserTests.strSourceDirectoryPath, strTESTFILENAME);
            if (File.Exists(OrionJsonParserTests.strSourceDirectoryPath) == false)
                try
                {
                    File.Copy(strContentTestFilepath, strSourceTestFilepath, true);
                }
                catch (Exception ex)
                {
                    xException = ex;
                }
            Assert.IsNull(xException, "Class Initialize() method failed;");

            if (xException == null)
                try
                {
                    strDirectoryNames = new String[] { "Create Parser Missing Source String", "Create Parser Ok" };
                    foreach (String strDirectoryNameTemp in strDirectoryNames)
                    {
                        strTargetDirectoryPath = Path.Combine(OrionJsonParserTests.strTestsDirectoryPath, strDirectoryNameTemp);
                        if (Directory.Exists(strTargetDirectoryPath) == false) Directory.CreateDirectory(strTargetDirectoryPath);
                    }
                }
                catch (Exception ex)
                {
                    xException = ex;
                }

            Assert.IsNull(xException, "Class Initialize() method failed;");
        }// Initialize()
        #endregion

        #region Test methods
        [TestMethod, TestCategory("OrionParserTests")]
        public void Create_Parser_Missing_Source_String()
        {
            OrionException xOrionException;
            OrionJsonObject xJsonObject;

            xOrionException = null;

            try
            {
                xJsonObject = new OrionJsonObject(null);
            }
            catch (OrionException ex)
            {
                xOrionException = ex;
            }
            Assert.IsNotNull(xOrionException);
            Assert.AreEqual(xOrionException.Message, "Source Json string can't be null;");
        }// Create_Parser_Missing_Source_String()
        [TestMethod, TestCategory("OrionParserTests")]
        public void Create_Parser_Ok()
        {
            String strJsonContent, strJsonFilePath, strTestDirectoryPath;
            Exception xException;
            OrionException xOrionException;
            OrionJsonObject xJsonObject;
            List<Object> xObjectArray, xObjectArray2;

            strJsonContent = null;
            strTestDirectoryPath = Path.Combine(OrionJsonParserTests.strTestsDirectoryPath, "Create Parser Ok");
            strJsonFilePath = Path.Combine(strTestDirectoryPath, OrionJsonParserTests.strTESTFILENAME);
            xException = null;
            xOrionException = null;
            xJsonObject = null;

            try
            {
                File.Copy(Path.Combine(OrionJsonParserTests.strSourceDirectoryPath, OrionJsonParserTests.strTESTFILENAME), strJsonFilePath, true);
            }
            catch (Exception ex)
            {
                xException = ex;
            }
            Assert.IsNull(xException);
            Assert.IsTrue(File.Exists(strJsonFilePath));

            try
            {
                strJsonContent = File.ReadAllText(strJsonFilePath,System.Text.Encoding.UTF8);
            }
            catch (OrionException ex)
            {
                xOrionException = ex;
            }
            Assert.IsNull(xOrionException);
            Assert.IsFalse(String.IsNullOrWhiteSpace(strJsonContent));

            try
            {
                xJsonObject = new OrionJsonObject(strJsonContent);
            }
            catch (OrionException ex)
            {
                xOrionException = ex;
            }
            Assert.IsNull(xOrionException);
            Assert.AreEqual(xJsonObject.Count, 4);

            this.CheckValue(xJsonObject, "page", 1L);
            this.CheckValue(xJsonObject, "total_results", 1758L);
            this.CheckValue(xJsonObject, "total_pages", 88L);
            this.CheckList(xJsonObject, "results", 20);
            xObjectArray = xJsonObject["results"] as List<Object>;

            xJsonObject = (OrionJsonObject)xObjectArray[0];
            this.CheckValue(xJsonObject, "vote_count", 3300L);
            this.CheckValue(xJsonObject, "id", 539L);
            this.CheckValue(xJsonObject, "video", false);
            this.CheckValue(xJsonObject, "vote_average", 8.3);
            this.CheckValue(xJsonObject, "title", "Psycho");
            this.CheckValue(xJsonObject, "popularity", 35.20885);
            this.CheckValue(xJsonObject, "poster_path", "\\/81d8oyEFgj7FlxJqSDXWr8JH8kV.jpg");
            this.CheckValue(xJsonObject, "original_language", "en");
            this.CheckValue(xJsonObject, "original_title", "Psycho");
            this.CheckList(xJsonObject, "genre_ids", 3);
            xObjectArray2 = xJsonObject["genre_ids"] as List<Object>;
            Assert.AreEqual(xObjectArray2[0], 18L);
            Assert.AreEqual(xObjectArray2[1], 27L);
            Assert.AreEqual(xObjectArray2[2], 53L);
            this.CheckValue(xJsonObject, "backdrop_path", "\\/3md49VBCeqY6MSNyAVY6d5eC6bA.jpg");
            this.CheckValue(xJsonObject, "adult", false);
            this.CheckValue(xJsonObject, "overview", "When larcenous real estate clerk Marion Crane goes on the lam with a wad of cash and hopes of starting a new life, she ends up at the notorious Bates Motel, where manager Norman Bates cares for his housebound mother. The place seems quirky, but fine… until Marion decides to take a shower.");
            this.CheckValue(xJsonObject, "release_date", "1960-06-16");

            xJsonObject = (OrionJsonObject)xObjectArray[1];
            this.CheckValue(xJsonObject, "vote_count", 1632L);
            this.CheckValue(xJsonObject, "id", 426L);
            this.CheckValue(xJsonObject, "video", false);
            this.CheckValue(xJsonObject, "vote_average", 8.1);
            this.CheckValue(xJsonObject, "title", "Vertigo");
            this.CheckValue(xJsonObject, "popularity", 29.347118);
            this.CheckValue(xJsonObject, "poster_path", "\\/obhM86qyv8RsE69XSMTtT9FdE0b.jpg");
            this.CheckValue(xJsonObject, "original_language", "en");
            this.CheckValue(xJsonObject, "original_title", "Vertigo");
            this.CheckList(xJsonObject, "genre_ids", 3);
            xObjectArray2 = xJsonObject["genre_ids"] as List<Object>;
            Assert.AreEqual(xObjectArray2[0], 9648L);
            Assert.AreEqual(xObjectArray2[1], 10749L);
            Assert.AreEqual(xObjectArray2[2], 53L);
            this.CheckValue(xJsonObject, "backdrop_path", "\\/sNlCvWbAEYZZGlPIOdiQc4y1X7d.jpg");
            this.CheckValue(xJsonObject, "adult", false);
            this.CheckValue(xJsonObject, "overview", "A retired San Francisco detective suffering from acrophobia investigates the strange activities of an old friend's wife, all the while becoming dangerously obsessed with her.");
            this.CheckValue(xJsonObject, "release_date", "1958-05-09");

        }// Create_Parser_Ok()
        #endregion

        #region Utility procedures
        private void CheckList(OrionJsonObject sourceObject, String itemName, Int32 itemCount)
        {
            Assert.IsTrue(sourceObject.ContainsKey(itemName));
            Assert.IsInstanceOfType(sourceObject[itemName], typeof(List<Object>));
            Assert.AreEqual(((List<Object>)sourceObject[itemName]).Count, itemCount);
        }// CheckList()
        private void CheckValue(OrionJsonObject sourceObject, String itemName, Object itemValue)
        {
            Assert.IsTrue(sourceObject.ContainsKey(itemName));
            Assert.AreEqual(sourceObject[itemName], itemValue);
        }// CheckValue()
        #endregion
    }
}