using AcmeSoft.Data.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeSoft.Data.Repository
{
    public class EmployeeMaintenanceRepository
    {
        private readonly IDatabaseContext _dbContext;

        public EmployeeMaintenanceRepository(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> AllEmployees()
        {
            var sql = "SELECT [EmployeeId], [PersonId], [EmployeeNum], [EmployedDate], [TerminatedDate] FROM Employee";
            using (IDbConnection conn = _dbContext.Connection)
            {
                return await conn.QueryAsync<Employee>(sql);
            }
        }

        public async Task<Employee> FindEmployee(int employeeId)
        {
            var sql = $"SELECT [EmployeeId], [PersonId], [EmployeeNum], [EmployedDate], [TerminatedDate] FROM Employee WHERE [EmployeeId] = {employeeId}";
            using (IDbConnection conn = _dbContext.Connection)
            {
                return await conn.QueryFirstOrDefaultAsync<Employee>(sql);
            }
        }

        public async Task<IEnumerable<Employee>> FindEmployeesByPerson(int personId)
        {
            var sql = $"SELECT [EmployeeId], [PersonId], [EmployeeNum], [EmployedDate], [TerminatedDate] FROM Employee WHERE [PersonId] = {personId}";
            using (IDbConnection conn = _dbContext.Connection)
            {
                return await conn.QueryAsync<Employee>(sql);
            }
        }

        public async Task UpsertEmployee(Employee employee)
        {
            var sql = $"IF EXISTS (SELECT 1 FROM [Employee] WHERE EmployeeID = {employee.EmployeeId}) UPDATE [Employee] SET [PersonId] = @PersonId, [EmployeeNum] = @EmployeeNum, [EmployedDate] = @EmployedDate, [TerminatedDate] = @TerminatedDate WHERE EmployeeId = {employee.EmployeeId} ELSE INSERT INTO [Employee] ([PersonId],[EmployeeNum],[EmployedDate],[TerminatedDate]) VALUES (@PersonId, @EmployeeNum, @EmployedDate, @TerminatedDate)";
            using (IDbConnection conn = _dbContext.Connection)
            {
                await conn.ExecuteAsync(sql, employee);
            }
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var sql = $"DELETE [Employee] WHERE EmployeeID = {employeeId}";
            using (IDbConnection conn = _dbContext.Connection)
            {
                await conn.ExecuteAsync(sql);
            }
        }

        public async Task<IEnumerable<Person>> AllPeopleAndEmployees()
        {
            var sql = $"SELECT P.[PersonId], P.[LastName],P.[FirstName],P.[BirthDate],E.[EmployeeId],E.[PersonId],E.[EmployeeNum],E.[EmployedDate],E.[TerminatedDate] FROM [Person] P LEFT OUTER JOIN [Employee] E ON E.PersonId = P.PersonId";

            using (IDbConnection conn = _dbContext.Connection)
            {
                var personDict = new Dictionary<int, Person>();

                return await conn.QueryAsync<Person, Employee, Person>(sql,
                    (person, employee) =>
                    {
                        Person personEntry;


                        if (!personDict.TryGetValue(person.PersonId, out personEntry))
                        {
                            personEntry = person;
                            personEntry.Employees = new List<Employee>();
                            personDict.Add(personEntry.PersonId, personEntry);
                        }


                        personEntry.Employees.Add(employee);
                        return personEntry;
                    },
                    splitOn: "EmployeeId");
            }
        }

        //TODO: Remove these
        //public async Task<IEnumerable<Person>> AllPeople()
        //{
        //    var sql = "SELECT [PersonId], [LastName], [FirstName], [BirthDate] FROM Person";
        //    using (IDbConnection conn = _dbContext.Connection)
        //    {
        //        return await conn.QueryAsync<Person>(sql);
        //    }
        //}

        //public async Task<Person> FindPerson(int personId)
        //{
        //    var sql = $"SELECT [PersonId], [LastName], [FirstName], [BirthDate] FROM Person WHERE [PersonId] = {personId}";
        //    using (IDbConnection conn = _dbContext.Connection)
        //    {
        //        return await conn.QueryFirstOrDefaultAsync<Person>(sql);
        //    }
        //}

        //public async Task<IEnumerable<Employee>> AllEmployees()
        //{
        //    var sql = "SELECT [EmployeeId], [PersonId], [EmployeeNum], [EmployedDate], [TerminatedDate] FROM Employee";
        //    using (IDbConnection conn = _dbContext.Connection)
        //    {
        //        return await conn.QueryAsync<Employee>(sql);
        //    }

        //}
    }
}
