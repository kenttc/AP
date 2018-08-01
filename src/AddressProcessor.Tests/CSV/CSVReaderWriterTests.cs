using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AddressProcessing.CSV;
namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private const string TestInputFile = @"..\..\test_data\contacts.csv";
        private const string TestWriteInputFile = @"..\..\test_data\writecontacts.csv";


        [Test]
        public void CSVReaderWriter_Open_when_mode_is_read_and_file_is_valid_will_return_ReadStreamValid()
        {
            //arrange
            var sut = new CSVReaderWriter();
            //act
            sut.Open(TestInputFile, CSVReaderWriter.Mode.Read);

            //assert
            Assert.That(sut.ReadStreamValid,"Unable to open text file" );

            //clean up 
            sut.Close();
        }

        [Test]
        public void CSVReaderWriter_Open_when_mode_is_write_will_return_WriteStreamValid()
        {
            //arrange
            var sut = new CSVReaderWriter();
            //act
            sut.Open(TestWriteInputFile, CSVReaderWriter.Mode.Write);

            //assert
            Assert.That(sut.WriteStreamValid, "Unable to open text file");

            //clean up 
            sut.Close();
        }


    }
}
