using System;
using System.Text;

using MessagePack;

namespace MessagePackGetterRepro
{
    class Program
    {
        static void Main(string[] args)
        {
            MessagePackSerializerOptions lz4Options =
                MessagePackSerializerOptions.Standard.WithCompression(
                    MessagePackCompression.Lz4Block
                );

            var ro = new TestMessageWithReadOnlyProperty();
            byte[] serialized = MessagePackSerializer.Serialize(ro, lz4Options);

            Console.WriteLine($"Message with read-only property serializes to:             {ByteArrayToString(serialized)}");

            var rw = new TestMessageWithReadWriteProperties();
            serialized = MessagePackSerializer.Serialize(rw, lz4Options);

            Console.WriteLine($"Message with read-write properties serializes to:          {ByteArrayToString(serialized)}");

            var get = new TestMessageWithGetter();
            serialized = MessagePackSerializer.Serialize(get, lz4Options);

            Console.WriteLine($"Message with constant properties as getters serializes to: {ByteArrayToString(serialized)}");
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString().TrimEnd();
        }
    }

    [MessagePackObject]
    public class TestMessageWithReadOnlyProperty
    {
        [Key(0)]
        public int Property1 = 123;

        [Key(1)]
        public readonly int Property2 = 456;

        [Key(2)]
        public int Property3 = 789;
    }

    [MessagePackObject]
    public class TestMessageWithReadWriteProperties
    {
        [Key(0)]
        public int Property1 = 123;

        [Key(1)]
        public int Property2 = 456;

        [Key(2)]
        public int Property3 = 789;
    }

    [MessagePackObject]
    public class TestMessageWithGetter
    {
        [Key(0)]
        public int Property1 = 123;

        [Key(1)]
        public int Property2 { get { return 456; } }

        [Key(2)]
        public int Property3 = 789;
    }
}
