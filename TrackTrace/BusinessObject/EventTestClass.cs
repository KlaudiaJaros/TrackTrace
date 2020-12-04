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
        public void SetId_SetsCorrectId()
        {
            // arrange:
            long id = 111;
            Event eventTest = new Event();

            // act:
            eventTest.ID=id;
            long actualID = eventTest.ID;

            // assert:
            Assert.AreEqual(id, actualID, 0, "ID set method failed.");
        }

        [TestMethod]
        public void GetId_ReturnsCorrectId()
        {
            // arrange:
            long id = 222;
            Event eventTest = new Event();
            eventTest.ID=id;

            // act:
            long actualID = eventTest.ID;

            // assert:
            Assert.AreEqual(id, actualID, 0, "ID get method failed.");
        }



        [TestMethod]
        public void CorrectDateTimeIsHeld()
        {
            // arrange:
            DateTime date = new DateTime(2020, 11, 25, 12, 20, 00);
            Event eventTest = new Event();

            // act:
            eventTest.DateAndTime=date;
            DateTime actualDate = eventTest.DateAndTime;

            // assert:
            Assert.AreEqual(date, actualDate, "DateTime set and/or get failed.");
        }

        [TestMethod]
        public void ConvertToCSVTest()
        {
            // arrange:
            long id = 20;
            DateTime date = new DateTime(2020, 11, 25, 15, 15, 00);
            
            Event eventTest = new Event();
            eventTest.ID=id;
            eventTest.DateAndTime=date;

            string expectedCSV = id.ToString() +"," + date.ToString();

            // act:
            string actualCSV = eventTest.ToCSV();

            // assert:
            Assert.IsTrue(expectedCSV.Equals(actualCSV),"CSV conversion failed.");
        }
    }
}
