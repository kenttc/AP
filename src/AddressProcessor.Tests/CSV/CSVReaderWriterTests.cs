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
        private const string TestOtherFile = @"..\..\test_data\NotabsString.csv";
        
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

        /// <summary>
        /// needed to do this to make the code testable - tradeoff here is i had to make this public as the write to line is at the moment not testable unless i create an interface for File and use moq to verify the right calls have been made while writing the line
        /// </summary>
        [Test]
        public void Cols_to_string_should_end_with_tab()
        {
            //arrange
            var sut = new CSVReaderWriter();
            string[] cols = { "col1", "col2"};
            
            //act
            sut.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            var result = sut.ColToString(cols);


            //assert
            Assert.That(result, Is.EqualTo("col1\tcol2"));

            //clean up 
            sut.Close();

        }
        /// <summary>
        ///ok i missed this test for closing the file / executing the close via the dispose. 
        /// </summary>
        [Test]
        public void Close_when_open_with_write_should_set_Write_stream_to_null()
        {
            //arrange
            var sut = new CSVReaderWriter();
            
            //act
            sut.Open(TestWriteInputFile, CSVReaderWriter.Mode.Write);
            sut.Close();


            //assert
            Assert.That(sut.WriteStreamValid, Is.False);

            //clean up 
            //this is not ideal.. but will work for now since i can't refactor File out due to backwards compatibility.
            if (System.IO.File.Exists(TestWriteInputFile)) System.IO.File.Delete(TestWriteInputFile);

        }
        /// <summary>
        /// and for the read - ok i missed this test for closing the file / executing the close via the dispose. 
        /// </summary>
        [Test]
        public void Close_when_open_with_read_should_set_read_stream_to_null()
        {
            //arrange
            var sut = new CSVReaderWriter();
            

            //act
            sut.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            sut.Close();


            //assert
            Assert.That(sut.ReadStreamValid, Is.False);

            

        }

        /// <summary>
        /// no tabs string file
        /// </summary>
        [Test]
        public void Read_EdgeCase_Open_read_no_tabs_file()
        {
            //arrange
            var sut = new CSVReaderWriter();
            string column1 = "";
            string column2 = "";

            //act
            sut.Open(TestOtherFile, CSVReaderWriter.Mode.Read);
            sut.Read(out column1, out column2);
            sut.Close();

            //assert
            Assert.That(sut.ReadStreamValid, Is.False);



        }

    }
}
