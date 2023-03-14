using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLiteLibrary.Models
{
    public class GridSpotModel
    {
        public string GridSpotletter { get; set; }

        public int GridSpotNumber { get; set;}

        public GridSpotStatus Status { get; set; } = GridSpotStatus.Empty;
    }
}