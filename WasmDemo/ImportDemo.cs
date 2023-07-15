
using WasmDemo;
using Wasmtime;

internal class ImportDemo
{
    public static void Run()
    {

        var wasmFile = "../../../../WasmLib/bin/Debug/net6.0/WasmLib.wasm";
        var goWasmFile = "../../../../wasm-module/go-wasm-module/go-wasm-module.wasm";
        var rustWasmFile = "../../../../wasm-module/rust-wasm-module/target/wasm32-wasi/release/rust_wasm_module.wasm";

        using var wasmHelper = new WasmHelper();

        wasmHelper.Linker.DefineFunction("env", "hello_from_env", () =>
        {
            Console.WriteLine("Hello from the Dotnet Host!");
        });

        using var goWasmModule = Module.FromFile(wasmHelper.Engine, goWasmFile);
        using var wasmLibModule = Module.FromFile(wasmHelper.Engine, wasmFile);

        wasmHelper.Linker.DefineModule(wasmHelper.Store, goWasmModule);

        var goWasmIns = wasmHelper.GetInstance(goWasmModule);
        var goMainFunc = goWasmIns.GetFunction("_start")!;
        goMainFunc.Invoke();
        var r = goWasmIns.GetFunction("main.add")?.Invoke(1, 3);

        var wasmLibIns = wasmHelper.GetInstance(wasmLibModule);
        var cSharpMainFunc = wasmLibIns.GetFunction("_start")!;
        cSharpMainFunc.Invoke();


        using var rustWasmModule = Module.FromFile(wasmHelper.Engine, rustWasmFile);
        var rustWasmIns = wasmHelper.GetInstance(rustWasmModule);
        var res = rustWasmIns.GetFunction("add")?.Invoke(4, 7);
        Console.WriteLine(res);
    }
}
