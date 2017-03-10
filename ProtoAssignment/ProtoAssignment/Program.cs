using System;
using System.IO;
using System.Text;

namespace ProtoAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            new Mps7().Start();
        }
    }

    public class Mps7
    {
        #region private vars
        private string _mainframe;
        private string _version;
        private int _numRecords;
        #endregion private vars

        #region properties
        public double TotalDebits { get; set; } = 0.0;
        public double TotalCredits { get; set; } = 0.0;
        public int TotalAutopayStarted { get; set; } = 0;
        public int TotalAutopayEnded { get; set; } = 0;
        public double SpecialUserBalance { get; set; } = 0;
        public enum RecordType
        {
            Debit = 0,
            Credit = 1,
            StartAutopay = 2,
            EndAutopay = 3
        }
        #endregion properties

        #region parsing
        public void Start()
        {
            using (BinaryReaderBigEndian bigEndianReader = new BinaryReaderBigEndian(File.Open("txnlog.dat", FileMode.Open)))
            {
                _mainframe = Encoding.Default.GetString(bigEndianReader.ReadBytes(4));
                _version = "v" + BitConverter.ToString(bigEndianReader.ReadBytes(1));
                _numRecords = (int)bigEndianReader.ReadConvertToUInt32(4, 0);

                for (int i = 0; i <= _numRecords; i++)
                {

                    RecordType recType = bigEndianReader.ConvertRecType(1);
                    var timeStamp = (int)bigEndianReader.ReadConvertToUInt32(4, 0);
                    var userId = (long)bigEndianReader.ReadConvertToUInt64(8, 0);
                    bool isSpecialUser = userId == 2456938384156277127;
                    if (isSpecialUser)
                    {
                        var bla = "";
                    }
                    double cash;
                    switch (recType)
                    {
                        case RecordType.Debit:
                            cash = bigEndianReader.ReadDollars(8, 0);
                            TotalDebits += cash;
                            SpecialUserBalance += isSpecialUser ? cash : 0.0;
                            break;
                        case RecordType.Credit:
                            cash = bigEndianReader.ReadDollars(8, 0);
                            TotalCredits += cash;
                            SpecialUserBalance += isSpecialUser ? cash : 0.0;
                            break;
                        case RecordType.StartAutopay:
                            TotalAutopayStarted++;
                            break;
                        case RecordType.EndAutopay:
                            TotalAutopayEnded++;
                            break;
                    }
                }
            }
            #endregion parsing

            Console.WriteLine(_mainframe + " " + _version + " Number of Records: " + _numRecords);
            Console.WriteLine();
            Console.WriteLine("* What is the total amount in dollars of debits?");
            Console.WriteLine("========>   " + TotalDebits.ToString("C"));
            Console.WriteLine();
            Console.WriteLine("* What is the total amount in dollars of credits?");
            Console.WriteLine("========>   " + TotalCredits.ToString("C"));
            Console.WriteLine();
            Console.WriteLine("* How many autopays were started?");
            Console.WriteLine("========>   " + TotalAutopayStarted);
            Console.WriteLine();
            Console.WriteLine("* How many autopays were ended?");
            Console.WriteLine("========>   " + TotalAutopayEnded);
            Console.WriteLine("* What is balance of user ID 2456938384156277127?");
            Console.WriteLine("========>   " + SpecialUserBalance.ToString("C"));
            Console.ReadLine();
        }
    }
}
