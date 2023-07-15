using WasmDemo;
using Module = Wasmtime.Module;

// ExportDemo.Run();
// ImportDemo.Run();

var watText = @"(module
  (type (;0;) (func (param i32 i32) (result i32)))
  (type (;1;) (func))
  (func $add (type 0) (param i32 i32) (result i32)
    local.get 1
    local.get 0
    i32.add)
  (func $dummy (type 1))
  (func $__wasm_call_dtors (type 1)
    call $dummy
    call $dummy)
  (func $add.command_export (type 0) (param i32 i32) (result i32)
    local.get 0
    local.get 1
    call $add
    call $__wasm_call_dtors)
  (table (;0;) 1 1 funcref)
  (memory (;0;) 16)
  (global $__stack_pointer (mut i32) (i32.const 1048576))
  (export ""memory"" (memory 0))
  (export ""add"" (func $add.command_export)))";


using var wasmHelper = new WasmHelper();

using var wasmModule = Module.FromText(wasmHelper.Engine, "module-name", watText);

wasmHelper.Linker.DefineModule(wasmHelper.Store, wasmModule);

var wasmIns = wasmHelper.GetInstance(wasmModule);
var res = wasmIns.GetFunction("add")?.Invoke(4, 7);
Console.WriteLine(res);
