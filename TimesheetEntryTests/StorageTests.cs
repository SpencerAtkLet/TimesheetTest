using CMap_Tech_Test.src.Storage;
using CMap_Tech_Test.src.Timesheet;

namespace TimesheetEntryTests
{
    [TestClass]
    public sealed class StorageTests
    {
        [TestMethod]
        public void AddEntry_AddsNewEntry_ReturnsTrue()
        {
            var userID = Guid.NewGuid();
            var projectID = Guid.NewGuid();
            var date = new DateOnly(2025, 07, 22);
            var timesheet = new Timesheet(userID, projectID, date, 8, "");
            var storage = new Storage();
            storage.AddEntry(timesheet);

            Assert.IsTrue(storage.entries.ContainsKey((userID, date)));
        }

        [TestMethod]
        public void RemoveEntry_RemovesExistingEntry_ReturnsTrue()
        {
            var userID = Guid.NewGuid();
            var projectID = Guid.NewGuid();
            var date = new DateOnly(2025, 07, 22);
            var timesheet = new Timesheet(userID, projectID, date, 8, "");
            var storage = new Storage();
            storage.AddEntry(timesheet);

            var result = storage.RemoveEntry(userID, date);

            Assert.IsTrue(result);
            Assert.IsFalse(storage.entries.ContainsKey((userID, date)));
        }

        [TestMethod]
        public void RemoveEntry_NonExistingEntry_ReturnsFalse()
        {
            var storage = new Storage();
            var result = storage.RemoveEntry(Guid.NewGuid(), new DateOnly(2025, 07, 22));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EditEntry_UpdatesExistingEntry_ReturnsTrue()
        {
            var userID = Guid.NewGuid();
            var projectID = Guid.NewGuid();
            var date = new DateOnly(2025, 07, 22);
            var timesheet = new Timesheet(userID, projectID, date, 8, "");
            var storage = new Storage();
            storage.AddEntry(timesheet);

            var updatedTimesheet = new Timesheet(userID, projectID, date, 10, "Updated");
            var result = storage.EditEntry(updatedTimesheet, projectID);

            Assert.IsTrue(result);
            Assert.AreEqual(10, storage.entries[(userID, date)][0].hoursWorked);
            Assert.AreEqual("Updated", storage.entries[(userID, date)][0].description);
        }

        [TestMethod]
        public void EditEntry_NonExistingEntry_ReturnsFalse()
        {
            var userID = Guid.NewGuid();
            var projectID = Guid.NewGuid();
            var date = new DateOnly(2025, 07, 22);
            var timesheet = new Timesheet(userID, projectID, date, 8, "");
            var storage = new Storage();

            var result = storage.EditEntry(timesheet, projectID);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ListEntries_ReturnsEntriesForWeek()
        {
            var userID = Guid.NewGuid();
            var projectID = Guid.NewGuid();
            var weekStart = new DateOnly(2025, 07, 21); // Monday
            var storage = new Storage();

            for (int i = 0; i < 5; i++)
            {
                var date = weekStart.AddDays(i);
                var timesheet = new Timesheet(userID, projectID, date, 8, $"Day {i}");
                storage.AddEntry(timesheet);
            }

            var entries = storage.ListEntries(userID, weekStart);

            Assert.AreEqual(5, entries.Count);
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual($"Day {i}", entries[i].description);
            }
        }

        [TestMethod]
        public void TotalHours_ReturnsSumOfHoursForWeek()
        {
            var userID = Guid.NewGuid();
            var projectID = Guid.NewGuid();
            var weekStart = new DateOnly(2025, 07, 21); // Monday
            var storage = new Storage();

            for (int i = 0; i < 5; i++)
            {
                var date = weekStart.AddDays(i);
                var timesheet = new Timesheet(userID, projectID, date, 8, "");
                storage.AddEntry(timesheet);
            }

            var total = storage.TotalHours(userID, weekStart);

            Assert.AreEqual(40, total);
        }
    }
}
