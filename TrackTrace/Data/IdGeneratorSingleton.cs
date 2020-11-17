using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.Data
{
    class IdGeneratorSingleton
    {
        private static IdGeneratorSingleton instance;
        private static int id = 0;

        private IdGeneratorSingleton() { }

        public static IdGeneratorSingleton IdGenerator
        {
            get
            {
                if (instance == null)
                {
                    instance = new IdGeneratorSingleton();
                }
                return instance;
            }        
        }
        public int GenerateID()
        {
            id++;
            return id;
        }
    }
}
