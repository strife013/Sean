namespace System
{
    //注意:不要再动此类的数据结构，已在很多地方用作元数据处理

    public class SimpleData<Tid>
    {
        public Tid id { get; set; }

        public string value { get; set; }

        public string text { get; set; }

        public object tag { get; set; }
    }

    public class SimpleData : SimpleData<long>
    {
        public SimpleData() { }

        public SimpleData(long id, string value)
        {
            this.id = id;
            this.value = value;
        }

        public SimpleData(string text, string value)
        {
            this.text = text;
            this.value = value;
        }
    }
}
