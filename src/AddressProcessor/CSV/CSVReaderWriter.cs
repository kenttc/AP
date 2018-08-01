using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter : IDisposable
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        private char[] _separator = { '\t' };

        const string WRITE_SEPARATOR = "\t";

        const int FIRST_COLUMN = 0;
        const int SECOND_COLUMN = 1;


     
        /// <summary>
        /// added this to make the code testable 
        /// as the point in time there's no figure out if the file exists or not
        /// </summary>
        public bool ReadStreamValid => _readerStream != null;

        /// <summary>
        /// added this to make the code testable - might be removed later. 
        /// as the point in time there's no figure out if the file exists or not
        /// </summary>
        public bool WriteStreamValid => _writerStream != null;


        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    _readerStream = File.OpenText(fileName);
                    break;
                case Mode.Write:
                    FileInfo fileInfo = new FileInfo(fileName);
                    _writerStream = fileInfo.CreateText();
                    break;
                default:
                    //technically will never fire unless the enum changes without notice
                    throw new Exception("Unknown file mode for " + fileName);
                    
            }
  
        }

        public void Write(params string[] columns)
        {
            WriteLine(ColToString(columns));
        }

        public string ColToString(params string[] columns)
        {
            string outPut = "";
            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += WRITE_SEPARATOR;
                }
            }
            return outPut;
        }

        public bool Read(out string column1, out string column2)
        {

            column1 = null;
            column2 = null;

            string line;
            string[] columns;

            

            line = ReadLine();

            if (line == null)
            {
                return false;
            }
            
            columns = line.Split(_separator);

            if(string.IsNullOrEmpty(columns[FIRST_COLUMN]) || string.IsNullOrEmpty(columns[SECOND_COLUMN]))
            {
                return false;
            }

            column1 = columns[FIRST_COLUMN];
            column2 = columns[SECOND_COLUMN];

            return true;
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            this.Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                    //
                    if (_readerStream != null)
                    {
                        _readerStream.Close();
                    }

                    if (_writerStream != null)
                    {
                        _writerStream.Dispose();
                    }
                        
                }

                
                _readerStream = null;
                _writerStream = null;

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
