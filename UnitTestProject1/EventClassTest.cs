using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TrackTrace.BusinessObject;

namespace UnitTestProject1
{
    [TestClass]
    public class EventClassTest
    {
        [TestMethod]
        public void SetId_SetsCorrectId()
        {
            // arrange:
            long id = 111;
            Event eventTest = new Event();

            // act:
            eventTest.ID = id;
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
            eventTest.ID = id;

            // act:
            long actualID = eventTest.ID;

            // assert:
            Assert.AreEqual(id, actualID, 0, "ID get method failed.");
        }

        [TestMethod]
        public void SetDT_SetsCorrectDateTime()
        {
            // arrange:
            DateTime date = new DateTime(2020, 11, 25, 12, 20, 00);
            Event eventTest = new Event();

            // act:
            eventTest.DateAndTime = date;
            DateTime actualDate = eventTest.DateAndTime;

            // assert:
            Assert.AreEqual(date, actualDate, "DateTime set method failed.");
        }

        [TestMethod]
        public void GetDT_GetsCorrectDateTime()
        {
            // arrange:
            DateTime date = new DateTime(2020, 11, 25, 12, 20, 00);
            Event eventTest = new Event();
            eventTest.DateAndTime = date;

            // act:
            DateTime actualDate = eventTest.DateAndTime;

            // assert:
            Assert.AreEqual(date, actualDate, "DateTime get method failed.");
        }

        [TestMethod]
        public void ToCSV_ConvertsCorrectly()
        {
            // arrange:
            long id = 20;
            DateTime date = new DateTime(2020, 11, 25, 15, 15, 00);

            Event eventTest = new Event();
            eventTest.ID = id;
            eventTest.DateAndTime = date;

            string expectedCSV = id.ToString() + "," + date.ToString();

            // act:
            string actualCSV = eventTest.ToCSV();

            // assert:
            Assert.IsTrue(expectedCSV.Equals(actualCSV), "CSV conversion failed.");
        }
    }
}
