namespace pizzaperks.Data
{
    public class DataUtility
    {
        public DataUtility() { }

        public static Task ManageDataAsync(IHost host)
        {


            using var svcScope = host.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;



            return Task.FromResult(0);
        }
    }
}
