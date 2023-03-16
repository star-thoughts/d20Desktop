using System.Globalization;

namespace Fiction
{
    public static class Exceptions
    {
        #region Constructors
        static Exceptions()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }
        #endregion
        #region Events
        /// <summary>
        /// Event that is triggered when an exception is considered ignored
        /// </summary>
        public static event EventHandler<IgnoredExceptionEventArgs> IgnoredException;
        /// <summary>
        /// Triggers an IgnoredException event
        /// </summary>
        /// <param name="exc">Exception that was ignored</param>
        public static void RaiseIgnoredException(Exception exc)
        {
            RaiseIgnoredException(exc, String.Empty);
        }
        /// <summary>
        /// Triggers an IgnoredException event
        /// </summary>
        /// <param name="exc">Exception that was ignored</param>
        /// <param name="informationFormat">Format string for extra information to write</param>
        /// <param name="info">Extra information to write</param>
        public static void RaiseIgnoredException(Exception exc, string informationFormat, params string[] info)
        {
            string information = informationFormat;
            if (info != null && info.Length > 0)
                information = string.Format(CultureInfo.InvariantCulture, informationFormat, info);

            RaiseIgnoredException(exc, information);
        }
        /// <summary>
        /// Triggers an IgnoredException event
        /// </summary>
        /// <param name="exc">Exception that was ignored</param>
        /// <param name="information">Extra information to write</param>
        public static void RaiseIgnoredException(Exception exc, string information)
        {
            try
            {
                IgnoredException?.Invoke(null, new IgnoredExceptionEventArgs(exc));
            }
            catch { }
        }

