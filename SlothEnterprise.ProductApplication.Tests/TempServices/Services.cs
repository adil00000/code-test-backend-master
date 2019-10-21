using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlothEnterprise.ProductApplication.Tests.TempServices
{
    public class SelectInvoiceService : ISelectInvoiceService
    {
        public int SubmitApplicationFor(string companyNumber, decimal invoiceAmount, decimal advancePercentage)
        {
            return 1;
        }
    }
    public class ConfidentialInvoiceService : IConfidentialInvoiceService
    {
        public IApplicationResult SubmitApplicationFor(CompanyDataRequest applicantData, decimal invoiceLedgerTotalValue, decimal advantagePercentage, decimal vatRate)
        {
            return new ApplicationResult() { ApplicationId = applicantData.CompanyNumber, Success = true, Errors = null };
        }
    }
    public class BusinessLoansService : IBusinessLoansService
    {
        public IApplicationResult SubmitApplicationFor(CompanyDataRequest applicantData, LoansRequest businessLoans)
        {
            return new ApplicationResult() {  ApplicationId = applicantData.CompanyNumber, Success=true, Errors = null};
        }
    }

    public class ApplicationResult : IApplicationResult
    {
        public int? ApplicationId { get ; set ; }
        public bool Success { get ; set ; }
        public IList<string> Errors { get ; set ; }
    }
}
