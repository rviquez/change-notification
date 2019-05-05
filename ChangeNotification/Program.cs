using System;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace ChangeNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            //var connectionString = "data source=.;initial catalog=TestDB;Trusted_Connection=False;";
            var connectionString = "Server=localhost,1433;Database=TestDB;User Id=sa;Password=Ev@27Rip;";
            using (var tableDependency = new SqlTableDependency<Customer>(connectionString, "Customers"))
            {
                tableDependency.OnChanged += TableDependency_Changed;
                tableDependency.OnError += TableDependency_OnError;

                tableDependency.Start();
                Console.WriteLine("Waiting for receiving notifications...");
                Console.WriteLine("Press any key to stop");
                Console.ReadKey();
                tableDependency.Stop();

            }
        }

        private static void TableDependency_Changed(object sender, RecordChangedEventArgs<Customer> e)
        {
            Console.WriteLine(Environment.NewLine);

            if (e.ChangeType != ChangeType.None)
            {
                var changedEntity = e.Entity;
                Console.WriteLine("DML operation: " + e.ChangeType);
                Console.WriteLine("Id: " + changedEntity.Id);
                Console.WriteLine("Name: " + changedEntity.Name);
                Console.WriteLine("Surname: " + changedEntity.Surname);
                Console.WriteLine(Environment.NewLine);

            }
        }

        private static void TableDependency_OnError(object sender, ErrorEventArgs e)
        {
            Exception ex = e.Error;
            throw ex;
        }

    }
}
