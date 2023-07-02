using WasmLib;

Console.WriteLine("Hello from Main!");

WasmImport.HelloFromEnv();

int res = WasmImport.GoWasmAdd(10,1);
Console.WriteLine($"GoWasmAdd 10+1={res}");