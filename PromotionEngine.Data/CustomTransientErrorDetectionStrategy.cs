using Microsoft.Practices.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Data
{
    /// <summary>
    /// Responsible for detecting specific transient conditions
    /// </summary>
    public class CustomTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        private const string sqlErrorCodes = "-2146232060,40197,40501,40613,49918,49919,49920";

        /// <summary>
        /// Determines whether the specified exception represents a transient failure that can be compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>True if the specified exception is considered as transient, otherwise false.</returns>
        public bool IsTransient(Exception ex)
        {
            //Refer: https://docs.microsoft.com/en-us/azure/sql-database/sql-database-develop-error-messages (Transient fault error codes)
            if (sqlErrorCodes.Contains(ex.HResult.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
