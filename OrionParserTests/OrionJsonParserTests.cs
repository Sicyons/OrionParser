using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrionCore;

namespace OrionParserTests
{
    [TestClass]
    public class OrionJsonParserTests
    {
        #region Fields
        private const String strTESTFILENAME = "Movies.json";

        private static String strTestsDirectoryPath;
        #endregion

        #region Initializations
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            String strContentDirectoryPath, strSourceDirectoryPath, strContentTestFilepath, strSourceTestFilepath, strTargetDirectoryPath;
            String[] strDirectoryNames;
            Exception xException;

            xException = null;

            OrionJsonParserTests.strTestsDirectoryPath = Path.Combine(OrionDeploymentInfos.DataFolder, "OrionJsonParserTests");

            strContentDirectoryPath = Path.Combine(OrionDeploymentInfos.DataFolder, "Content");
            Assert.IsTrue(Directory.Exists(strContentDirectoryPath), "Class Initialize() method failed;");

            strContentTestFilepath = Path.Combine(strContentDirectoryPath, strTESTFILENAME);
            Assert.IsTrue(File.Exists(strContentTestFilepath), "Class Initialize() method failed;");

            strSourceDirectoryPath = Path.Combine(OrionJsonParserTests.strTestsDirectoryPath, "Source");
            try
            {
                if (Directory.Exists(strSourceDirectoryPath) == false) Directory.CreateDirectory(strSourceDirectoryPath);
            }
            catch (Exception ex)
            {
                xException = ex;
            }
            Assert.IsNull(xException, "Class Initialize() method failed;");

            strSourceTestFilepath = Path.Combine(strSourceDirectoryPath, strTESTFILENAME);
            if (File.Exists(strSourceDirectoryPath) == false)
                try
                {
                    File.Copy(strContentTestFilepath, strSourceTestFilepath);
                }
                catch (Exception ex)
                {
                    xException = ex;
                }
            Assert.IsNull(xException, "Class Initialize() method failed;");

            if (xException == null)
                try
                {
                    strDirectoryNames = new String[] { "Create Parser missing folder" };
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
        [TestInitialize]
        public void InitializeTest()
        {
            Exception xException;

            xException = null;

            Assert.IsNull(xException, "Test Initialize() method failed;");
        }// InitializeTest()
        #endregion

        #region Test methods
        [TestMethod, TestCategory("OrionParserTests")]
        public void Create_Parser_Missing_Folder()
        {
        }// Create_Parser_Missing_Folder()
        #endregion
    }
}