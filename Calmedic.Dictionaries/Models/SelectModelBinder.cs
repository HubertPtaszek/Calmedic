namespace Calmedic.Utils
{
    public class SelectModelBinder
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class SelectModelBinder<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
    }
}
