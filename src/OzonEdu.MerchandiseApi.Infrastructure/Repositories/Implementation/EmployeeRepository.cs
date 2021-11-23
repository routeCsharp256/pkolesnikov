using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Queries;
using ClothingSize = OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate.ClothingSize;
using Domain = OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate.Employee;
using MerchDelivery = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDelivery;
using MerchDeliveryStatus = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDeliveryStatus;
using MerchPackType = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchPackType;

#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const int Timeout = 5;
        
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly IMerchDeliveryRepository _merchDeliveryRepository;

        public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker,
            IMerchDeliveryRepository merchDeliveryRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
            _merchDeliveryRepository = merchDeliveryRepository;
        }

        public async Task<Employee?> CreateAsync(Employee itemToCreate, CancellationToken token)
        {
            var parameters = new
            {
                Name = itemToCreate.Name.Value,
                ClothingSizeId = itemToCreate.ClothingSize?.Id,
                EmailAddress = itemToCreate.EmailAddress?.Value,
                ManagerEmailAddress = itemToCreate.ManagerEmailAddress?.Value
            };

            var commandDefinition = new CommandDefinition(
                EmployeeQuery.Insert,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToCreate);
            return itemToCreate;
        }

        public async Task<Employee?> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            var parameters = new
            {
                EmployeeId = itemToUpdate.Id,
                Name = itemToUpdate.Name.Value,
                ClothingSizeId = itemToUpdate.ClothingSize?.Id,
                EmailAddress = itemToUpdate.EmailAddress?.Value,
                ManagerEmailAddress = itemToUpdate.ManagerEmailAddress?.Value
            };
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.Update,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }

        public async Task<Employee?> FindAsync(int id, CancellationToken token = default)
        {
            var parameters = new
            {
                Id = id
            };
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.FilterById,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection.QueryAsync<Models.Employee, Models.ClothingSize, Employee>(
                commandDefinition,
                (employee, size) => new Employee(
                    new Name(employee.Name),
                    employee.EmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.EmailAddress),
                    employee.ManagerEmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.ManagerEmailAddress),
                    size?.Id is null 
                        ? null 
                        : new ClothingSize(size.Id.Value, size.Name ?? string.Empty)));

            return employees.FirstOrDefault();
        }

        public async Task<Employee?> FindAsync(string email, CancellationToken token = default)
        {
            var parameters = new
            {
                Email = email
            };
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.FilterByEmail,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection.QueryAsync<Models.Employee, Models.ClothingSize, Employee>(
                commandDefinition,
                (employee, size) => new Employee(
                    new Name(employee.Name),
                    employee.EmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.EmailAddress),
                    employee.ManagerEmailAddress is null
                        ? null 
                        : new EmailAddress(employee.ManagerEmailAddress),
                    size is null ? null : new ClothingSize(size.Id.Value, size.Name)));

            return employees.FirstOrDefault();
        }

        public async Task<IEnumerable<Employee>> GetByMerchDeliveryStatusAndSkuCollection(int statusId, 
            IEnumerable<long> skuIdCollection,
            CancellationToken token = default)
        {
            var parameters = new
            {
                StatusId = statusId,
                SkuIds = skuIdCollection.ToArray()
            };
            
            var commandDefinition = new CommandDefinition(
                EmployeeQuery.FilterByMerchDeliveryStatusAndSkuCollection,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection.QueryAsync<Models.Employee, Models.ClothingSize, Employee>(
                commandDefinition,
                (employee, size) => new Employee(
                    new Name(employee.Name),
                    employee.EmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.EmailAddress),
                    employee.ManagerEmailAddress is null
                        ? null 
                        : new EmailAddress(employee.ManagerEmailAddress),
                    size is null ? null : new ClothingSize(size.Id.Value, size.Name)));

            return employees;
        }
    }
}