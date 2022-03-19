using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Acturis.Test.Question6.vA
{
    class RecordAndHisWorkers
    {
        bool _isTheHighest;
        string _myId;
        string _bossId;
        List<string> _workers;
        string[] _recordData;
        public RecordAndHisWorkers(string[] rData)
        {
            _recordData = rData;
            _myId = rData[0];
            _bossId = rData[1];
            _workers = new List<string>();
            _isTheHighest = false;
        }
        public void AddWorker(string newWorker)
        {
            _workers.Add(newWorker);
        }

        public void Print()
        {
            Console.WriteLine($"{_recordData[2]} {_recordData[3]} {_recordData[4]} {_recordData[6]}");
        }

        public void PrintAllChildren(Dictionary<string, RecordAndHisWorkers> elementsDir, int level)
        {
            _workers.Sort(myCompareIds);
            foreach (var keyOfWorker in _workers)
            {
                int restoreLevel = level;
                for (int i = 0; i < level; i++)
                {
                    Console.Write("  ");
                }
                Console.Write(" ->");
                elementsDir[keyOfWorker].Print();
                level += 1;
                elementsDir[keyOfWorker].PrintAllChildren(elementsDir, level);
                level = restoreLevel;
            }
        }

        public bool IsTheHighest
        {
            get => _isTheHighest;
            set => _isTheHighest = value;
        }

        public string BossId
        {
            get => _bossId;
            set => _bossId = value;
        }

        public string Id
        {
            get => _myId;
            set => _myId = value;
        }

        public string[] RecordData
        {
            get => _recordData;
            set => _recordData = value;
        }

        public List<string> Workers
        {
            get => _workers;
            set => _workers = value;
        }

        private static int myCompareIds(string x, string y)
        {
            int valX = Int32.Parse(x);
            int valY = Int32.Parse(y);
            if (valX < valY)
            {
                return -1;
            }
            else if (valX > valY)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    class Program
    {
        private static int myComparRecordAndHisWorkers(RecordAndHisWorkers x, RecordAndHisWorkers y)
        {
            int valX = Int32.Parse(x.Id);
            int valY = Int32.Parse(y.Id);
            if (valX < valY)
            {
                return -1;
            }
            else if (valX > valY)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        static void printOrganogram(Dictionary<string, RecordAndHisWorkers> elementsDir)
        {
            List<RecordAndHisWorkers> highestOnes = new List<RecordAndHisWorkers>();
            foreach (var item in elementsDir)
            {
                if (item.Value.IsTheHighest)
                {
                    highestOnes.Add(item.Value);        
                }
            }
            highestOnes.Sort(myComparRecordAndHisWorkers);
            foreach(var item in highestOnes)
            {
                item.Print();
                item.PrintAllChildren(elementsDir, 0);
                Console.Write("\n");
            }
        }

        static void Main(string[] args)
        {
            Dictionary<string, RecordAndHisWorkers> allWrokersDir = new Dictionary<string, RecordAndHisWorkers>();
            using (var reader = new System.IO.StreamReader("companies_data.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    allWrokersDir.Add(values[0], new RecordAndHisWorkers(values));
                }
                foreach (var record in allWrokersDir)
                {
                    RecordAndHisWorkers tempBoss;
                    if (allWrokersDir.TryGetValue(record.Value.BossId, out tempBoss))
                    {
                        tempBoss.AddWorker(record.Value.Id);
                    }
                    else
                    {
                        record.Value.IsTheHighest = true;
                    }
                }
            }

            printOrganogram(allWrokersDir);

            Console.ReadKey();
        }
    }
}
