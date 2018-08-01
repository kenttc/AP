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
        private const string TestInputFile = @"..\test_data\contacts.csv";


        [Test]
        public void Should_send_mail_using_mailshot_service()
        {

            var sut = new CSVReaderWriter();
            sut.Open(TestInputFile, CSVReaderWriter.Mode.Read);


            Assert.That(sut.ReadStream,"Unable to open text file" );
        }

    }
}
