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
        private const string TestEmptyInputFile = @"..\..\test_data\empty.csv";
        private const string TestEmptyWithTabsInputFile = @"..\..\test_data\emptyWithTabs.csv";
 
        /// <summary>
        /// I am doing this because i try not to touch / refactor - code before there are test in place to ensure all i don't break existing functionality. 
        /// but i added some code as it's new functionality to allow me testing capabilities
        /// this is what i learnt from working with legacy code - michael c feathers
        /// </summary>
        [Test]
        public void Open_when_mode_is_read_and_file_is_valid_will_return_ReadStreamValid()
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
        public void Open_when_mode_is_write_will_return_WriteStreamValid()
        {
            //arrange
            var sut = new CSVReaderWriter();
            //act
            sut.Open(TestWriteInputFile, CSVReaderWriter.Mode.Write);

            //assert
            Assert.That(sut.WriteStreamValid, "Unable to open text file");

            //clean up 
            sut.Close();
            //this is not ideal.. but will work for now since i can't refactor File out due to backwards compatibility.
            if(System.IO.File.Exists(TestWriteInputFile))System.IO.File.Delete(TestWriteInputFile);
        }

        // ok i wanted to write tests for Write but it seems that the addressFileProcessorTest will cover the write since it accurately  has the number of contacts to be written



        [Test]
        public void Read_when_file_is_empty_return_false_and_out_values_null()
        {
            //arrange
            var sut = new CSVReaderWriter();
            string col1Out = "";
            string col2Out = "";
            //act
            sut.Open(TestEmptyInputFile, CSVReaderWriter.Mode.Read);
            var result = sut.Read(out col1Out, out col2Out);

            //assert
            Assert.That(result, Is.False);
            Assert.That(col1Out, Is.Null);
            Assert.That(col2Out, Is.Null);


            //clean up 
            sut.Close();

        }


        /// <summary>
        /// this will test edge cases when file has tabs but nothing in the column as an enhancement?
        /// </summary>
        [Test]
        public void Read_when_file_column_is_empty_return_false_and_out_values_null()
        {
            //arrange
            var sut = new CSVReaderWriter();
            string col1Out = "";
            string col2Out = "";
            //act
            sut.Open(TestEmptyWithTabsInputFile, CSVReaderWriter.Mode.Read);
            var result = sut.Read(out col1Out, out col2Out);

            //assert
            Assert.That(result, Is.False);
            Assert.That(col1Out, Is.Null);
            Assert.That(col2Out, Is.Null);

            //clean up 
            sut.Close();
            
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Read_when_file_column_is_valid_return_true_and_outValues_as_expected()
        {
            //arrange
            var sut = new CSVReaderWriter();
            string col1Out = "";
            string col2Out = "";
            //act
            sut.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            var result = sut.Read(out col1Out, out col2Out);

            //assert
            Assert.That(result, Is.True);
            Assert.That(col1Out, Is.EqualTo("Shelby Macias"));
            Assert.That(col2Out, Is.EqualTo("3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England"));

            //clean up 
            sut.Close();

        }


    }
}