        /// <summary>
        /// Called to deal with an unhandled exception in an application
        /// </summary>
        /// <param name="exc">Exception that went unhandled.</param>
        public static void RaiseUnhandledException(Exception exc)
        {
        }
        #endregion
        #region Testers
        public static void ThrowIfArgumentDefault<T>(T value, string property) where T : struct
        {
            if (value.Equals(default(T)))
                ThrowArgumentException(Resources.CommonResources.ArgumentDefaultException, property);
        }
        /// <summary>
        /// Throws an ArgumentNullException if value is null
        /// </summary>
        /// <param name="value">Value to test</param>
        public static void ThrowIfArgumentNull(object value, string property)
        {
            if (value == null)
                ThrowNullArgumentException(property);
        }
        public static void ThrowIfArgumentNullOrEmpty(string value, string property)
        {
            if (value == null)
                ThrowNullArgumentException(property);
            if (value == string.Empty)
                ThrowArgumentException(Fiction.Resources.CommonResources.ExceptionStringEmpty, property);
        }
        public static void ThrowIfArgumentNegative(int value, string property)
        {
            if (value < 0)
                ThrowArgumentException(Fiction.Resources.CommonResources.ArgumentNegativeException, property);
        }
        public static void ThrowIfArgumentNegative(long value, string property)
        {
            if (value < 0)
                ThrowArgumentException(Fiction.Resources.CommonResources.ArgumentNegativeException, property);
        }
        /// <summary>
        /// Tests the given value against a minimum value and throws if it is not met
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="argument">Argument value</param>
        /// <param name="minValue">Minimum argument value</param>
        /// <param name="property">Expression that evaluates to the property to test</param>
        public static void ThrowIfArgumentNotMinValue(int argument, int minValue, string property)
        {
            if (argument < minValue)
            {
                ThrowArgumentOutOfRangeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Fiction.Resources.CommonResources.ArgumentMinimumException,
                        minValue),
                    property);
            }
        }
        #endregion
        #region Throwers
        /// <summary>
        /// Throws a NullArgumentException
        /// </summary>
        public static void ThrowNullArgumentException(string property)
        {
            throw new ArgumentNullException(property);
        }
        /// <summary>
        /// Throws an ArgumentException
        /// </summary>
        /// <param name="message">Exception message</param>
        public static void ThrowArgumentException(string message, string property)
        {
            throw new ArgumentException(message, property);
        }
        /// <summary>
        /// Throws an InvalidOperationException
        /// </summary>
        /// <param name="message">Message to throw</param>
        public static void ThrowInvalidOperation(string message)
        {
            throw new InvalidOperationException(message);
        }
        /// <summary>
        /// Throws an InvalidOperationException with a default message
        /// </summary>
        public static void ThrowInvalidOperation()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Throws an ArgumentOutOfRange exception with the given message
        /// </summary>
        /// <param name="message">Message for the exception</param>
        /// <param name="property">Property the exception is for</param>
        public static void ThrowArgumentOutOfRangeException(string message, string property)
        {
            throw new ArgumentOutOfRangeException(property, message);
        }
        #endregion
        #region Helpers
        /// <summary>
        /// Runs the given action and stops any exceptions from bubbling out of it
        /// </summary>
        /// <param name="action">Action to run</param>
        public static void FailSafeMethodCall(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exc)
            {
                RaiseIgnoredException(exc);
            }
        }
        public static T FailSafeMethodCall<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception exc)
            {
                RaiseIgnoredException(exc);
            }

            return default;
        }
        /// <summary>
        /// Runs the given action and stops any exceptions from bubbling out of it
        /// </summary>
        /// <param name="action">Action to run</param>
        public static async Task FailSafeMethodCall(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception exc)
            {
                RaiseIgnoredException(exc);
            }
        }

        /// <summary>
        /// Retries an action up to 5 times when an exception occurs
        /// </summary>
        /// <param name="action">Action to retry</param>
        public static void RetryOnException(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            bool success = false;
            List<Exception> exceptions = new List<Exception>();
            int count = 0;

            while (!success)
            {
                success = true;
                try
                {
                    action();
                }
                catch (Exception exc)
                {
                    success = false;
                    exceptions.Add(exc);
                    count++;

                    if (count >= 5)
                        throw new AggregateException("Failed maximum retry count in RetryOnExceptionAttribute.", exceptions);

                    Task.Delay(200).Wait();
                }
            }
        }
        /// <summary>
        /// Retries an action up to 5 times when an exception occurs
        /// </summary>
        /// <typeparam name="T">Type of result of the action</typeparam>
        /// <param name="action">Action to retry</param>
        /// <returns>Result of the action</returns>
        public static T RetryOnException<T>(Func<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            bool success = false;
            List<Exception> exceptions = new List<Exception>();
            int count = 0;
            T result = default(T);

            while (!success)
            {
                success = true;
                try
                {
                    result = action();
                }
                catch (Exception exc)
                {
                    success = false;
                    exceptions.Add(exc);
                    count++;

                    if (count >= 5)
                        throw new AggregateException("Failed maximum retry count in RetryOnExceptionAttribute.", exceptions);

                    Task.Delay(200).Wait();
                }
            }

            return result;
        }

        /// <summary>
        /// Retries an action up to 5 times when an exception occurs
        /// </summary>
        /// <typeparam name="TR">Type of result of the action</typeparam>
        /// <typeparam name="T1">Type of parameter to the action</typeparam>
        /// <param name="action">Action to perform</param>
        /// <param name="parameter">Parameter to pass to the action</param>
        /// <returns>Result of the action</returns>
        /// <exception cref="AggregateException">Each run of the given method resulted in an exception being thrown, and are provided in this exception.</exception>
        public static async Task RetryOnExceptionAsync<T1>(Action<T1> action, T1 parameter)
        {
            int maxCount = 5;
            int delayMilliseconds = 200;
            bool success = false;
            List<Exception> exceptions = new List<Exception>();
            int count = 0;

            while (!success && count < maxCount)
            {
                success = true;
                try
                {
                    await Task.Factory.StartNew(p => action((T1)p), parameter, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Current);
                }
                catch (Exception exc)
                {
                    success = false;
                    exceptions.Add(exc);
                    count++;

                    await Task.Delay(delayMilliseconds);
                }
            }
            if (count >= maxCount)
                throw new AggregateException("Failed maximum retry count in " + nameof(RetryOnExceptionAsync) + ".", exceptions);
        }
        /// <summary>
        /// Retries an action up to 5 times when an exception occurs
        /// </summary>
        /// <typeparam name="TR">Type of result of the action</typeparam>
        /// <typeparam name="T1">Type of parameter to the action</typeparam>
        /// <param name="action">Action to perform</param>
        /// <param name="parameter">Parameter to pass to the action</param>
        /// <returns>Result of the action</returns>
        /// <exception cref="AggregateException">Each run of the given method resulted in an exception being thrown, and are provided in this exception.</exception>
        public static async Task<TR> RetryOnExceptionAsync<TR, T1>(Func<T1, TR> action, T1 parameter)
        {
            int maxCount = 5;
            int delayMilliseconds = 200;
            bool success = false;
            List<Exception> exceptions = new List<Exception>();
            int count = 0;

            while (!success && count < maxCount)
            {
                success = true;
                try
                {
                    return await Task.Factory.StartNew(p => action((T1)p), parameter, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Current);
                }
                catch (Exception exc)
                {
                    success = false;
                    exceptions.Add(exc);
                    count++;

                    await Task.Delay(delayMilliseconds);
                }
            }
            throw new AggregateException("Failed maximum retry count in " + nameof(RetryOnExceptionAsync) + ".", exceptions);
        }
        /// <summary>
        /// Retries an action up to 5 times when an exception occurs
        /// </summary>
        /// <typeparam name="TR">Type of result of the action</typeparam>
        /// <typeparam name="T1">Type of parameter to the action</typeparam>
        /// <param name="action">Action to perform</param>
        /// <param name="parameter">Parameter to pass to the action</param>
        /// <param name="token">Token for cancellation of progress</param>
        /// <returns>Result of the action</returns>
        /// <exception cref="OperationCanceledException"><see cref="CancellationToken"/> passed in entered a cancelled state</exception>
        /// <exception cref="AggregateException">Each run of the given method resulted in an exception being thrown, and are provided in this exception.</exception>
        public static async Task<TR> RetryOnExceptionAsync<TR, T1>(Func<T1, TR> action, T1 parameter, CancellationToken token)
        {
            int maxCount = 5;
            int delayMilliseconds = 200;
            bool success = false;
            List<Exception> exceptions = new List<Exception>();
            int count = 0;

            while (!success && count < maxCount)
            {
                success = true;
                token.ThrowIfCancellationRequested();
                try
                {
                    return await Task.Factory.StartNew(p => action((T1)p), parameter, token, TaskCreationOptions.None, TaskScheduler.Current);
                }
                catch (Exception exc)
                {
                    success = false;
                    exceptions.Add(exc);
                    count++;

                    await Task.Delay(delayMilliseconds, token);
                }
            }
            throw new AggregateException("Failed maximum retry count in RetryOnExceptionAttribute.", exceptions);
        }
        /// <summary>
        /// Gets whether or not the exception can be serialized using a DataContractSerializer
        /// </summary>
        /// <param name="exc">Exception to test</param>
        /// <returns>Whether or not the exception can be serialized</returns>
        public static bool IsSerializable(this Exception exc)
        {
            if (exc.GetType().GetCustomAttributes(typeof(SerializableAttribute), false).Any())
            {
                if (exc is AggregateException)
                    return ((AggregateException)exc).InnerExceptions.All(p => p.IsSerializable());

                if (exc.InnerException != null)
                    return exc.InnerException.IsSerializable();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a list of child exceptions of this exception
        /// </summary>
        /// <param name="exc">Exception to get children from</param>
        /// <returns>List of child exceptions</returns>
        public static Exception[] ToArray(this Exception exc)
        {
            AggregateException aggregate = exc as AggregateException;
            if (aggregate != null)
                return aggregate.InnerExceptions.ToArray();
            else if (exc.InnerException != null)
                return new Exception[] { exc.InnerException };
            return Array.Empty<Exception>();
        }
        #endregion
        #region Event Handlers
        static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Raise an ignored event
            RaiseIgnoredException(e.Exception);
            // Prevent the exception from tearing down the application
            e.SetObserved();
        }

        #endregion
    }
}
