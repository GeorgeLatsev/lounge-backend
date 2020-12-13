namespace Lounge.Services.Notifications.API.Notifications.Methods
{
    public abstract class BaseMethod
    {
        private readonly object[] _args;

        protected BaseMethod(params object[] args)
        {
            _args = args;
        }

        public string GetName() => GetType().Name;

        public object[] GetArgs() => _args;
    }
}
