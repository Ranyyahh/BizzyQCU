using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BizzyQCU.Models
{
    [Serializable]
    public class EnterpriseSummary
    {
        public string PhotoDataUrl { get; set; }
        public string QrDataUrl { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Gcash { get; set; }
        public string Role { get; set; }
    }

    [Serializable]
    public class ManagerSummary
    {
        public string Name { get; set; }
        public string Section { get; set; }
        public string StudentId { get; set; }
        public string ContactNumber { get; set; }
    }

    [Serializable]
    public class EnterpriseStatsSummary
    {
        public int OrdersCompleted { get; set; }
        public int ProductsListed { get; set; }
        public decimal TotalSales { get; set; }
    }

    [Serializable]
    public class EnterpriseDashboardViewModel
    {
        public EnterpriseSummary Enterprise { get; set; }
        public ManagerSummary Manager { get; set; }
        public EnterpriseStatsSummary Stats { get; set; }
    }

    [Serializable]
    public class UserProfileViewModel
    {
        public string PhotoDataUrl { get; set; }
        public string QrDataUrl { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseType { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string ManagerName { get; set; }
        public string StudentId { get; set; }
        public string Section { get; set; }
        public string ManagerContactNumber { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class AccountSettingsPageViewModel
    {
        public UserProfileViewModel Profile { get; set; }
        public ChangePasswordViewModel PasswordChange { get; set; }
        public bool ReopenPasswordModal { get; set; }
    }

    [Serializable]
    public class TransactionRecordViewModel
    {
        public string UserName { get; set; }
        public string Product { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
    }

    public class TransactionsPageViewModel
    {
        public string SearchTerm { get; set; }
        public IList<TransactionRecordViewModel> Transactions { get; set; }
    }
}
