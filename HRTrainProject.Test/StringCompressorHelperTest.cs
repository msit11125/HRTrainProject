using HRTrainProject.Core.Models;
using HRTrainProject.DAL;
using HRTrainProject.Services.Interfaces;
using HRTrainProject.Services;
using HRTrainProject.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace HRTrainProject.Test
{
    [TestClass]
    public class StringCompressorHelperTest
    {
        [TestMethod]
        public void StringCompressTest()
        {
            string long_value = @"<div>abcdefghijklmnopqrstuvwxyz0123456789
                                  abcdefghijklmnopqrstuvwxyz0123456789
                                  abcdefghijklmnopqrstuvwxyz0123456789
                                  abcdefghijklmnopqrstuvwxyz0123456789
                                  abcdefghijklmnopqrstuvwxyz0123456789
                                  abcdefghijklmnopqrstuvwxyz0123456789</div>";

            string long_value_compresss = StringCompressorHelper.CompressString(long_value);
            string long_value_decompresss = StringCompressorHelper.DecompressString(long_value_compresss);


            Assert.IsTrue(long_value_compresss.Length < long_value.Length);

            Assert.AreEqual(long_value, long_value_decompresss);
        }
    }
}
