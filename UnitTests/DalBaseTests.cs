namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests
{
    using Data.Entity.Testing.Moq;
    using TestObjects;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class DalBaseTests : TestDbContextBase<DateRangeEffectiveDtoObject, int>
    {
        #region Test Data Set
        private static readonly IQueryable<DateRangeEffectiveDtoObject> TestDtoObjects = new[]
        {
            new DateRangeEffectiveDtoObject
            {
                Id = StaticTestValues.ValidDateRangeEffectiveDtoObjectId1,
                Name = StaticTestValues.ValidName1,
                Description = StaticTestValues.ValidDescription1,
                EffectiveStartDate = StaticTestValues.EffectiveStartDateTimeOffset1,
                EffectiveEndDate = StaticTestValues.EffectiveEndDateTimeOffset1,
                CreatedByUserId = StaticTestValues.CreatedByUserId1,
                CreatedOn = StaticTestValues.CreatedOnDateTimeOffset1,
                LastUpdatedByUserId = StaticTestValues.LastUpdatedByUserId1,
                LastUpdatedOn = StaticTestValues.LastUpdatedOnDateTimeOffset1
            },
            new DateRangeEffectiveDtoObject
            {
                Id = StaticTestValues.ValidDateRangeEffectiveDtoObjectId2,
                Name = StaticTestValues.ValidName2,
                Description = StaticTestValues.ValidDescription2
            }
        }.AsQueryable();
        #endregion

        public DalBaseTests() : base(TestDtoObjects)
        {
        }

        #region Create Tests
        [TestMethod]
        [TestCategory("DbContextBase Create")]
        public async Task Given_ATestDtoObject_When_InsertIsInvoked_Then_AddMethodShouldBeInvokedOnlyOnce()
        {
            await DbContextBase.Create(new DateRangeEffectiveDtoObject());

            MockDbSet.Verify(m => m.Add(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Once());
        }

        [TestMethod]
        [TestCategory("DbContextBase Create")]
        public async Task Given_ATestDtoObject_When_InsertIsInvoked_Then_SaveChangesAsyncMethodShouldBeInvokedOnlyOnce()
        {
            await DbContextBase.Create(new DateRangeEffectiveDtoObject());

            MockDbContext.Verify(m => m.SaveChangesAsync(), Times.Once());
        }
        #endregion

        #region Read Tests
        [TestMethod]
        [TestCategory("DbContextBase Read")]
        public async Task Given_ATestDtoObjectId_When_GetIsInvoked_Then_ATestDtoObjectShouldBeReturned()
        {
            var testDtoObject = await DbContextBase.Read(StaticTestValues.ValidDateRangeEffectiveDtoObjectId1);

            Assert.AreEqual(StaticTestValues.ValidDateRangeEffectiveDtoObjectId1, testDtoObject.Id);
        }

        [TestMethod]
        [TestCategory("DbContextBase Read")]
        public async Task Given_AnInvalidTestDtoObjectId_When_GetIsInvoked_Then_AnObjectNotFoundExceptionShouldBeThrown()
        {
            try
            {
                await DbContextBase.Read(StaticTestValues.InvalidDateRangeEffectiveDtoObjectId1);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(KeyNotFoundException));
            }
        }
        #endregion

        #region Update Tests
        [TestMethod]
        [TestCategory("DbContextBase Update")]
        public async Task Given_AValidTestDtoObjectWithoutChanges_When_UpdateIsInvoked_Then_SaveChangesAsyncMethodShouldNotBeInvoked()
        {
            var testObjectWithoutUpdates = TestDtoObjects.First();

            await DbContextBase.Update(testObjectWithoutUpdates);

            MockDbContext.Verify(m => m.SaveChangesAsync(), Times.Never);
        }

        [TestMethod]
        [TestCategory("DbContextBase Update")]
        public async Task Given_AValidTestDtoObjectWithoutChanges_When_UpdateIsInvoked_Then_LastUpdateOnValueShouldNotHaveChanged()
        {
            var testObjectWithoutUpdates = TestDtoObjects.First();

            var returnedObject = await DbContextBase.Update(testObjectWithoutUpdates);

            Assert.AreEqual(testObjectWithoutUpdates.LastUpdatedOn, returnedObject.LastUpdatedOn);
        }

        [TestMethod]
        [TestCategory("DbContextBase Update")]
        public async Task Given_AValidTestDtoObjectWithChanges_When_UpdateIsInvoked_Then_SaveChangesAsyncMethodShouldOnlyBeInvokedOnce()
        {
            var testObjectWithUpdates = new DateRangeEffectiveDtoObject
            {
                Id = StaticTestValues.ValidDateRangeEffectiveDtoObjectId1,
                Name = StaticTestValues.ValidName2,
                Description = StaticTestValues.ValidDescription2,
                EffectiveStartDate = StaticTestValues.EffectiveStartDateTimeOffset2,
                EffectiveEndDate = StaticTestValues.EffectiveEndDateTimeOffset2,
                CreatedByUserId = StaticTestValues.CreatedByUserId2,
                CreatedOn = StaticTestValues.CreatedOnDateTimeOffset2,
                LastUpdatedByUserId = StaticTestValues.LastUpdatedByUserId2,
                LastUpdatedOn = StaticTestValues.LastUpdatedOnDateTimeOffset2
            };

            await DbContextBase.Update(testObjectWithUpdates);

            MockDbContext.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        [TestCategory("DbContextBase Update")]
        public async Task Given_AValidTestDtoObjectWithChanges_When_UpdateIsInvoked_Then_ValuesExpectedToUpdateShouldBeUpdated()
        {
            var testObjectWithUpdates = new DateRangeEffectiveDtoObject
            {
                Id = StaticTestValues.ValidDateRangeEffectiveDtoObjectId1,
                Name = StaticTestValues.ValidName2,
                Description = StaticTestValues.ValidDescription2,
                EffectiveStartDate = StaticTestValues.EffectiveStartDateTimeOffset2,
                EffectiveEndDate = StaticTestValues.EffectiveEndDateTimeOffset2,
                CreatedByUserId = StaticTestValues.CreatedByUserId2,
                CreatedOn = StaticTestValues.CreatedOnDateTimeOffset2,
                LastUpdatedByUserId = StaticTestValues.LastUpdatedByUserId2,
                LastUpdatedOn = StaticTestValues.LastUpdatedOnDateTimeOffset2
            };

            var returnedObject = await DbContextBase.Update(testObjectWithUpdates);

            Assert.AreEqual(StaticTestValues.ValidName2, returnedObject.Name);
            Assert.AreEqual(StaticTestValues.EffectiveStartDateTimeOffset2, returnedObject.EffectiveStartDate);
            Assert.AreEqual(StaticTestValues.EffectiveEndDateTimeOffset2, returnedObject.EffectiveEndDate);
            Assert.AreEqual(StaticTestValues.LastUpdatedByUserId2, returnedObject.LastUpdatedByUserId);

            Assert.AreNotEqual(StaticTestValues.LastUpdatedOnDateTimeOffset1, returnedObject.LastUpdatedOn);
            Assert.AreNotEqual(StaticTestValues.LastUpdatedOnDateTimeOffset2, returnedObject.LastUpdatedOn);
            Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < returnedObject.LastUpdatedOn && returnedObject.LastUpdatedOn < DateTimeOffset.UtcNow.AddSeconds(1));
        }

        [TestMethod]
        [TestCategory("DbContextBase Update")]
        public async Task Given_AValidTestDtoObjectWithChanges_When_UpdateIsInvoked_Then_ValuesNotExpectedToUpdateShouldNotBeUpdated()
        {
            var testObjectWithUpdates = new DateRangeEffectiveDtoObject
            {
                Id = StaticTestValues.ValidDateRangeEffectiveDtoObjectId1,
                Name = StaticTestValues.ValidName2,
                Description = StaticTestValues.ValidDescription2,
                EffectiveStartDate = StaticTestValues.EffectiveStartDateTimeOffset2,
                EffectiveEndDate = StaticTestValues.EffectiveEndDateTimeOffset2,
                CreatedByUserId = StaticTestValues.CreatedByUserId2,
                CreatedOn = StaticTestValues.CreatedOnDateTimeOffset2,
                LastUpdatedByUserId = StaticTestValues.LastUpdatedByUserId2,
                LastUpdatedOn = StaticTestValues.LastUpdatedOnDateTimeOffset2
            };

            var returnedObject = await DbContextBase.Update(testObjectWithUpdates);

            Assert.AreEqual(StaticTestValues.ValidDateRangeEffectiveDtoObjectId1, returnedObject.Id);
            Assert.AreEqual(StaticTestValues.ValidDescription1, returnedObject.Description);
            Assert.AreEqual(StaticTestValues.CreatedByUserId1, returnedObject.CreatedByUserId);
            Assert.AreEqual(StaticTestValues.CreatedOnDateTimeOffset1, returnedObject.CreatedOn);
        }
        #endregion

        #region Delete Tests
        [TestMethod]
        [TestCategory("DbContextBase Delete")]
        public async Task Given_AValidTestDtoObjectId_When_DeleteIsInvoked_Then_RemoveMethodShouldOnlyBeInvokedOnce()
        {
            await DbContextBase.Delete(StaticTestValues.ValidDateRangeEffectiveDtoObjectId1);

            MockDbSet.Verify(m => m.Remove(It.Is<DateRangeEffectiveDtoObject>(s => s.Id == StaticTestValues.ValidDateRangeEffectiveDtoObjectId1)), Times.Once());
        }

        [TestMethod]
        [TestCategory("DbContextBase Delete")]
        public async Task Given_AValidTestDtoObjectId_When_DeleteIsInvoked_Then_SaveChangesAsyncMethodShouldOnlyBeInvokedOnce()
        {
            await DbContextBase.Delete(StaticTestValues.ValidDateRangeEffectiveDtoObjectId1);

            MockDbContext.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        [TestCategory("DbContextBase Delete")]
        public async Task Given_AnInvalidTestDtoObjectId_When_DeleteIsInvoked_Then_AnObjectNotFoundExceptionShouldBeThrown()
        {
            try
            {
                await DbContextBase.Delete(StaticTestValues.ValidDateRangeEffectiveDtoObjectId1);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(KeyNotFoundException));
            }
        }
        #endregion
    }
}
