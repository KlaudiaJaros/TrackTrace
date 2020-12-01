using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackTrace.BusinessObject
{
    [TestClass]
    class EventTestClass
    {
        [TestMethod]
        public void CorrectIDIsHeld()
        {
            // arrange:
            int id = 1;
            Event eventTest = new Event();

            // act:
            eventTest.SetId(id);
            long actualID = eventTest.GetId();

            // assert:
            Assert.AreEqual(id, actualID, 0, "ID get and set methods failed.");
        }

        [TestMethod]
        public void CorrectDateTimeIsHeld()
        {
            // arrange:
            DateTime date = new DateTime(2020, 11, 25, 12, 20, 00);
            Event eventTest = new Event();

            // act:
            eventTest.SetDateTime(date);
            DateTime actualDate = eventTest.GetDateTime();

            // assert:
            Assert.AreEqual(date, actualDate, "DateTime get and set methods failed.");
        }
        public void ConvertToCSVTest()
        {
            // arrange:
            int id = 20;
            DateTime date = new DateTime(2020, 11, 25, 15, 15, 00);
            Event eventTest = new Event();
            eventTest.SetId(id);
            eventTest.SetDateTime(date);
            string expectedCSV = id.ToString() +"," + date.ToString();

            // act:
            string actualCSV = eventTest.ToCSV();

            // assert:
            Assert.IsTrue(expectedCSV.Equals(actualCSV),"CSV conversion failed.");
        }
    }
}
