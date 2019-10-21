using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using SlothEnterprise.ProductApplication.Tests.TempServices;
using System;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests : IDisposable
    {
        SelectInvoiceService _selectInvoiceService = new SelectInvoiceService(); //removed readonly as we will do a test to catch null poiter execption
        ConfidentialInvoiceService _confidentialInvoiceWebService = new ConfidentialInvoiceService(); //removed readonly as we will do a test to catch null poiter execption
        BusinessLoansService _businessLoansService = new BusinessLoansService(); //removed readonly as we will do a test to catch null poiter execption
        SellerCompanyData _companyData = null;
        SellerApplication _sellerApplication = null;
        ProductApplicationService _applicationService;

        /// <summary>
        /// Setup the properties for the test
        /// </summary>
        public ProductApplicationTests()
        {
            _companyData = new SellerCompanyData() { DirectorName = "Dr No", Founded = new DateTime(2008, 12, 25), Name = "No One", Number = 666 };
            _sellerApplication = new SellerApplication();
            _applicationService = new ProductApplicationService(_selectInvoiceService, _confidentialInvoiceWebService, _businessLoansService);

        }
  
        private void SetupSellProduct(IProduct product)
        {
            _sellerApplication.CompanyData = _companyData;
            _sellerApplication.Product = product;
        }

        [Fact]
        public void TestSelectiveInvoiceDiscountTest()
        {
            SelectiveInvoiceDiscount selectiveInvoiceDiscount = new SelectiveInvoiceDiscount() { Id = 101, InvoiceAmount = 100 };
            SetupSellProduct(selectiveInvoiceDiscount);
            var result = _applicationService.SubmitApplicationFor(_sellerApplication);
            Assert.Equal(1, result);
        }

        [Fact]
        public void TestBusinessLoans()
        {
            BusinessLoans businessLoans = new BusinessLoans() { Id = 102, InterestRatePerAnnum= 0.9m, LoanAmount = 100000.00m};
            SetupSellProduct(businessLoans);
            var result = _applicationService.SubmitApplicationFor(_sellerApplication);
            Assert.Equal(_companyData.Number, result);
        }

        [Fact]
        public void TestConfidentialInvoiceDiscount()
        {
            ConfidentialInvoiceDiscount confidentialInvoiceDiscount = new ConfidentialInvoiceDiscount() { Id = 103, AdvancePercentage = 0.5m, TotalLedgerNetworth = 10000.00m, VatRate = 0.14m };
            SetupSellProduct(confidentialInvoiceDiscount);
            var result = _applicationService.SubmitApplicationFor(_sellerApplication);

            Assert.Equal(_companyData.Number, result);
        }


        [Fact]
        public void TestBusinessLoans_WithInvalidArguments_ThrowsArgumentNullException()
        {
            _applicationService = null;
            //arrange
            _businessLoansService = null;
            _applicationService = new ProductApplicationService(_selectInvoiceService, _confidentialInvoiceWebService, _businessLoansService);

            BusinessLoans businessLoans = new BusinessLoans() { Id = 102, InterestRatePerAnnum = 0.9m, LoanAmount = 100000.00m };
            SetupSellProduct(businessLoans);

            // act & assert
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _applicationService.SubmitApplicationFor(_sellerApplication));
            Assert.Equal("InvoiceDiscount Service is null", ex.ParamName);
        }

        [Fact]
        public void TestSelectiveInvoiceDiscountTest_WithInvalidArguments_ThrowsArgumentNullException()
        {
            _applicationService = null;
            //arrange
            _selectInvoiceService = null;
            _applicationService = new ProductApplicationService(_selectInvoiceService, _confidentialInvoiceWebService, _businessLoansService);

            SelectiveInvoiceDiscount selectiveInvoiceDiscount = new SelectiveInvoiceDiscount() { Id = 101, InvoiceAmount = 100 };
            SetupSellProduct(selectiveInvoiceDiscount);

            // act & assert
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _applicationService.SubmitApplicationFor(_sellerApplication));
            Assert.Equal("InvoiceDiscount Service is null", ex.ParamName);
        }


        [Fact]
        public void TestConfidentialInvoiceDiscount_WithInvalidArguments_ThrowsArgumentNullException()
        {
            _applicationService = null;
            //arrange
            _confidentialInvoiceWebService = null;
            _applicationService = new ProductApplicationService(_selectInvoiceService, _confidentialInvoiceWebService, _businessLoansService);

            ConfidentialInvoiceDiscount confidentialInvoiceDiscount = new ConfidentialInvoiceDiscount() { Id = 103, AdvancePercentage = 0.5m, TotalLedgerNetworth = 10000.00m, VatRate = 0.14m };
            SetupSellProduct(confidentialInvoiceDiscount);

            // act & assert
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _applicationService.SubmitApplicationFor(_sellerApplication));
            Assert.Equal("ConfidentialInvoice Service is null", ex.ParamName);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _selectInvoiceService = null;
                    _confidentialInvoiceWebService = null;
                    _businessLoansService = null;
                    _companyData = null;
                    _sellerApplication = null;
                    _applicationService = null;
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
