using System;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService
    {
        private readonly ISelectInvoiceService _selectInvoiceService;
        private readonly IConfidentialInvoiceService _confidentialInvoiceService;
        private readonly IBusinessLoansService _businessLoansService;

        public ProductApplicationService(ISelectInvoiceService selectInvoiceService, IConfidentialInvoiceService confidentialInvoiceWebService, IBusinessLoansService businessLoansService)
        {
            _selectInvoiceService = selectInvoiceService;
            _confidentialInvoiceService = confidentialInvoiceWebService;
            _businessLoansService = businessLoansService;
        }

        public int SubmitApplicationFor(ISellerApplication application)
        {
            int finalResult = 0;
            try
            {
                switch (application.Product)
                {
                    case SelectiveInvoiceDiscount selectiveInvoiceDiscount:
                        {
                            if (_selectInvoiceService == null)
                                throw new ArgumentNullException("InvoiceDiscount Service is null");

                            finalResult = _selectInvoiceService.SubmitApplicationFor(application.CompanyData.Number.ToString(), selectiveInvoiceDiscount.InvoiceAmount, selectiveInvoiceDiscount.AdvancePercentage);
                            break;
                        }
                    case ConfidentialInvoiceDiscount confidentialInvoiceDiscount:
                        {
                            if (_confidentialInvoiceService == null)
                                throw new ArgumentNullException("ConfidentialInvoice Service is null");

                            var result = _confidentialInvoiceService.SubmitApplicationFor(ConvertToCompanyRequest(application.CompanyData), confidentialInvoiceDiscount.TotalLedgerNetworth, confidentialInvoiceDiscount.AdvancePercentage, confidentialInvoiceDiscount.VatRate);
                            finalResult = (result.Success) ? result.ApplicationId ?? -1 : -1;
                            break;
                        }
                    case BusinessLoans businessLoans:
                        {
                            if (_businessLoansService == null)
                                throw new ArgumentNullException("InvoiceDiscount Service is null");

                            var result = _businessLoansService.SubmitApplicationFor(ConvertToCompanyRequest(application.CompanyData), ConvertToLoansRequest(businessLoans));
                            finalResult = (result.Success) ? result.ApplicationId ?? -1 : -1;
                            break;
                        }
                }
            }
            catch (Exception)
            {
                throw;
            }

             return finalResult;

        }

        /// <summary>
        /// Convert from BusinessLoans to LoansRequest
        /// </summary>
        /// <param name="businessLoans"></param>
        /// <returns></returns>
        private LoansRequest ConvertToLoansRequest(BusinessLoans businessLoans)
        {
            return new LoansRequest
            {
                InterestRatePerAnnum = businessLoans.InterestRatePerAnnum,
                LoanAmount = businessLoans.LoanAmount
            };
        }

        /// <summary>
        /// Convert from SellerCompanyData to CompanyDataRequest
        /// </summary>
        /// <param name="companyData"></param>
        /// <returns></returns>
        private CompanyDataRequest ConvertToCompanyRequest(ISellerCompanyData companyData)
        {
            return new CompanyDataRequest
            {
                CompanyFounded = companyData.Founded,
                CompanyNumber = companyData.Number,
                CompanyName = companyData.Name,
                DirectorName = companyData.DirectorName
            };
        }
    }
}