﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataProcessorLib;

namespace DataProcessorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvFile = new CsvFile(@"C:\WirelessDialog.csv");
            csvFile.Delete();
        }
    }
}
