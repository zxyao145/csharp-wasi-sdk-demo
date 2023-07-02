
using System.Text;
using WasmDemo;

internal class ExportDemo
{
    static string wasmFile = "../../../../WasmLib/bin/Debug/net6.0/WasmLib.wasm";

    public static void Run()
    {
        RunMain();
        RunCustomMethod();
        RunStringParam();
        RunIntArrayParam();
    }


    static void RunMain()
    {
        using var wasmHelper = new WasmHelper(wasmFile);
        var instance = wasmHelper.Instance;

        var main = instance.GetFunction("_start")!;
        main.Invoke();
    }


    static void RunCustomMethod()
    {
        using var wasmHelper = new WasmHelper(wasmFile);
        var instance = wasmHelper.Instance;
        var main = instance.GetFunction("_start")!;
        main.Invoke();


        var run = instance.GetAction("run")!;
        run.Invoke();
    }


    static void RunStringParam()
    {
        using var wasmHelper = new WasmHelper(wasmFile);
        var instance = wasmHelper.Instance;
        var main = instance.GetFunction("_start")!;
        main.Invoke();


        var address = CreateWasmString("hi wasm");
        var echoTimes = 5;

        var echo = instance.GetFunction("echo");
        echo?.Invoke(echoTimes, address);


        int CreateWasmString(string value)
        {
            value += "\0";

            var mem = instance.GetMemory("memory")!;
            var wasmMalloc = instance.GetFunction<int, int>("malloc")!;

            var len = Encoding.UTF8.GetByteCount(value);
            var startAddress = wasmMalloc.Invoke(len);
            mem.WriteString(startAddress, value, Encoding.UTF8);

            return startAddress;
        }
    }


    static void RunByteArrayParam()
    {
        using var wasmHelper = new WasmHelper(wasmFile);
        var instance = wasmHelper.Instance;
        var main = instance.GetFunction("_start")!;
        main.Invoke();


        // 调用 wasm
        var (start, byteLen) = CreateWasmArray(new[] { 4, 5 });
        var arrayParamFunc = instance.GetFunction<int, int, int>("byte_array_param")!;
        int res = arrayParamFunc.Invoke(start, byteLen);

        // 根据 wasm 返回的指针，获取数据
        var mem = instance.GetMemory("memory")!;
        long dataPointerStart = mem.ReadInt32(res);
        long dataLen = mem.ReadInt32(res + sizeof(int));
        // 根据 数据的指针 和 长度 解析数据
        byte[] data = new byte[dataLen];
        for (int i = 0; i < dataLen; i++)
        {
            byte b = mem.ReadByte(dataPointerStart + i);
            data[i] = b;
            Console.WriteLine($"byte array {i} is {b}");
        }


        (int start, int len) CreateWasmArray(int[] value)
        {
            var mem = instance.GetMemory("memory")!;
            var wasmMalloc = instance.GetFunction<int, int>("malloc")!;

            var len = value.Length * sizeof(int);
            var start = wasmMalloc.Invoke(len);
            Console.WriteLine($"{start}, {len}");
            var index = 0;
            for (int i = 0; i < len; i += sizeof(int))
            {
                mem!.WriteInt32(start + i, value[index++]);
            }

            return (start, len);
        }
    }


    static void RunIntArrayParam()
    {
        using var wasmHelper = new WasmHelper(wasmFile);
        var instance = wasmHelper.Instance;
        var main = instance.GetFunction("_start")!;
        main.Invoke();


        // 调用 wasm
        var (start, intLen) = CreateWasmArray(new[] { 4, 5 });
        var arrayParamFunc = instance.GetAction<int, int>("int_array_param")!;
        arrayParamFunc.Invoke(start, intLen);


        (int start, int len) CreateWasmArray(int[] value)
        {
            var mem = instance.GetMemory("memory")!;
            var wasmMalloc = instance.GetFunction<int, int>("malloc")!;

            var len = value.Length * sizeof(int);
            var start = wasmMalloc.Invoke(len);
            var index = 0;
            for (int i = 0; i < len; i += sizeof(int))
            {
                mem!.WriteInt32(start + i, value[index++]);
            }

            return (start, value.Length);
        }
    }
}
