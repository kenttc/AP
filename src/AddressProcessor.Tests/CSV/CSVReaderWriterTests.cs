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


        [Test]
        public void CSVReaderWriter_Open_when_mode_is_read_and_file_is_valid_will_return_ReadStreamValid()
        {

            var sut = new CSVReaderWriter();
            sut.Open(TestInputFile, CSVReaderWriter.Mode.Read);


            Assert.That(sut.ReadStreamValid,"Unable to open text file" );
        }

    }
}
