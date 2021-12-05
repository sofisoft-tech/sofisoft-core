namespace Sofisoft.MongoDb.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UseTransactionAttribute : Attribute
    {
        public UseTransactionAttribute() { }
    }
}