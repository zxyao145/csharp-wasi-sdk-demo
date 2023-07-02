namespace WasmLib;

public class WasmClass
{
    public static void Run()
    {
        Console.WriteLine("Hello from WasmClass.Run");
    }

    public static void Echo(int times, string msg)
    {
        for (var i = 0; i < times; i++)
        {
            Console.WriteLine($"Echo: {msg}");
        }
    }


    public static IntPtr ByteArrayParam(byte[] bytes)
    {
        // 将 byte[] 转为目标类型 int[]
        var intLen = bytes.Length / sizeof(int);
        var intArr = new int[intLen];
        for (var index = 0; index < intLen; index++)
        {
            intArr[index] = BitConverter.ToInt32(bytes, index * sizeof(int));
        }

        // 输出 int[]
        foreach (var nr in intArr)
        {
            Console.WriteLine($"ArrayParam val:{nr}");
        }

        // 准备返回的数据
        byte[] returnData = { 0x05, 0x01, 0x07 };

        unsafe
        {
            // 获取返回数据的指针
            int pointAddr = 0;
            fixed (byte* p = returnData)
            {
                IntPtr pn = (IntPtr)p;
                pointAddr = pn.ToInt32();
            }

            Console.WriteLine("data pointAddr:" + pointAddr);

            // 将返回数据的指针和长度封装为一个数组，并将其指针返回给调用方
            int[] returnDataInfo = new[]
            {
                pointAddr, returnData.Length
            };

            fixed (int* p = returnDataInfo)
            {
                IntPtr pn = (IntPtr)p;
                Console.WriteLine("return result pointer:" + pn);
                return pn;
            }
        }
    }


    public static void IntArrayParam(int[] intArr)
    {
        Console.WriteLine("IntArrayParam len:" + intArr.Length);
        foreach (var nr in intArr)
        {
            Console.WriteLine($"IntArrayParam val:{nr}");
        }
    }


    public static void RunImport()
    {
        Console.WriteLine("Hello from WasmClass.Run");
    }
}