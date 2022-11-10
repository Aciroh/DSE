using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Data
    {
        //Execution
        public int superscalarFactor { get; set; } = 1; 
        public int numberOfRenameEntities { get; set; } = 1;
        public int ReorderEntries { get; set; } = 1;

        //General
        public int NumberOfReservationStationsPerBuffer { get; set; } = 1;
        public int NumberOfIntegerExecutionUnits { get; set; } = 1;
        public int NumberOfFloatingPointExecutionUnits { get; set; } = 1;
        public int NumberOfBranchExecutionUnits { get; set; } = 1;
		public int NumberOfMemoryExecutionUnits { get; set; } = 1;



    }
}
