using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public Name Name { get; }
        
        public ClothingSize? ClothingSize { get; set; }
        
        public EmailAddress EmailAddress { get; set; }

        public EmailAddress? HrEmailAddress { get; set; }

        public List<MerchDelivery> MerchDeliveries { get; } = new List<MerchDelivery>();

        public Employee(Name name, EmailAddress email)
        {
            Name = name;
            SetEmailAddress(email);
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

        public void SetClothingSize(ClothingSize size)
        {
            ClothingSize = size;
        }

        public void AddMerchDelivery(MerchDelivery merchDelivery)
        {
            MerchDeliveries.Add(merchDelivery);
        }

        private static bool IsValidMail(string emailAddress)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");  
            var match = regex.Match(emailAddress);
            return match.Success;
        }
    }
}