using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class AI
    {
        public bool turn;
        Map nMap;
        UnitManager um;
        public AI(ref Map _nMap, ref UnitManager _um)
        {
            nMap = _nMap;
            um = _um;
            turn = false;
        }
        public void Update()
        {
            if (!turn) return;

            turn = false;
        }
    }
}
