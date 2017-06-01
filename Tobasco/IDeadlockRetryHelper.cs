using System;

namespace Tobasco
{
    public interface IDeadlockRetryHelper
    {
        void ExecuteWithRetry(Action methodToUseInTransaction);

        void ExecuteWithRetry(Action methodToUseInTransaction, int retries);
    }
}