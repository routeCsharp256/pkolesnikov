using System;
using System.Net.Mail;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public EmployeeId EmployeeId { get; }
        
        public Name Name { get; }
        
        public ClothingSize ClothingSize { get; }
        
        public EmailAddress EmailAddress { get; set; }
        
        public EmailAddress HrEmailAddress { get; set; }

        public Employee(EmployeeId id, Name name, ClothingSize size, EmailAddress email, EmailAddress hrEmail)
        {
            EmployeeId = id;
            Name = name;
            ClothingSize = size;
            SetEmailAddress(email);
            SetHrEmailAddress(hrEmail);
        }

        public void SetEmailAddress(EmailAddress email)
        {
            if (!IsValidMail(email.Value))
                throw new ArgumentException("Not valid email", nameof(email));

            EmailAddress = email;
        }
        
        public void SetHrEmailAddress(EmailAddress email)
        {
            if (!IsValidMail(email.Value))
                throw new ArgumentException("Not valid email", nameof(email));

            HrEmailAddress = email;
        }

        private bool IsValidMail(string emailAddress)
        {
            try {
                var mailAddress = new MailAddress(emailAddress);
                return mailAddress.Address == emailAddress;
            }
            catch {
                return false;
            }
        }
    }
}