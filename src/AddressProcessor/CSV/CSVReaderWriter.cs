using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.

        at the moment the class is small enough to stay as it is, 
        but if the method grows to do other things, i would expect this class to be refactored to 2 different classes , 
        1 for reading and 1 for writing , 
        while there can be an abstraction can be done to split them but if this is not needed as this is easier to maintain 
        then i think we can stick with this for now as refactoring can never end.  
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
        /// <summary>
        /// googled to stack overflow to find out which is faster. 
        /// https://stackoverflow.com/questions/585860/string-join-vs-stringbuilder-which-is-faster
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public string ColToString(params string[] columns)
        {
            return string.Join(WRITE_SEPARATOR, columns);
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
            if(columns?.Length <= 1)
            {
                return false;
            }

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
                        _readerStream.Dispose();
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
