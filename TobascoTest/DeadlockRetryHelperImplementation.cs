using System;
using System.Data.SqlClient;
using System.Transactions;
using Tobasco;

namespace TobascoTest
{
    public class DeadlockRetryHelperImplementation : IDeadlockRetryHelper
    {
        private const int SqlDeadLockExceptionNumber1205 = 1205;

        public void ExecuteWithRetry(Action repositoryMethod)
        {
            ExecuteWithRetry(repositoryMethod, 3);
        }

        public void ExecuteWithRetry(Action repositoryMethod, int maxRetries)
        {
            ExecuteWithDeadlockCatch(repositoryMethod, maxRetries);
        }

        private void ExecuteWithDeadlockCatch(Action repositoryMethod, int maxRetries)
        {
            if (HasAmbientTransaction())
            {
                repositoryMethod();
            }

            int retries = 0;

            while (retries < maxRetries)
            {
                try
                {
                    using (var ts = new TransactionScope())
                    {
                        repositoryMethod();
                        ts.Complete();
                        return;
                    }
                }
                catch (Exception e)
                {
                    if (IsSqlDeadlock(e))
                    {
                        retries++;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            repositoryMethod();
        }

        public bool HasAmbientTransaction()
        {
            return Transaction.Current != null;
        }

        private bool IsSqlDeadlock(Exception exception)
        {
            if (exception == null)
            {
                return false;
            }

            var sqlException = exception as SqlException;

            if (sqlException != null && sqlException.Number == SqlDeadLockExceptionNumber1205)
            {
                return true;
            }

            if (exception.InnerException != null)
            {
                return IsSqlDeadlock(exception.InnerException);
            }

            return false;
        }
    }
}