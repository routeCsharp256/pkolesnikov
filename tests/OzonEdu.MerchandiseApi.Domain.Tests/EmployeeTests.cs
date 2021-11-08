﻿using System;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using Xunit;

namespace OzonEdu.MerchandiseApi.Domain.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void SetEmail_CorrectEmail_Success()
        {
            var emailAddress = new EmailAddress("ya@ya.ru");
            
            var employee = new Employee(new Name("Name"), emailAddress);
             
            Assert.Equal(emailAddress, employee.EmailAddress);
        }
        
        [Fact]
        public void SetEmail_NotCorrectEmail_ArgumentException()
        {
            var emailAddress = new EmailAddress("ya@ya");
            
            Assert.Throws<ArgumentException>(() => new Employee(
                new Name("Name"),
                emailAddress));
        }
    }
}