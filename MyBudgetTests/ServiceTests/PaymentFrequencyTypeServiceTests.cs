using Moq;
using MyBudget.Models;
using MyBudget.Services;

namespace MyBudgetTests.ServiceTests
{
    public class PaymentFrequencyTypeServiceTests
    {
        [Fact]
        public async void GetList_Test()
        {
            var paymentFrequencyTypeService = new Mock<ITypeService<PaymentFrequencyTypes>>();
            paymentFrequencyTypeService.Setup(p => p.GetList()).ReturnsAsync(FakeList);
            var service = paymentFrequencyTypeService.Object;

            var result = await service.GetList();

            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }

        [Fact]
        public async void GetById_Test()
        {
            int testId = 1;
            PaymentFrequencyTypes? testType = FakeList().FirstOrDefault(p => p.PaymentFrequencyTypeId == testId);

            var paymentFrequencyTypeService = new Mock<ITypeService<PaymentFrequencyTypes>>();
            paymentFrequencyTypeService.Setup(p => p.GetById(testId)).ReturnsAsync(testType);
            var service = paymentFrequencyTypeService.Object;

            var result = await service.GetById(testId);

            Assert.Equal("test1", result.PaymentFrequencyType);
        }

        [Fact]
        public async void CreateRecord_Test()
        {
            var newType = FakeNewType();

            var paymentFrequencyTypeService = new Mock<ITypeService<PaymentFrequencyTypes>>();
            paymentFrequencyTypeService.Setup(p => p.CreateRecord(newType)).ReturnsAsync(newType);
            var service = paymentFrequencyTypeService.Object;

            var result = await service.CreateRecord(newType);

            Assert.Equal(newType.PaymentFrequencyTypeId, result.PaymentFrequencyTypeId);
            Assert.Equal(newType.PaymentFrequencyType, result.PaymentFrequencyType);
        }

        [Fact]
        public async void UpdateRecord_Test() 
        {
            var listTypes = FakeList();
            var updatedType = FakeUpdatedType();

            var paymentFrequencyTypeService = new Mock<ITypeService<PaymentFrequencyTypes>>();
            paymentFrequencyTypeService.Setup(p => p.UpdateRecord(updatedType)).ReturnsAsync(updatedType);
            var service = paymentFrequencyTypeService.Object;

            var result = await service.UpdateRecord(updatedType);

            Assert.True(result.PaymentFrequencyType != "test1");
            Assert.True(result.PaymentFrequencyType == "updated");
            Assert.Equal(updatedType.PaymentFrequencyTypeId, result.PaymentFrequencyTypeId);
        }

        // PRIVATE METHODS

        private List<PaymentFrequencyTypes> FakeList()
        {
            PaymentFrequencyTypes type1 = new() { PaymentFrequencyTypeId = 1, PaymentFrequencyType = "test1" };
            PaymentFrequencyTypes type2 = new() { PaymentFrequencyTypeId = 2, PaymentFrequencyType = "test2" };

            List<PaymentFrequencyTypes> list = new()
            {
                type1,
                type2
            };

            return list;
        }

        private PaymentFrequencyTypes FakeNewType()
        {
            return new() { PaymentFrequencyTypeId = 3, PaymentFrequencyType = "test3" };
        }

        private PaymentFrequencyTypes FakeUpdatedType()
        {
            var typeList = FakeList();
            var type = typeList[0];
            type.PaymentFrequencyType = "updated";
            return type;
        }
    }
}
