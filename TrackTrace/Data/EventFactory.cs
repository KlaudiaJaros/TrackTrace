using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackTrace.BusinessObject;

namespace TrackTrace.Data
{
    class EventFactory
    {
        public Event CreateEvent(char code)
        {
            if (code == 'V')
            {
                return new Visit();
            }
            else if (code == 'C')
            {
                return new Contact();
            }
            else
                return null;
        } 
    }
}
