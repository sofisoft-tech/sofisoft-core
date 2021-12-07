namespace Sofisoft.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TransactionAttribute : Attribute
    {
        public TransactionAttribute() { }
    }
}